using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace sB.UI
{
	public class MainMenuExtraFuntions : MonoBehaviour
	{
		[SerializeField]
		private Button startGameButton;
		[SerializeField]
		private Button quitGameButton;

		private void Start()
		{
			startGameButton.onClick.AddListener(LoadMainLevel);
			quitGameButton.onClick.AddListener(QuitLife);
		}


		private void LoadMainLevel()
		{
			SceneManager.LoadScene(DB.Const.Levels.MainLevel);
		}

		private void QuitLife()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			return;
#endif
#pragma warning disable 162
			Application.Quit();
#pragma warning restore 162
		}
	}
}
