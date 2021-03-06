﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Word : MonoBehaviour
{
	//from laurenz:
	//what if you make the words change in size depending on how often they occur?

	private const float MAX_DRAW_DIST = 30f;
	private const float MIN_LOOK_AT_DIST = 25;

	private FSM<Word> _fsm;
	private bool hasCollider;

	private GameObject wordsHolder;

	private Color currentColor;
	// Use this for initialization
	void Start ()
	{
		EventManager.Instance.Register<Events.PlayerWordCollisionEvent>(ChangeTextColorForReadability);
		wordsHolder = GameObject.Find("Words");
		transform.SetParent(wordsHolder.transform);
		_fsm = new FSM<Word>(this);
		_fsm.TransitionTo<LookingAtPlayer>();
		ChangeTextColorForReadability(new Events.PlayerWordCollisionEvent());
		currentColor = GetComponent<TextMeshPro>().color;
	}

	// Update is called once per frame
	void Update () {
		_fsm.Update();
		//Look at player
//		if (GetPlayerDistance() > 100f)
//		{
//			LookAtPlayer();
//		}
//
//
//		TurnOnColliderWhenPlayerIsNear();
	}

	private void ChangeTextColorForReadability(GameEvent e)
	{
		if (AudioAndSkyManager.instance.IsSkyColorCloseToBlack)
		{
			GetComponent<TextMeshPro>().color = Random.ColorHSV(0.75f, 1, 0.75f, 1, 0.75f, 1);
			currentColor = GetComponent<TextMeshPro>().color;
			StartCoroutine(RestoreTextMeshProColorBeforeHit(GetComponent<TextMeshPro>()));
		}
		else //if it's closer to white
		{
//			GetComponent<TextMeshPro>().color = Random.ColorHSV(1,1.9f,1,1.9f,1,1.9f,1,1.9f);
//			GetComponent<TextMeshPro>().color = Color.black;
			GetComponent<TextMeshPro>().color = Random.ColorHSV(0, 0.25f, 0, 0.25f, 0, 0.25f);
			currentColor = GetComponent<TextMeshPro>().color;
			StartCoroutine(RestoreTextMeshProColorBeforeHit(GetComponent<TextMeshPro>()));
		}
	}

	private bool isTweenActive;

	private float GetDistanceToPlayer()
	{
		float distance = Mathf.Abs(Vector3.Distance(Player.instance.transform.position, transform.position));
		return distance;
	}

	private void SlowLookAtTween()
	{
 //		Sequence sequence = DOTween.Sequence();
//		sequence.Append(transform.DORotate(2 * transform.position - Player.instance.gameObject.transform.position, 5f));
//		sequence.OnComplete(() => isTweenActive = false);

		transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles,
			2 * transform.position - Player.instance.gameObject.transform.position, Time.time);
	}

	private void TurnOnColliderWhenPlayerIsNear()
	{
		if (GetPlayerDistance() <= 30f)
		{
			if (!hasCollider)
			{
				GenerateCollider();
			}
		}
		else if (GetPlayerDistance() > 30f)
		{
			if (hasCollider)
			{
				Destroy(gameObject.GetComponent<BoxCollider>());
				hasCollider = false;
			}
		}
	}

	private void GenerateCollider()
	{
		gameObject.AddComponent<BoxCollider>();
		BoxCollider boxCollider = GetComponent<BoxCollider>();
		boxCollider.size *= 2f;
		boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 20f);
		hasCollider = true;
	}

	private void LookAtPlayer()
	{
		transform.LookAt(2 * transform.position - Player.instance.gameObject.transform.position);
	}

	private float GetPlayerDistance()
	{
		float playerDist;
		playerDist = Vector3.Distance(Player.instance.transform.position, transform.position);
		return playerDist;
	}
	
	private IEnumerator RestoreTextMeshProColorBeforeHit(TextMeshPro someText)
	{
		yield return new WaitForSeconds(3f);
//		Debug.Log(currentColor);
//		Debug.Log("changing color back to currentColor");
		someText.color = currentColor;
	}

	private void OnDisable()
	{
		EventManager.Instance.Unregister<Events.PlayerWordCollisionEvent>(ChangeTextColorForReadability);
	}

	private void OnDestroy()
	{
//		Debug.Log("Word disabled!");
		EventManager.Instance.Unregister<Events.PlayerWordCollisionEvent>(ChangeTextColorForReadability);
	}

	private class WordState : FSM<Word>.State
	{
		
	}

	private class LookingAtPlayer : WordState
	{
		public override void OnEnter()
		{
			base.OnEnter();	
//			Context.LookAtPlayer();
		}

		public override void Update()
		{
			base.Update();
			if (Context.GetPlayerDistance() > 50)
			{
 				Context.LookAtPlayer();
			}
			else
			{
				TransitionTo<ReadyForCollision>();	
			}
		}
	}

	private class ReadyForCollision : WordState
	{
		public override void OnEnter()
		{
			base.OnEnter();
		}

		public override void Update()
		{
			base.Update();
			Context.TurnOnColliderWhenPlayerIsNear();

			if (Context.GetPlayerDistance() > 50)
			{
				TransitionTo<LookingAtPlayer>();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
 			Context.transform.DORotateQuaternion(Quaternion.LookRotation(Context.transform.position - Player.instance.gameObject.transform.position), 1f);
 		}
	}

}
