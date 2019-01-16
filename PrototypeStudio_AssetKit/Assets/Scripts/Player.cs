using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	private Main main;
	public static Player instance;

	public Rewired.Player player;
	public int playerId = 0;
	// Use this for initialization
	private Rigidbody rb;
	[SerializeField] private float speed;
	[SerializeField]private Vector3 moveVector;
	[SerializeField] private Vector2 lookVector;

	private int wordsWritten = 0;
	private int lineCount = 0;
	private string myPoem;
	private const int LINE_LENGTH = 5;
	private const int LINE_COUNT = 4;
	private const float WORD_COLOR_DELAY = 3f;
	
	void Start ()
	{
		main = FindObjectOfType<Main>();
		if(instance == null){
			instance = this;
 		} else {
			Destroy(gameObject);
		}

		player = ReInput.players.GetPlayer(playerId);
		rb = GetComponent<Rigidbody>();
		
	}

	// Update is called once per frame
	void Update()
	{
		switch (main.gameState)
		{
			case Main.GameState.Intro:
				break;
			case Main.GameState.Game:
				GetInput();
				ProcessInput();
				break;
			case Main.GameState.End:
//				if (Input.GetKeyDown(KeyCode.R))
//				{
//					SceneManager.LoadScene("main");
//				}
				break;
			default:
				break;
		}

	}

	private void GetInput()
	{
		moveVector.x = player.GetAxis("MoveX");
		moveVector.y = player.GetAxis("MoveY");
		moveVector.z = player.GetAxis("MoveZ");
		lookVector.x += player.GetAxis("LookX");
		lookVector.y += player.GetAxis("LookY");
	}

	private void ProcessInput()
	{
 		rb.AddRelativeForce(moveVector * speed);
 		rb.MoveRotation(Quaternion.Euler(lookVector));

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene("main");
		}
 	}

//	private void OnTriggerEnter(Collider other)
//	{
//		if (other.gameObject.GetComponent<Word>() != null)
//		{	
//			TextMeshPro wordHit = other.gameObject.GetComponent<TextMeshPro>();
//			string text = wordHit.text;
//			wordHit.color = Color.yellow;
//			AudioManager.instance.PlaySFXOnHit();
//			if (wordsWritten <= 4 && wordsWritten > 0)
//			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.title + "_" + Main.author, wordHit.text + " ", false);
//				++wordsWritten;
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
//			}
//			else if (wordsWritten == 0)
//			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.title + "_" + Main.author, "\n\n" + wordHit.text + " ", false);
//				++wordsWritten;
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
//			}
//			else if (wordsWritten > 4)
//			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.title + "_" + Main.author, wordHit.text + " ", true);
//				wordsWritten = 1;
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
//			} 
//
//
////			System.IO.File.WriteAllText(@"C:\Users\Pao Salcedo\Desktop\WriteText.txt", text);	
//		}
//	}
	
	private void OnTriggerEnter(Collider other)
	{
		//add this back in if you want to use eventmanager for collision detection
//		EventManager.Instance.Fire(new Events.PlayerWordCollisionEvent());
		//comment this line below out to only use eventmanager for collision detection
		AudioAndSkyManager.instance.PlaySfx();
		if (other.gameObject.GetComponent<Word>() != null)
		{	
			TextMeshPro wordHit = other.gameObject.GetComponent<TextMeshPro>();
			Color color = wordHit.color;
			wordHit.color = Color.yellow;
			StartCoroutine(ChangeTextColorOnHit(wordHit, color));
			if (wordsWritten < LINE_LENGTH - 1 && wordsWritten > 0)
			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.author + "_" + Main.poemNum, wordHit.text + " ", false);
				++wordsWritten;
				myPoem += wordHit.text +  " ";
				Main.poemText = myPoem;
//				Debug.Log(myPoem);
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}
			else if (wordsWritten == 0)
			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.author + "_" + Main.poemNum, "\n\n" + wordHit.text + " ", false);
				if (lineCount >= LINE_COUNT)
				{
					lineCount = 0;
					myPoem += "\n\n" + wordHit.text + " ";	
					Main.poemText = myPoem;
					++wordsWritten;
					return;
				}
				myPoem += "\n" + wordHit.text + " ";	
				Main.poemText = myPoem;
				++wordsWritten;
//				Debug.Log(myPoem);
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}
			else if (wordsWritten >= LINE_LENGTH - 1)
			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.author + "_" + Main.poemNum, wordHit.text + " ", true);
				myPoem += wordHit.text + " ";
				Main.poemText = myPoem;
				wordsWritten = 0;
				++lineCount;
//				Debug.Log(myPoem);
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}


//			System.IO.File.WriteAllText(@"C:\Users\Pao Salcedo\Desktop\WriteText.txt", text);	
		}
	}

	private IEnumerator ChangeTextColorOnHit(TextMeshPro someText, Color colorPrev)
	{
		yield return new WaitForSeconds(WORD_COLOR_DELAY);
		someText.color = colorPrev;
	}
}
