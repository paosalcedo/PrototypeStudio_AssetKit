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
	private string myPoem;
	
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
				if (Input.GetKeyDown(KeyCode.R))
				{
					SceneManager.LoadScene("main");
				}
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
		EventManager.Instance.Fire(new Events.PlayerWordCollisionEvent());
		if (other.gameObject.GetComponent<Word>() != null)
		{	
			TextMeshPro wordHit = other.gameObject.GetComponent<TextMeshPro>();
			string text = wordHit.text;
			wordHit.color = Color.yellow;
			if (wordsWritten <= 4 && wordsWritten > 0)
			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.author + "_" + Main.poemNum, wordHit.text + " ", false);
				++wordsWritten;
				myPoem = myPoem + wordHit.text +  " ";
				Main.poemText = myPoem;
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}
			else if (wordsWritten == 0)
			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.author + "_" + Main.poemNum, "\n\n" + wordHit.text + " ", false);
				myPoem = myPoem + "\n\n" + wordHit.text + " ";	
				Main.poemText = myPoem;
				++wordsWritten;
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}
			else if (wordsWritten > 4)
			{
//				TextUtilities.WriteStringToFile(Application.dataPath, Main.author + "_" + Main.poemNum, wordHit.text + " ", true);
				myPoem = myPoem + wordHit.text + " ";
				Main.poemText = myPoem;
				wordsWritten = 1;
//				StartCoroutine(ChangeTextColorOnHit(wordHit));
			} 


//			System.IO.File.WriteAllText(@"C:\Users\Pao Salcedo\Desktop\WriteText.txt", text);	
		}
	}

	private IEnumerator ChangeTextColorOnHit(TextMeshPro someText)
	{
		yield return new WaitForSeconds(3f);
//		someText.color = Color.black;
	}
}
