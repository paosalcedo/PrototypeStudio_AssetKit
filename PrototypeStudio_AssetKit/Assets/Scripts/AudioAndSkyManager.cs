using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioAndSkyManager : MonoBehaviour
{
	private const float DARKNESS_THRESHOLD = 0.5f; //the closer to 0, the blacker the sky.  
	
	public static AudioAndSkyManager instance;

	public Material _skyboxMat;
	private Material _newMat;
	[SerializeField]private Color _skyColor;
	
	private AudioSource ambient;
	private AudioSource music;
//	private AudioSource sfx;
	[SerializeField]private AudioClip[] pianoClips;
	[SerializeField]private AudioClip[] ambientClips;
	[SerializeField] private AudioClip[] musicClips;

	public bool IsSkyColorCloseToBlack;

	// Use this for initialization
	void Start ()
	{
		EventManager.Instance.Register<Events.PlayerWordCollisionEvent>(PlaySfxAndChangeSkyboxColorOnHit);
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

		SetupSkyboxMats();

//		AudioSource sfx = gameObject.AddComponent<AudioSource>();
//		sfx.playOnAwake = false;
//		sfx.loop = false;
	}

	public void SetupSkyboxMats()
	{
		_newMat = new Material(_skyboxMat);
		_skyColor = Random.ColorHSV();
		_newMat.SetColor("_Tint", _skyColor);
		RenderSettings.skybox = _newMat;
		if (_skyColor.r <= DARKNESS_THRESHOLD && _skyColor.g <= DARKNESS_THRESHOLD && _skyColor.b <= DARKNESS_THRESHOLD)
		{
			IsSkyColorCloseToBlack = true;
			//can also be set to a certain random range above 0.5f;
		}
		else
		{
			IsSkyColorCloseToBlack = false;
		}
	}

	void Update()
	{
		//debug controls
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			EventManager.Instance.Fire(new Events.PlayerWordCollisionEvent());
//			PlaySfxAndChangeSkyboxColorOnHit();
		}
		
		if(Input.GetKeyDown(KeyCode.Alpha2))
			_newMat.SetColor("_Tint", _skyColor);

//		_skyboxMat.SetColor("_Tint", _skyColor);
//		_skyboxMat.SetColor("Tint Color", _skyColor);
	}

	// Update is called once per frame


	public void PlaySfxAndChangeSkyboxColorOnHit(GameEvent e)
	{
		AudioSource sfx = gameObject.AddComponent<AudioSource>();
		sfx.clip = pianoClips[Random.Range(0, pianoClips.Length - 1)];
		sfx.PlayScheduled(AudioSettings.dspTime + 0.00000001f);
		Destroy(sfx, sfx.clip.length);
		_skyColor = Random.ColorHSV();
		
		//check if skycolor is too close to 
		if (_skyColor.r <= DARKNESS_THRESHOLD && _skyColor.g <= DARKNESS_THRESHOLD && _skyColor.b <= DARKNESS_THRESHOLD)
		{
			IsSkyColorCloseToBlack = true;
			//can also be set to a certain random range above 0.5f;
		}
		else
		{
			IsSkyColorCloseToBlack = false;
		}
	
		_newMat.SetColor("_Tint", _skyColor);
		UiTextManager.instance.GetNewImageColorForReadability(Main.instance.Crosshair);
	}
	
	public void PlaySfx()
	{
		AudioSource sfx = gameObject.AddComponent<AudioSource>();
		sfx.clip = pianoClips[Random.Range(0, pianoClips.Length - 1)];
		sfx.PlayScheduled(AudioSettings.dspTime + 0.00000001f);
		Destroy(sfx, sfx.clip.length);
	}

	private void OnDestroy()
	{
		EventManager.Instance.Unregister<Events.PlayerWordCollisionEvent>(PlaySfxAndChangeSkyboxColorOnHit);
	}
}
