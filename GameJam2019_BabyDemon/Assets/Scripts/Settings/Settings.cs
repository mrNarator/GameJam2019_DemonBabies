using DB;
using DB.Enemy;
using DB.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "CreateGameSettings")]
public class Settings : ScriptableObject
{
	public PlayerMovement.Config PlayerMovementSettings;
	public CharacterInteraction.Config InteractionSettings;
	public CharacterControls.Config ControlSettings;
	public BlankingTransitionUI.Config FadingSettings;
	public Enemy EnemySettings;
	public GameplayConfig GameplaySettings;
	public GeneralPrefabsConfig GeneralPrefabs;

	private const string defaultLocation = "GameSettings";
	private static Settings _settings;

	[System.Serializable]
	public class GameplayConfig
	{
		public int NumberOfLives;
		public float DrawDifficultyStepIncrease;
		public float DrawDifficultyMax;
		public float LifeLostFadedTime;
	}

	[System.Serializable]
	public class GeneralPrefabsConfig
	{
		public GameObject UIStatusPrefab;
		public GameObject UIBlankingPrefab;
		public GameObject UIGameOver;
	}

	[System.Serializable]
	public class Enemy
	{
		public EnemyMovement.Config Movement;
		public EnemyAI.Config AI;
	}

	public static Settings Get
	{
		get
		{
			if (_settings != null)
				return _settings;

			_settings = Resources.Load<Settings>(defaultLocation);
			return _settings;
		}
	}
}
