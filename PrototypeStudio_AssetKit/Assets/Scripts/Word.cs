using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Interface;
using UnityEngine;
using TMPro;

public class Word : MonoBehaviour
{
	private bool hasCollider;

	private GameObject wordsHolder;
	// Use this for initialization
	void Start ()
	{
		wordsHolder = GameObject.Find("Words");
		transform.SetParent(wordsHolder.transform);
	}
	
	// Update is called once per frame
	void Update () {
 
		//Look at player
		
		TurnOnColliderWhenPlayerIsNear();
	}

	private void TurnOnColliderWhenPlayerIsNear()
	{
		if (GetPlayerDistance() <= 30f)
		{
			if (!hasCollider)
			{
				gameObject.AddComponent<BoxCollider>();
				BoxCollider boxCollider = GetComponent<BoxCollider>();
				boxCollider.size *= 2f;
//				gameObject.GetComponent<MeshCollider>().isTrigger = true;
//				gameObject.GetComponent<MeshCollider>().convex = true;
				hasCollider = true;
			}
		}
		else if (GetPlayerDistance() > 30f)
		{
			LookAtPlayer();

			if (hasCollider)
			{
				Destroy(gameObject.GetComponent<BoxCollider>());
				hasCollider = false;
			}
		}
	}

	private void LookAtPlayer()
	{
		transform.LookAt(2 * transform.position - Player.instance.gameObject.transform.position);
	}

	protected float GetPlayerDistance()
	{
		float playerDist;
		playerDist = Vector3.Distance(Player.instance.transform.position, transform.position);
		return playerDist;
	}

}
