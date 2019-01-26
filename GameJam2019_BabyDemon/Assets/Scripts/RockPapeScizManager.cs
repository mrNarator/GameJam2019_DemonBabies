using DB.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPapeScizManager : MonoBehaviour
{
	public GameObject rockPapeScizCanvasPrefab;
	public GameObject rockPapeScizCardPrefab;

	private GameObject rockPapeScizCanvas;

	public List<RockPapeScizSO> rockPapeScizSOs = new List<RockPapeScizSO>();

	public List<RockPapeSciz> cardsVisualOptions = new List<RockPapeSciz>();

	private Camera mainCamera;
	private RockPapeSciz selected;
	private int selectedIndex;

	private bool handlingCardSelection = false;
	private bool handlingEnemySelection = false;
	private bool handlingFinalResult = false;

	private RockPapeScizState playerChosenState;
	private RockPapeScizState enemyPickedState;
	
	private void Awake()
	{
		mainCamera = Camera.main;
		GlobalEvents.GetEvent<RockPapeScizEvent>().Subscribe(HandleRockPapeSciz);
		rockPapeScizCanvasPrefab.SetActive(false);
	}
	void Start()
    {
		HandleRockPapeSciz(RockPapeScizEvent.Args.Make());
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
		}
		else if(handlingEnemySelection)
		{
			HandleEnemySelection();
		}
		else if(handlingFinalResult)
		{
			HandleFinalResult();
		}
    }

	private void HandleRockPapeSciz(RockPapeScizEvent.Args args)
	{
		rockPapeScizCanvas = Instantiate(rockPapeScizCanvasPrefab);
		rockPapeScizCanvas.SetActive(true);
		rockPapeScizCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
		StartCoroutine(ShowOptionCards());
	
	}

	IEnumerator ShowOptionCards()
	{
		yield return null;
		int size = rockPapeScizSOs.Count;

		RectTransform CanvasRect = rockPapeScizCanvas.GetComponent<RectTransform>();

		float screenWidth = CanvasRect.rect.width;
		float screenHeight = CanvasRect.rect.height;
		Debug.Log(screenHeight);
		float offsetX = screenWidth /(size + 1);
		float offsetY = screenHeight / 2;


		Vector2 prevPosition = new Vector2(offsetX, offsetY) ;
		foreach(var item in rockPapeScizSOs)
		{
			Debug.Log(prevPosition);
			GameObject temp = GameObject.Instantiate(rockPapeScizCardPrefab, rockPapeScizCanvas.transform);
			StartCoroutine(SetPositionForRect(temp, new Vector2(prevPosition.x, prevPosition.y)));
			RockPapeSciz option =temp.GetComponent<RockPapeSciz>();
			option.SetScriptableObject(item);
			cardsVisualOptions.Add(option);
			prevPosition = new Vector2(prevPosition.x + offsetX, offsetY);
		}

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
	private void HandleFinalResult()
	{

	}
	private void HandleEnemySelection()
	{
		handlingEnemySelection = false;
	}
	private void HandleCardSelectionInput()
	{
		if(Input.GetButtonDown(DB.Const.Controls.RIGHT))
		{
			Debug.Log("moving right" + selectedIndex);
			MoveSelectionRight();
		}
		if(Input.GetButtonDown(DB.Const.Controls.LEFT))
		{
			Debug.Log("moving left" + selectedIndex);
			MoveSelectionLeft();
		}
		if(Input.GetButtonDown(DB.Const.Controls.SUBMIT))
		{
			SubmitSelection();
		}
	}

	private void SubmitSelection()
	{
		playerChosenState = selected.SO.state;

		selected = null;
		selectedIndex = 0;
		foreach(var item in cardsVisualOptions)
		{
			Destroy(item.gameObject);
		}
		handlingCardSelection = false;
		handlingEnemySelection = true;
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
