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
		private TextMeshProUGUI simpleScoreText;
		[SerializeField]
		private TextMeshProUGUI hiScoreText;
		[SerializeField]
		private Button retryButton;
		[SerializeField]
		private Button quitButton;

		private void Start()
		{
			var scoreVal = ScoreManager.Get.Score;
			var hiScoreVal = ScoreManager.Get.HiScore;
			score.text = scoreVal.ToString();
			hiScrore.text = hiScoreVal.ToString();
			retryButton.onClick.AddListener(RetryLevel);
			quitButton.onClick.AddListener(QuitGameAndLife);


			var newHiScore = scoreVal == hiScoreVal;
			simpleScoreText.gameObject.SetActive(!newHiScore);
			hiScoreText.gameObject.SetActive(newHiScore);
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
