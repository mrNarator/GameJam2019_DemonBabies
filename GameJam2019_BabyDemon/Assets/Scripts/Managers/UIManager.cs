using DB.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SB
{
	public class UIManager : MonoBehaviour
	{
		private static UIManager _instance;
		public static UIManager UnsafeGeet => _instance;

		private Settings.GeneralPrefabsConfig _prefabs;
		private Dictionary<string, GameObject> openedUI;

		private void Awake()
		{
			openedUI = new Dictionary<string, GameObject>();
			if(_instance == null)
			{
				_instance = this;
			}
			else
			{
				Debug.LogError("why are we creating this here?");
				return;
			}
			_prefabs = Settings.Get.GeneralPrefabs;
			GlobalEvents.GetEvent<GameLostEvent>().Subscribe(OnGameLost);
		}

		private void OnDestroy()
		{
			if(_instance == this)
			{
				_instance = null;
			}
			else
			{
				Debug.LogError("[Random pholosophical question]");
				return;
			}

			GlobalEvents.GetEvent<GameLostEvent>().UnSubscribe(OnGameLost);
		}

		private void OnGameLost(GameLostEvent.Args args)
		{
			Instantiate(_prefabs.UIGameOver);
		}
	}
}
