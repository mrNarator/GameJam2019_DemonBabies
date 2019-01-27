using DB;
using DB.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPapeScizManager : MonoBehaviour
{
	public GameObject rockPapeScizCanvasPrefab;
	public GameObject rockPapeScizCardPrefab;

	public List<RockPapeScizSO> rockPapeScizSOs = new List<RockPapeScizSO>();

	public List<RockPapeSciz> cardsVisualOptions = new List<RockPapeSciz>();
	

	public EnemyResponsiveRPSSlider rpsSlider;
	private RockPaperScissorsCanvas rockPapeScizCanvas;


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
			HandleFinalResult();
		}
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
		//rockPapeScizCanvas.GetComponent<Canvas>().worldCamera = mainCamera;

		enemy = args.Enemy;
		player = args.Player;
		managerIsBusy = true;

		SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();
		if(enemyRenderer == null)
		{
			enemyRenderer = enemy.GetComponentInChildren<SpriteRenderer>();
		}
		rockPapeScizCanvas.enemyImage.sprite = enemyRenderer.sprite;
		rockPapeScizCanvas.enemyImage.preserveAspect = true;

		StartCoroutine(ShowOptionCards());	
	}

	IEnumerator ShowOptionCards()
	{
		yield return null;
		int size = rockPapeScizSOs.Count;

		RectTransform CanvasRect = rockPapeScizCanvas.GetComponent<RectTransform>();

		float screenWidth = CanvasRect.rect.width;
		float screenHeight = CanvasRect.rect.height;
		float offsetX = screenWidth *2/3 / (size);
		float offsetY = screenHeight / 2;


		Vector2 prevPosition = new Vector2(0, offsetY) ;
		foreach(var item in rockPapeScizSOs)
		{
			GameObject temp = GameObject.Instantiate(rockPapeScizCardPrefab, rockPapeScizCanvas.transform);
			StartCoroutine(SetPositionForRect(temp, new Vector2(prevPosition.x, prevPosition.y)));
			RockPapeSciz option = temp.GetComponent<RockPapeSciz>();
			option.SetScriptableObject(item);
			cardsVisualOptions.Add(option);
			prevPosition = new Vector2(prevPosition.x + offsetX, offsetY);
		}

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
		GlobalEvents.GetEvent<FightFinishedEvent>().Publish();
		Debug.LogFormat("resolve result: {0}", result.ToString());
		switch(result)
		{
			case RockPapeScizResult.Win:
				cumulativeDifficulty = 0f;
				ScoreManager.Get.AddScore();
				enemy.Kill();
				break;
			case RockPapeScizResult.Loose:
				cumulativeDifficulty = 0f;
				ScoreManager.Get.RegisterLoseDraw();
				break;
			case RockPapeScizResult.Draw:
				cumulativeDifficulty += _config.DrawDifficultyStepIncrease;
				cumulativeDifficulty = Mathf.Clamp(cumulativeDifficulty, 0f, _config.DrawDifficultyMax);
				StartCoroutine(ResummonFightNextFrame());
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

	private void HandleCardSelectionInput()
	{
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

		selected = null;
		selectedIndex = 0;
		foreach(var item in cardsVisualOptions)
		{
			Destroy(item.gameObject);
		}
		cardsVisualOptions.Clear();
		handlingCardSelection = false;
		handlingFinalResult = true;
	}

	private void MoveSelectionRight()
	{
		if(selectedIndex >= cardsVisualOptions.Count-1)
		{
			return;
		}
		selected.Deselect();
		selectedIndex += 1;				
		selected = cardsVisualOptions[selectedIndex];
		selected.Select();
	}
	
	private void MoveSelectionLeft()
	{
		if (selectedIndex <= 0 )
		{
			return;
		}
		selected.Deselect();
		selectedIndex -= 1;
		selected = cardsVisualOptions[selectedIndex];
		selected.Select();
	}
}
