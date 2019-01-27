using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DB.UI
{
	public class GameOverUI : MonoBehaviour
	{

		[SerializeField]
		private TextMeshProUGUI score;
		[SerializeField]
		private TextMeshProUGUI hiScrore;
		[SerializeField]
		private Button retryButton;
		[SerializeField]
		private Button quitButton;

		private void Start()
		{
			score.text = ScoreManager.Get.Score.ToString();
			hiScrore.text = ScoreManager.Get.HiScore.ToString();
			retryButton.onClick.AddListener(RetryLevel);
			quitButton.onClick.AddListener(QuitGameAndLife);
		}

		private void RetryLevel()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		private void QuitGameAndLife()
		{
			Application.Quit();
		}
	}
}
