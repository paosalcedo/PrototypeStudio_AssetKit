using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

	public enum GameState
	{
		Intro,
		Game,
		End
	}

	public GameState gameState; 

	public InputField titleField;

	public InputField nameField;

	public GameObject restartButton;

	public static string title;
	public static string author;

	public TextMeshProUGUI poem;

	private GameObject wordsHolder;
	
	// Use this for initialization
	void Start ()
	{
		wordsHolder = GameObject.Find("Words");
		gameState = GameState.Intro;
		Cursor.lockState = CursorLockMode.Confined;
	}
	
	// Update is called once per frame
	void Update () {
		switch (gameState)
		{
			case GameState.Intro:
				break;
			case GameState.Game:
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				if (Input.GetKeyDown(KeyCode.Return))
				{
					ViewPoem();	
				}
				break;
			case GameState.End:
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				break;
		}
	}

	public void RecordTitleAndName()
	{
		title = titleField.text;
		author = nameField.text;
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + author, titleField.text, true);
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + author, "\nA poem by " + nameField.text, true);
		gameState = GameState.Game;
	}

	public void ViewPoem()
	{
		gameState = GameState.End;
		wordsHolder.SetActive(false);
		restartButton.SetActive(true);
		poem.text = TextUtilities.ReadTextFromFile(Application.dataPath, title + "_" + author);
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("main");
	}

} 
