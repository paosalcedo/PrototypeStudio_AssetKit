using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class StreamReaderScript : MonoBehaviour
{

	private string[] fileNames = { "stories.txt"};

	private int fileNum = 0;
	
//	string someWord = "";
 	protected List<string> words = new List<string>();
	protected List<GameObject> textmeshGOs = new List<GameObject>();
 	
	// Use this for initialization
	protected virtual void Start ()
	{
		string fileName = fileNames[fileNum];
//		string filePath = Application.dataPath + "/" + fileName;
		string path = "Assets/Resources/" + fileName;

		StreamReader sr = new StreamReader(path);	
 		
		while (!sr.EndOfStream)
		{
			string line = sr.ReadLine();
			string someWord = "";

			for (int i = 0; i < line.Length; i++)
			{
				if (line[i] != ' ')
				{
					someWord = someWord + line[i].ToString();
//					spaceIndices.Add(i);
				} else if (line[i] == ' ')
				{
// 					GameObject newWord = new GameObject();
//					newWord.AddComponent<TextMesh>();
//					newWord.GetComponent<TextMesh>().text = someWord;
//					textmeshGOs.Add(newWord);
 					words.Add(someWord);					
					someWord = "";	
					
				}
			}

			
		}
		
		sr.Close();
		FindObjectOfType<WordEmitter>().Setup();
	}

	protected virtual void Setup()
	{
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
//			Debug.Log(someWord);
		}
		
		
	}
}
