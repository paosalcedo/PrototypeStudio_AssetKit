using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
	[SerializeField]private GameObject _playerModel;
	[SerializeField] private Player _player;
	[SerializeField] private Image _crosshair;

	public Image Crosshair
	{
		get { return _crosshair; }
		set { _crosshair = value; }
	}

	public enum GameState
	{
		Intro,
		Game,
		Authorship, 
		End
	}

	public static Main instance;

	public enum Mode
	{
		Original,
		CopyPaste
	}

	public Mode _mode;

	private FSM<Main> _fsm;
	public GameState gameState; 

	public InputField titleField;
	public InputField poemField;
	public InputField authorField;

	public GameObject restartButton;
	public GameObject controlsTextGO;
	
	public static string title;
	public static string pastedPoem;
	public static string poemText;
	public int poemNum;

	public TextMeshProUGUI poem;
	public TextMeshProUGUI poemBG;

	private GameObject wordsHolder;
	private TextReaderScript textReader;
	private WordEmitter wordEmitter;
	private Vector3 poemPosition;
	private float poemY = 0;
	
	//state-based gameobjects
	[SerializeField] private GameObject intro;
	[SerializeField] private GameObject game;
	[SerializeField] private GameObject authorship;
	[SerializeField] private GameObject end;
	
	// Use this for initialization
	void Start ()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}

		_fsm = new FSM<Main>(this);
 		wordEmitter = FindObjectOfType<WordEmitter>();
		wordsHolder = GameObject.Find("Words");
		textReader = FindObjectOfType<TextReaderScript>();
		if (poemNum != 0)
		{
			++poemNum;		
		}
		_fsm.TransitionTo<IntroState>();
		gameState = GameState.Intro;
 	}
	
	// Update is called once per frame
	void Update () {
		_fsm.Update();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		switch (gameState)
		{
			case GameState.Intro:
				
				break;
			case GameState.Game:
				
				break;
			case GameState.Authorship:
							//poemText is updated here.											
				break;
			case GameState.End:
				
				ScrollPoem();
 				break;
		}
		
	}
	
	private void ScrollPoem()
	{
		poemY = poem.rectTransform.position.y;
 		poemY += Player.instance.player.GetAxis("ScrollText") * 5f;
 		poem.rectTransform.position = new Vector3(poem.rectTransform.position.x, poemY, poem.rectTransform.position.z);
	}
	
	public void RecordTitleAndName()
	{
		title = titleField.text;
		pastedPoem = poemField.text;
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + pastedPoem, titleField.text, true);
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + pastedPoem, "\nA poem by " + poemField.text, true);
		textReader.ReadPoemOld();
		wordEmitter.Setup();
		gameState = GameState.Game;
	}
	
	//INTRO FUNCTIONS
	
	public void StartGameScene()
	{
		Debug.Log("startgamescene called!");
		pastedPoem = poemField.text;
		if (_mode == Mode.Original)
		{
			textReader.ReadPoemOld();		
		}
		else
		{
			textReader.ReadPoem();
		}
		wordEmitter.Setup();
		_fsm.TransitionTo<GameplayState>();
 		gameState = GameState.Game;
	}
	
	//GAME FUNCTIONS
	public void AppendAuthorNameToPoemText()
	{
		if (_mode == Mode.Original)
		{
			if (pastedPoem == "")
			{
				poemText = "by " + "the Unnamed Poet" + "\n" + poemText;
			}
			else
			{
				poemText = "by " + pastedPoem + "\n" + poemText;
			}
		}
	}

	public void AppendTitleAndAuthorToPoemText()
	{
		poemText = titleField.text + "\n" + "by " + authorField.text + "\n" + poemText;
		TextUtilities.WriteStringToFile(Application.dataPath, titleField.text + " by " + authorField.text + ".txt", "\n" + poemText, true);
		_fsm.TransitionTo<EndState>();
	}

	//AUTHORSHIP FUNCTIONS

	public void ViewPoem()
	{
		gameState = GameState.End;
		controlsTextGO.SetActive(false);
		wordsHolder.SetActive(false);
		restartButton.SetActive(true);
		poem.text = TextUtilities.ReadTextFromFile(Application.dataPath, title + "_" + pastedPoem);
		
		Player.instance.transform.eulerAngles = Vector3.right * -90f;
		Camera.main.transform.localPosition = new Vector3(29.41f, -5.3f, -46.2f);
		poemBG.text = "Mousewheel Up/Down to scroll up/down";
	}

	public void RestartGame()
	{
		UiTextManager.instance.ClearTextsList();
//		FileUtil.DeleteFileOrDirectory(Application.dataPath + "/TEMP_POEM.txt");
		poemText = String.Empty;
		TextUtilities.ClearTextInFile("TEMP_POEM.txt");
		SceneManager.LoadScene("main");
	}

	//states
	
	private class MainState : FSM<Main>.State {
		
	}

	private class IntroState : MainState
	{
		public override void OnEnter()
		{
			base.OnEnter();
//			FileUtil.DeleteFileOrDirectory(Application.dataPath + "/TEMP_POEM.txt");
			TextUtilities.ClearTextInFile("TEMP_POEM.txt");
			AudioAndSkyManager.instance.SetupSkyboxMats();
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Context.intro.SetActive(true);
			Context.gameState = GameState.Intro;
			Context._playerModel.SetActive(false);
			UiTextManager.instance.AddAllUiTextsToList();
			UiTextManager.instance.ChangeTextColorForReadability();
		}

		public override void Update()
		{
			base.Update();
			
		}

		public override void OnExit()
		{
			base.OnExit();
			UiTextManager.instance.ClearTextsList();
			//Set Player to active and attach to CharacterJoint.
			Context._playerModel.SetActive(true);
			Context._player.GetComponent<CharacterJoint>().connectedBody =
			Context._playerModel.GetComponent<Rigidbody>();
			Context.intro.SetActive(false);
		}
	}

	private class GameplayState : MainState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Debug.Log("Entered gameplay state!");
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Context.game.SetActive(true);
			UiTextManager.instance.AddAllUiTextsToList();
			UiTextManager.instance.ChangeTextColorForReadability();
			Context._crosshair.color = UiTextManager.instance.GetNewTextColorForReadability();
		}
		

		public override void Update()
		{
			base.Update();
			if (Input.GetKeyDown(KeyCode.Return))
			{
				TransitionTo<AuthorshipState>();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
//			Context.AppendAuthorNameToPoemText();
			UiTextManager.instance.ClearTextsList();
			Context.game.SetActive(false);
		}
	}

	private class AuthorshipState : MainState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Player.instance.transform.eulerAngles = Vector3.right * -90f;
			Camera.main.transform.localPosition = new Vector3(29.41f, -5.3f, -46.2f);
			Context.controlsTextGO.SetActive(false);
			Context.wordsHolder.SetActive(false);
			Context.poem.text = poemText;
			Context.gameState = GameState.Authorship;
			Context.authorship.SetActive(true);

			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Context.poem.color = UiTextManager.instance.GetNewTextMeshProGuiColorForReadability(Context.poem);
			UiTextManager.instance.AddAllUiTextsToList();
			UiTextManager.instance.ChangeTextColorForReadability();
		}

		public override void Update()
		{
			base.Update();
			Context.ScrollPoem();
		}

		public override void OnExit()
		{
			base.OnExit();
//			Context.();
//			Context.AppendTitleAndAuthorToPoemText();
			UiTextManager.instance.ClearTextsList();
			Context.authorship.SetActive(false);
		}
	}

	private class EndState : MainState
	{
		public override void OnEnter()
		{
			base.OnEnter();
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			Context.gameState = GameState.End;
			Context.end.SetActive(true);
			Context.poem.text = poemText;
			Context.poem.color = UiTextManager.instance.GetNewTextMeshProGuiColorForReadability(Context.poem);
			Context.poemBG.color = UiTextManager.instance.GetNewTextMeshProGuiColorForReadability(Context.poemBG);
			UiTextManager.instance.AddAllUiTextsToList();
			UiTextManager.instance.ChangeTextColorForReadability();
			
		}
		
		public override void Update()
		{
			base.Update();
			Context.ScrollPoem();
		}

		public override void OnExit()
		{
			base.OnExit();
			UiTextManager.instance.ClearTextsList();
			Context.end.SetActive(false);
		}
	}
} 
