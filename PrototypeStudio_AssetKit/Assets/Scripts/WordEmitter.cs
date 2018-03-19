using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordEmitter : StreamReaderScript
{
	private float emissionInterval = 0f;
	private int wordIndex = 0;
	
	public void Update()
	{
		emissionInterval += Time.deltaTime;
		
		if (emissionInterval >= 0.1f)
		{
			if (wordIndex < words.Count)
			{
				GameObject newWord = new GameObject("new word");
				newWord.AddComponent<TextMesh>();
				newWord.GetComponent<TextMesh>().text = words[wordIndex];
				newWord.AddComponent<Rigidbody>();
				newWord.GetComponent<Rigidbody>().useGravity = true;			
				newWord.transform.position = Player.instance.gameObject.transform.position + new Vector3(Random.Range(-100, 100), 100, Random.Range(-100, 100));
				++wordIndex;
				emissionInterval = 0;
			}
		}	
	}
}
