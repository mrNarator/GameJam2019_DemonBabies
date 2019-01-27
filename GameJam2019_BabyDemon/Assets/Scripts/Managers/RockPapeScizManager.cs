using DB;
using DB.EventSystem;
using DB.EventSystem.UI;
using DB.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPapeScizManager : MonoBehaviour
{
	public GameObject rockPapeScizCanvasPrefab;
	public GameObject rockPapeScizCardPrefab;

	private RockPaperScissorsCanvas rockPapeScizCanvas;

	//public List<RockPapeScizSO> rockPapeScizSOs = new List<RockPapeScizSO>();

	public List<RockPapeSciz> cardsVisualOptions = new List<RockPapeSciz>();
	

	public EnemyResponsiveRPSSlider rpsSlider;


	//public Transform rasedLeftHand;
	//public Transform raisedRightHand;
	//public Transform raisedFinalUpLeftHand;
	//public Transform raisedFinalUpRightHand;

	private Camera mainCamera;
	private RockPapeSciz selected;
	private int selectedIndex;

	private bool handlingCardSelection = false;
	private bool handlingFinalResult = false;
	private bool managerIsBusy = false;

	private bool movingHandsUp = true;
	private bool moovingHandsDown = false;

	private RockPapeScizState playerChosenState;
	private RockPapeScizState enemyPickedState;

	private BaseConsumable enemy;
	private CharacterInteraction player;

	Settings.GameplayConfig _config;
	private float cumulativeDifficulty;
	
	private void Awake()
	{
		mainCamera = Camera.main;
		GlobalEvents.GetEvent<RockPapeScizEvent>().Subscribe(HandleRockPapeSciz);
		rockPapeScizCanvasPrefab.SetActive(false);
		_config = Settings.Get.GameplaySettings;

		GlobalEvents.GetEvent<GameLostEvent>().Subscribe(OnGameLost);
		GlobalEvents.GetEvent<LifeChangedEvent>().Subscribe(OnLifeChanged);
	}

	private void OnDestroy()
	{
		GlobalEvents.GetEvent<RockPapeScizEvent>().UnSubscribe(HandleRockPapeSciz);
	}
	// Update is called once per frame
	void Update()
    {
        if(handlingCardSelection)
		{
			HandleCardSelectionInput();
			VasVasVas();
		}
		else if(handlingFinalResult)
		{
			StartCoroutine(function());
		}
    }
	IEnumerator function ()
	{
		handlingFinalResult = false;
		var result = RockPapeScizSolver.solveOutcome(playerChosenState, enemyPickedState);


		switch (result)
		{
			case RockPapeScizResult.Win:

				rockPapeScizCanvas.Win.gameObject.SetActive(true);
				break;
			case RockPapeScizResult.Loose:

				rockPapeScizCanvas.Lose.gameObject.SetActive(true);
				break;
			case RockPapeScizResult.Draw:

				rockPapeScizCanvas.Draw.gameObject.SetActive(true);
				break;
		}

		yield return new WaitForSeconds(_config.LifeLostFadedTime);

		HandleFinalResult();
	}

	private void OnGameLost(GameLostEvent.Args args)
	{
		// show lost UI;
	}

	private void OnLifeChanged(LifeChangedEvent.Args args)
	{
		BlankingTransitionUI.Get.ShowFade(() => StartCoroutine(LifeChangedFadedTime()));
	}

	IEnumerator LifeChangedFadedTime()
	{
		Debug.LogFormat("starting 3 secod timer");
		yield return new WaitForSeconds(_config.LifeLostFadedTime);
		BlankingTransitionUI.Get.HideFade(() =>
				GlobalEvents.GetEvent<FightFinishedEvent>().Publish());
	}

	private void HandleRockPapeSciz(RockPapeScizEvent.Args args)
	{
		if(managerIsBusy)
		{
			return;
		}

		if(rockPapeScizCanvas == null)
		{
			rockPapeScizCanvas = Instantiate(rockPapeScizCanvasPrefab).GetComponent<RockPaperScissorsCanvas>();
			rpsSlider = rockPapeScizCanvas.GetComponentInChildren<EnemyResponsiveRPSSlider>();
		}

		rockPapeScizCanvas.gameObject.SetActive(true);
		rockPapeScizCanvas.GetComponent<Canvas>().worldCamera = mainCamera;


		rockPapeScizCanvas.Lose.gameObject.SetActive(false);
		rockPapeScizCanvas.Win.gameObject.SetActive(false);
		rockPapeScizCanvas.Draw.gameObject.SetActive(false);

		enemy = args.Enemy;
		player = args.Player;
		managerIsBusy = true;

		SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
		if(enemySpriteRenderer == null)
		{
			enemySpriteRenderer = enemy.GetComponentInChildren<SpriteRenderer>();
		}

		cardsVisualOptions = rockPapeScizCanvas.cardsVisualOptions;

		rockPapeScizCanvas.enemyImage.sprite = enemySpriteRenderer.sprite;

		StartCoroutine(ShowOptionCards());	
	}

	IEnumerator ShowOptionCards()
	{
		yield return null;
		//int size = rockPapeScizSOs.Count;

		//RectTransform CanvasRect = rockPapeScizCanvas.GetComponent<RectTransform>();

		//float screenWidth = CanvasRect.rect.width;
		//float screenHeight = CanvasRect.rect.height;
		//float offsetX = screenWidth /(size + 1);
		//float offsetY = screenHeight / 2;


		//Vector2 prevPosition = new Vector2(offsetX, offsetY) ;
		//foreach(var item in rockPapeScizSOs)
		//{
		//	GameObject temp = GameObject.Instantiate(rockPapeScizCardPrefab, rockPapeScizCanvas.transform);
		//	StartCoroutine(SetPositionForRect(temp, new Vector2(prevPosition.x, prevPosition.y)));
		//	RockPapeSciz option =temp.GetComponent<RockPapeSciz>();
		//	option.SetScriptableObject(item);
		//	cardsVisualOptions.Add(option);
		//	prevPosition = new Vector2(prevPosition.x + offsetX, offsetY);
		//}

		rpsSlider.gameObject.SetActive(true);
		rpsSlider.SetUpSlider(enemy, cumulativeDifficulty);

		selectedIndex = 0;
		selected = cardsVisualOptions[0];
		selected.Select();

		handlingCardSelection = true;
	}

	IEnumerator SetPositionForRect(GameObject obj, Vector2 position)
	{
		yield return null;
		obj.GetComponent<RectTransform>().localPosition += (Vector3)position;
	}

	private void VasVasVas()
	{

	}

	private void HandleFinalResult()
	{
		handlingFinalResult = false;
		var result = RockPapeScizSolver.solveOutcome(playerChosenState, enemyPickedState);
		Debug.LogFormat("resolve result: {0}", result.ToString());
		switch(result)
		{
			case RockPapeScizResult.Win:
                //rockPapeScizCanvas.Win.gameObject.SetActive(true);
                GlobalEvents.GetEvent<FightFinishedEvent>().Publish();
				cumulativeDifficulty = 0f;
				ScoreManager.Get.AddScore(GetAwardAmount());
				enemy?.Kill();
				GlobalSound.Get.PlaySingle(rockPapeScizCanvas.WinAudio);
				break;
			case RockPapeScizResult.Loose:
                //rockPapeScizCanvas.Lose.gameObject.SetActive(true);
                cumulativeDifficulty = 0f;
				enemy?.Kill();
				ScoreManager.Get.RegisterLoseDraw();
				GlobalSound.Get.PlaySingle(rockPapeScizCanvas.LooseAudio);
				break;
			case RockPapeScizResult.Draw:
                //rockPapeScizCanvas.Draw.gameObject.SetActive(true);
                cumulativeDifficulty += _config.DrawDifficultyStepIncrease;
				cumulativeDifficulty = Mathf.Clamp(cumulativeDifficulty, 0f, _config.DrawDifficultyMax);
				StartCoroutine(ResummonFightNextFrame());
				GlobalSound.Get.PlaySingle(rockPapeScizCanvas.DrawAudio);
				break;
		}
		rockPapeScizCanvas.gameObject.SetActive(false);
		managerIsBusy = false;
	}

	IEnumerator ResummonFightNextFrame()
	{
		yield return null;
		HandleRockPapeSciz(RockPapeScizEvent.Args.Make(player, enemy));
	}

	private int GetAwardAmount()
	{
		var result = enemy.demonPowerAmount;
		float selectedPercentage = 0f;
		switch (enemyPickedState)
		{
			case RockPapeScizState.Paper:
				selectedPercentage = rpsSlider.paperPercentage;
				break;
			case RockPapeScizState.Rock:
				selectedPercentage = rpsSlider.rockPercentage;
				break;
			case RockPapeScizState.Scissors:
				selectedPercentage = rpsSlider.scissorsPercentage;
				break;
		}
		// assuming there should be sign as wide as 60 percent
		result += Mathf.FloorToInt((100f - selectedPercentage) / 10f) - 4;
		return result;
	}

	private void HandleCardSelectionInput()
	{
		GlobalSound.Get.PlaySingle(rockPapeScizCanvas.ClickClick);
		if (Input.GetButtonDown(DB.Const.Controls.RIGHT))
		{
			//Debug.Log("moving right" + selectedIndex);
			MoveSelectionRight();
		}
		if (Input.GetButtonDown(DB.Const.Controls.LEFT))
		{
			//Debug.Log("moving left" + selectedIndex);
			MoveSelectionLeft();
		}
		if (Input.GetButtonDown(DB.Const.Controls.SUBMIT))
		{
			SubmitSelection();
		}
	}

	private void SubmitSelection()
	{
		playerChosenState = selected.SO.state;
		enemyPickedState = rpsSlider.ResolveResult();
		Debug.Log(playerChosenState.ToString() + enemyPickedState.ToString());

		rpsSlider.gameObject.SetActive(false);

		selected .Deselect();
		selected = null;
		selectedIndex = 0;

		handlingCardSelection = false;
		handlingFinalResult = true;
	}

	private void MoveSelectionLeft()
	{
		if(selectedIndex >= cardsVisualOptions.Count-1)
		{
			selected.Deselect();
			selectedIndex = 0;
			selected = cardsVisualOptions[selectedIndex];
			selected.Select();
		}
		else
		{
			selected.Deselect();
			selectedIndex += 1;
			selected = cardsVisualOptions[selectedIndex];
			selected.Select();
		}
	}
	
	private void MoveSelectionRight()
	{
		if (selectedIndex <= 0 )
		{
			selected.Deselect();
			selectedIndex = cardsVisualOptions.Count - 1;
			selected = cardsVisualOptions[selectedIndex];
			selected.Select();
		}
		else
		{
			selected.Deselect();
			selectedIndex -= 1;
			selected = cardsVisualOptions[selectedIndex];
			selected.Select();
		}
		
	}
}
