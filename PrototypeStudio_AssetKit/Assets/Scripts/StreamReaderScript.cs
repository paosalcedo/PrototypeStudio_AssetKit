using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StreamReaderScript : MonoBehaviour
{

	public string[] fileNames;

	public int levelNum = 0;
	// Use this for initialization
	void Start ()
	{
		string fileName = fileNames[levelNum];
		string filePath = Application.dataPath + "/" + fileName;

		StreamReader sr = new StreamReader(new MemoryStream((Resources.Load(fileName) as TextAsset).bytes));

		while (!sr.EndOfStream)
		{
			string line = sr.ReadLine();

			for (int i = 0; i < line.Length; i++)
			{
					
			}
		}
		
		sr.Close();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
