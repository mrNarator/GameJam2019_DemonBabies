using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DB.UI
{
	public class BlankingTransitionUI : MonoBehaviour
	{
		private static BlankingTransitionUI _instance;
		public static BlankingTransitionUI Get
		{
			get {
				if(_instance == null)
				{
					var _config = Settings.Get.GeneralPrefabs;
					_instance = Instantiate(_config.UIBlankingPrefab).GetComponent<BlankingTransitionUI>();
				}
				return _instance;
			}
		}



		[SerializeField]
		private Image Overlay;

		private Config _config;
		private bool fadingAlready;
		private Action callback;


		private void Awake()
		{
			_config = Settings.Get.FadingSettings;
		}

		public bool ShowFade(Action callback)
		{
			if(fadingAlready)
			{
				return false;
			}
			this.callback = callback;
			StartCoroutine(FadeOverTime(_config.fadedColor, _config.FadeTime));
			return true;
		}
		
		IEnumerator FadeOverTime(Color toColor, float time)
		{
			var starttime = Time.time;
			var startColor = Overlay.color;
			var tPassed = (Time.time - starttime) / time;

			UnityEngine.Debug.LogFormat("<color=#0066cc>start fading: {0} passed: {1} </color>", startColor, tPassed);
			while(tPassed < 1f)
			{
				yield return null;
				Overlay.color = Color.Lerp(startColor, toColor, tPassed);
				tPassed = (Time.time - starttime) / time;
			}
			yield return null;
			Overlay.color = toColor;
			if(callback != null)
			{
				callback();
			}
			fadingAlready = false;
		}

		public bool HideFade(Action callback)
		{
			this.callback = callback;
			if(fadingAlready)
			{
				return false;
			}
			StartCoroutine(FadeOverTime(_config.normalColor, _config.FadeTime));
			return true;
		}

		[Serializable]
		public class Config
		{
			public float FadeTime;
			public Color normalColor;
			public Color fadedColor;
		}
	}
}
