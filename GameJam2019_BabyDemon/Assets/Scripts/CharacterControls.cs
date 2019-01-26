using DB.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			StartCoroutine(DelayInteractionResolveCo());
		}

		IEnumerator DelayInteractionResolveCo()
		{

			UnityEngine.Debug.LogFormat("<color=#44ffcc>Starting Fight</color>");
			yield return new WaitForSeconds(_config.resolveDelayTemp);

			UnityEngine.Debug.LogFormat("<color=#006622>fight \"ended\"</color>");
			GlobalEvents.GetEvent<FightFInishedEvent>().Publish();
		}

		[Serializable]
		public class Config
		{
			public float resolveDelayTemp;
		}
	}
}
