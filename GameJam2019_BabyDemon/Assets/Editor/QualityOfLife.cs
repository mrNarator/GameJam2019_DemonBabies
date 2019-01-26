using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	internal class QualityOfLife : EditorWindow
	{
		private const string defaultLocation = "Assets/Resources/GameSettings.asset";

		[MenuItem("Custom/Shortcuts/GameSettings #%G")]
		private static void ContextInit()
		{
			Selection.activeObject = AssetDatabase.LoadAssetAtPath(defaultLocation, typeof(ScriptableObject));
		}

		[MenuItem("Custom/Shortcuts/Delete all userPrefs #&P")]
		private static void DeletePrefs()
		{
			PlayerPrefs.DeleteAll();
			PlayerPrefs.Save();
		}

		[MenuItem("Custom/Shortcuts/Save Project #&S")]
		private static void SaveProject()
		{
			AssetDatabase.SaveAssets();
		}

	}
}
