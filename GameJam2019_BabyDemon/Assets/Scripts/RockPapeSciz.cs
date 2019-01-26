using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RockPapeSciz : MonoBehaviour
{
	public RockPapeScizSO SO;

	public Image backGround;
	public Image Icon;

	private Sprite bgSprite;
	private Sprite iconSprite;


	private void Awake()
	{
		bgSprite = backGround.sprite;
		iconSprite = Icon.sprite;
	}

	public void SetScriptableObject(RockPapeScizSO obj)
	{
		SO = obj;
		bgSprite = SO.cardBackGroundSprite;
		iconSprite = SO.icon;
	}

	public void Select()
	{
		backGround.color = Color.red;
	}

	public void Deselect()
	{
		backGround.color = Color.white;
	}
}
