﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiTextManager : MonoBehaviour
{
	public static UiTextManager instance;
	// Use this for initialization	
	[SerializeField]private List<Text> _texts = new List<Text>();
	
	void Start ()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(instance);
		}
	}

	public void AddAllUiTextsToList()
	{
		if (_texts.Count < 1)
		{
			Debug.Log("Text count less than 1. Adding UI for color change now!");
			_texts.AddRange(FindObjectsOfType<Text>());		
		}
		else
		{ 
			Debug.LogWarning("List isn't empty, clearing first!");
			_texts.Clear();
//			Debug.LogWarning("List count is now " + _texts.Count);
//			_texts.AddRange(FindObjectsOfType<Text>());	
//			Debug.LogWarning("UI texts have been added; text count is now " + _texts.Count);
		}
	}

	public void ClearTextsList()
	{
		if (_texts.Count > 0)
		{
			Debug.Log("Clearing " + _texts.Count + " elements in UI text list!");
			_texts.Clear();		
		}
		else
		{ 
			Debug.LogWarning("Nothing to clear!");
		}
	}

	public void ChangeTextColorForReadability()
	{
		Debug.Log("Changing ui text color!");
		if (AudioAndSkyManager.instance.IsSkyColorCloseToBlack)
		{
			foreach (var text in _texts)
			{
				text.color = Random.ColorHSV(0.75f, 1, 0.75f, 1, 0.75f, 1);			
			}

		}
		else //if it's closer to white
		{
			foreach (var text in _texts)
			{	
				text.color = Random.ColorHSV(0, 0.25f, 0, 0.25f, 0, 0.25f);
			}
		}
	}

	// Update is called once per frame
	public Color GetNewTextColorForReadability()
	{
//		Debug.Log("Changing ui text color!");
		if (AudioAndSkyManager.instance.IsSkyColorCloseToBlack)
		{
			foreach (var text in _texts)
			{
				text.color = Random.ColorHSV(0.75f, 1, 0.75f, 1, 0.75f, 1);
				return text.color;
			}
		}
		else //if it's closer to white
		{
			foreach (var text in _texts)
			{	
				text.color = Random.ColorHSV(0, 0.25f, 0, 0.25f, 0, 0.25f);
				return text.color;
			}
		}
		return Color.black;
	}

	public void GetNewImageColorForReadability(Image i)
	{
		if (AudioAndSkyManager.instance.IsSkyColorCloseToBlack)
		{
			i.color = Random.ColorHSV(0.75f, 1, 0.75f, 1, 0.75f, 1);
			return;
		}
		i.color = Random.ColorHSV(0, 0.25f, 0, 0.25f, 0, 0.25f);
	}
}
