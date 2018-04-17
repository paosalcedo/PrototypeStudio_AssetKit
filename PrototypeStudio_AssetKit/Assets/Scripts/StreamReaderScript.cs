using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class StreamReaderScript : MonoBehaviour
{

	private string[] fileNames = { "stories.txt"};

	private int fileNum = 0;
	
  	protected List<string> words = new List<string>();
	protected List<GameObject> textmeshGOs = new List<GameObject>();
 	
	// Use this for initialization
	public void ReadPoem ()
	{
		string fileName = fileNames[fileNum];
		string path = Application.dataPath + "/" + fileName;

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
				} else if (line[i] == ' ')
				{
 					words.Add(someWord);					
					someWord = "";	
					
				}
			}
		}
		
		sr.Close();
	}

}
