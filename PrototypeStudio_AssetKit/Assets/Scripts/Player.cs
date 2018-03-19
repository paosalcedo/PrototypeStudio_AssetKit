using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player instance;


	// Use this for initialization
	void Start () {
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(this);
		} else {
			Destroy(gameObject);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
