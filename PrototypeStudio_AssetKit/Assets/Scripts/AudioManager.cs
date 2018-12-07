using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public Material _skyboxMat;
	private Material _newMat;
	[SerializeField]private Color _skyColor;
	
	private AudioSource ambient;
	private AudioSource music;
//	private AudioSource sfx;
	[SerializeField]private AudioClip[] pianoClips;
	[SerializeField]private AudioClip[] ambientClips;
	[SerializeField] private AudioClip[] musicClips;

	// Use this for initialization
	void Start ()
	{
		if(instance == null){
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}

		if (music == null)
		{
			music = gameObject.AddComponent<AudioSource>();
			music.clip = musicClips[Random.Range(0, musicClips.Length)];
			music.Play();	
			music.loop = true;
		}

		_newMat = new Material(_skyboxMat);
		_skyColor = Random.ColorHSV();
		_newMat.SetColor("_Tint", _skyColor);
		RenderSettings.skybox = _newMat;

//		AudioSource sfx = gameObject.AddComponent<AudioSource>();
//		sfx.playOnAwake = false;
//		sfx.loop = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			PlaySFXOnHit();
		}
//		_skyboxMat.SetColor("_Tint", _skyColor);
//		_skyboxMat.SetColor("Tint Color", _skyColor);
	}

	// Update is called once per frame


	public void PlaySFXOnHit()
	{
		AudioSource sfx = gameObject.AddComponent<AudioSource>();
		sfx.clip = pianoClips[Random.Range(0, pianoClips.Length - 1)];
		sfx.PlayScheduled(AudioSettings.dspTime + 0.00000001f);
		Destroy(sfx, sfx.clip.length);
		_skyColor = Random.ColorHSV();
		_newMat.SetColor("_Tint", _skyColor);
	}

}
