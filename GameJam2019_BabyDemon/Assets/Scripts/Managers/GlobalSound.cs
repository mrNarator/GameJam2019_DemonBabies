using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSound : MonoBehaviour
{
	//TODO:  connect dynamically with camera, or drag and drop
	public AudioSource efxSource;
	public AudioSource musicSource;
	public float lowPitchRange = .95f;
	public float highPitchRange = 1.05f;

	public static GlobalSound _instance;
	public GameObject globalSound { get; private set; }
	public static GlobalSound Get
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GlobalSound();
			}
			if (_instance.globalSound == null)
			{
			}

			return _instance;
		}
	}

	void init()
	{
		globalSound = new GameObject("GlobalSound");
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		else if (_instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void PlayLooping(AudioClip clip)
	{
		musicSource.clip = clip;
		musicSource.Play();
	}

	public void PlaySingle(AudioClip clip)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		efxSource.clip = clip;

		//Play the clip.
		efxSource.Play();
		
	}
	
}
