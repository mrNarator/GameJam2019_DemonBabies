using DB;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings.asset", menuName = "CreateGameSettings")]
public class Settings : ScriptableObject
{
	public PlayerMovement.Config PlayerMovementSettings;
	public CharacterInteraction.Config InteractionSettings;
	public CharacterControls.Config ControlSettings;

	private const string defaultLocation = "GameSettings";
	private static Settings _settings;

	[System.Serializable]
	public class GameplayConfig
	{
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
