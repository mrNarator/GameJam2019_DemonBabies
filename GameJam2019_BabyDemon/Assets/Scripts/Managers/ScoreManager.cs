using DB.EventSystem;
using DB.EventSystem.UI;
using UnityEngine;

namespace DB
{
	public class ScoreManager
	{
		private static ScoreManager _instance;
		public static ScoreManager Get
		{
			get {
				if (_instance == null)
				{
					_instance = new ScoreManager();
				}
				return _instance;
			}
		}

		private ScoreManager()
		{
			HiScore = PlayerPrefs.GetInt(DB.Const.Prefs.HIScore, 0);
			_config = Settings.Get.GameplaySettings;
		}

		Settings.GameplayConfig _config;

		public int Score { get; private set; }
		public int LosingCount { get; private set; } = 1;

		public int HiScore { get; private set; }

		public void AddScore(int score = 1)
		{
			Score += score;
			if(Score > HiScore)
			{
				HiScore = Score;
				PlayerPrefs.SetInt(DB.Const.Prefs.HIScore, HiScore);
			}
			GlobalEvents.GetEvent<ScoreChangedEvent>().Publish(ScoreChangedEvent.Args.Make(Score, HiScore));
		}

		public void RegisterLoseDraw()
		{
			LosingCount++;
			if(LosingCount > _config.NumberOfLives)
			{
				GlobalEvents.GetEvent<GameLostEvent>().Publish(GameLostEvent.Args.Make(Score));
			}
			else
			{
				GlobalEvents.GetEvent<LifeChangedEvent>().Publish(
					LifeChangedEvent.Args.Make(_config.NumberOfLives - LosingCount + 1, false));
			}
		}

		public void ResetLosing()
		{
			LosingCount = 0;
			GlobalEvents.GetEvent<LifeChangedEvent>().Publish(
				LifeChangedEvent.Args.Make(_config.NumberOfLives - LosingCount, true));
		}

		public void Reset()
		{
			Score = 0;
			LosingCount = 0;
		}
	}
}
