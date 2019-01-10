using System.Collections;
using System.Collections.Generic;
using System.IO;
using Rewired.Editor.Libraries.Rotorz.ReorderableList;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
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
	
	private FSM<Main> _fsm;
	public GameState gameState; 

	public InputField titleField;
	public InputField nameField;

	public GameObject restartButton;
	public GameObject controlsTextGO;
	
	public static string title;
	public static string author;
	public static string poemText;
	public int poemNum;

	public TextMeshProUGUI poem;
	public TextMeshProUGUI poemBG;

	private GameObject wordsHolder;
	private StreamReaderScript srScript;
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
		srScript = FindObjectOfType<StreamReaderScript>();
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
		author = nameField.text;
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + author, titleField.text, true);
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + author, "\nA poem by " + nameField.text, true);
		srScript.ReadPoem();
		wordEmitter.Setup();
		gameState = GameState.Game;
	}

	//INTRO FUNCTIONS
	
	public void StartGameScene()
	{
		author = nameField.text;
		srScript.ReadPoem();
		wordEmitter.Setup();
		_fsm.TransitionTo<GameplayState>();
 		gameState = GameState.Game;
	}
	
	//GAME FUNCTIONS
	public void AppendAuthorNameToPoemText()
	{
		if (author == "")
		{
			poemText = "by " + "the Unnamed Poet" + "\n" + poemText;
		}
		else
		{
			poemText = "by " + author + "\n" + poemText;
		}
	}

	public void AppendTitleToPoemText()
	{
		title = titleField.text;
		poemText = title + "\n" + poemText;
		_fsm.TransitionTo<EndState>();
	}

	//AUTHORSHIP FUNCTIONS

	public void SaveAuthor()
	{
			
	}

	public void ViewPoem()
	{
		gameState = GameState.End;
		controlsTextGO.SetActive(false);
		wordsHolder.SetActive(false);
		restartButton.SetActive(true);
		poem.text = TextUtilities.ReadTextFromFile(Application.dataPath, title + "_" + author);
		
		Player.instance.transform.eulerAngles = Vector3.right * -90f;
		Camera.main.transform.localPosition = new Vector3(29.41f, -5.3f, -46.2f);
		poemBG.text = "Mousewheel Up/Down to scroll up/down";
	}

	public void RestartGame()
	{
		UiTextManager.instance.ClearTextsList();
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
				Context.AppendAuthorNameToPoemText();
				TransitionTo<AuthorshipState>();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
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
			Context.poem.color = UiTextManager.instance.GetNewTextColorForReadability();
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
