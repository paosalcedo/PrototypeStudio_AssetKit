using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;

public class Player : MonoBehaviour
{
	public static Player instance;

	Rewired.Player player;
	public int playerId = 0;
	// Use this for initialization
	private Rigidbody rb;
	[SerializeField] private float speed;
	[SerializeField]private Vector3 moveVector;
	[SerializeField] private Vector2 lookVector;
	
	void Start () {
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}

		player = ReInput.players.GetPlayer(playerId);
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {
		GetInput();
		ProcessInput();
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
//		rb.velocity = moveVector * speed;
		rb.AddRelativeForce(moveVector * speed);
 //		transform.Translate(moveVector * speed * Time.deltaTime);
		rb.MoveRotation(Quaternion.Euler(lookVector));
//		Camera.main.transform.eulerAngles = lookVector;
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.GetComponent<Word>() != null)
		{
			Debug.Log("Hit text!");
			TextMeshPro wordHit = other.gameObject.GetComponent<TextMeshPro>();
			string text = wordHit.text;
			// WriteAllText creates a file, writes the specified string to the file,
			// and then closes the file.    You do NOT need to call Flush() or Close().
			System.IO.File.WriteAllText(@"C:\Users\Pao Salcedo\Desktop\WriteText.txt", text);
			
		}
	}
}
