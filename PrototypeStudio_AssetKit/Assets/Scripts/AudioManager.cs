using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

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

//		AudioSource sfx = gameObject.AddComponent<AudioSource>();
//		sfx.playOnAwake = false;
//		sfx.loop = false;
	}
	
	// Update is called once per frame


	public void PlaySFXOnHit()
	{
		AudioSource sfx = gameObject.AddComponent<AudioSource>();
		sfx.clip = pianoClips[Random.Range(0, pianoClips.Length - 1)];
		sfx.PlayScheduled(AudioSettings.dspTime + 0.00000001f);
		Destroy(sfx, sfx.clip.length);
	}

}
