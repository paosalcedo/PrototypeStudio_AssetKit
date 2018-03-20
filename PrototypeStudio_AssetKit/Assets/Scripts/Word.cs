using System.Collections;
using System.Collections.Generic;
using ProBuilder2.Interface;
using UnityEngine;
using TMPro;

public class Word : MonoBehaviour
{
	private bool hasCollider;
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () {
 
		//Look at player
		transform.LookAt(2 * transform.position - Player.instance.gameObject.transform.position);
		
		TurnOnColliderWhenPlayerIsNear();
	}

	private void TurnOnColliderWhenPlayerIsNear()
	{
		if (GetPlayerDistance() <= 30f)
		{
			if (!hasCollider)
			{
				gameObject.AddComponent<MeshCollider>();
				gameObject.GetComponent<MeshCollider>().convex = true;
				hasCollider = true;
			}
		}
		else if (GetPlayerDistance() > 30f)
		{
			if (hasCollider) Destroy(gameObject.GetComponent<MeshCollider>());			
		}
	}
	
	protected float GetPlayerDistance()
	{
		float playerDist;
		playerDist = Vector3.Distance(Player.instance.transform.position, transform.position);
		return playerDist;
	}

}
