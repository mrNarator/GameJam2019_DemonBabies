using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RockPapeSciz : MonoBehaviour
{
	public RockPapeScizSO SO;

    public Image selector;
	public Image backGround;
	public Image Icon;

	private void Awake()
	{
	}

	public void SetScriptableObject(RockPapeScizSO obj)
	{
		SO = obj;
		backGround.sprite = SO.cardBackGroundSprite;
		Icon.sprite = SO.icon;
	}

	public void Select()
	{
        selector.gameObject.SetActive(true);
	}

	public void Deselect()
	{
        selector.gameObject.SetActive(false);
	}
}
