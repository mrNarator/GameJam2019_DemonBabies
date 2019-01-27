using DB.EventSystem;
using DB.EventSystem.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DB.UI
{
	public class ScoreLifeDisplay : MonoBehaviour
	{
		[SerializeField]
		private GridLayoutGroup lifeGrid;
		[SerializeField]
		private Image heartPrefab;
		[SerializeField]
		private Transform skullTop;
		[SerializeField]
		private TextMeshProUGUI scoreText;

		private Settings.GameplayConfig _config;
		private List<Image> activeHearts;

		private void Awake()
		{
			_config = Settings.Get.GameplaySettings;
			activeHearts = new List<Image>();

			GlobalEvents.GetEvent<ScoreChangedEvent>().Subscribe(OnScoreChanged);
			GlobalEvents.GetEvent<LifeChangedEvent>().Subscribe(OnLifeChanged);
			GlobalEvents.GetEvent<GameLostEvent>().Subscribe(OnGameLost);
		}

		private void OnDestroy()
		{
			GlobalEvents.GetEvent<ScoreChangedEvent>().UnSubscribe(OnScoreChanged);
			GlobalEvents.GetEvent<LifeChangedEvent>().UnSubscribe(OnLifeChanged);
			GlobalEvents.GetEvent<GameLostEvent>().UnSubscribe(OnGameLost);
		}

		private void Start()
		{
			int j;
			var go = heartPrefab;
			for(int i = j = 0; i < _config.NumberOfLives; i++)
			{
				if(i != j)
				{
					go = Instantiate(heartPrefab, lifeGrid.transform);
				}
				var active = go.GetComponentsInChildren<Image>().First(x => x.transform != go.transform);
				activeHearts.Add(active);
				active.gameObject.SetActive(true);
			}
			scoreText.text = "0";
		}

		private void OnScoreChanged(ScoreChangedEvent.Args args)
		{
			scoreText.text = args.Score.ToString();
		}

		private void OnLifeChanged(LifeChangedEvent.Args args)
		{
			Debug.LogFormat("new life received: {0}", args.Newlife);
			for (int i = 0; i < activeHearts.Count; i++)
			{
				activeHearts[i].gameObject.SetActive(i < args.Newlife);
			}
			if(args.Gained)
			{
				// activeHearts[args.Newlife - 1].transform.parent // animation - enbiggen
			}
			else
			{
				// activeHearts[args.Newlife].transform.parent // animation - implode
			}
		}

		private void OnGameLost(GameLostEvent.Args args)
		{
			OnLifeChanged(LifeChangedEvent.Args.Make(0, false));
		}

		[ContextMenu("sendlifeLost")]
		private void SendLifeLost()
		{
			ScoreManager.Get.RegisterLoseDraw();
		}
	}
}
