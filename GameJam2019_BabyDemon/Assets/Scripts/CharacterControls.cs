using DB.EventSystem;
using System;
using System.Collections;
using UnityEngine;

namespace DB
{
	public class CharacterControls : MonoBehaviour
	{
		Config _config;

		private void Start()
		{
			_config = Settings.Get.ControlSettings;
			GlobalEvents.GetEvent<InteractionTrigerredEvent>().Subscribe(OnInteractionStarted);
		}

		private void OnDestroy()
		{
			GlobalEvents.GetEvent<InteractionTrigerredEvent>().UnSubscribe(OnInteractionStarted);
		}

		private void OnInteractionStarted(Transform interactWith)
		{

		}

		[Serializable]
		public class Config
		{
			public float resolveDelayTemp;
		}
	}
}
