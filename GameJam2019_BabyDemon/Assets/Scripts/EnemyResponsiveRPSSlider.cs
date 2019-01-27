using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EnemyResponsiveRPSSlider : MonoBehaviour
{
	public Image RockPivot;
	public Image ScissorsPivot;
	public Image PaperPivot;
	public Image Background;
	public Image Marker;

	public float rockPercentage { get; private set; }
	public float scissorsPercentage { get; private set; }
	public float paperPercentage { get; private set; }

	private bool setUp = false;
	private bool sliderGoingRight = true;
	private float extraSliderIncrease = 1f;

	BaseConsumable referencedObject;

	private void Update()
	{
		if(!setUp)
		{
			return;
		}
		if(sliderGoingRight)
		{
			UpdateMarkerPosition(referencedObject.sliderSpeed);
		}
		else
		{
			UpdateMarkerPosition(-referencedObject.sliderSpeed);
		}
	}
	public void SetUpSlider(BaseConsumable baseConsumable, float extraIncrease)
	{
		Marker.rectTransform.anchoredPosition = new Vector2(UnityEngine.Random.Range(0, Background.rectTransform.rect.width),0);
		referencedObject = baseConsumable;
		ResolvePercentages();
		UpdateFillAmounts();
		setUp = true;
		extraSliderIncrease += extraIncrease;
	}

	public void UpdateMarkerPosition(float sliderSpeed)
	{
		Vector2 newPosition = new Vector2(
				Mathf.Clamp(Marker.rectTransform.anchoredPosition.x + sliderSpeed  * extraSliderIncrease * Time.deltaTime * Time.timeScale, 
				0 , 
				Background.rectTransform.rect.width)
			, 0);
		Marker.rectTransform.anchoredPosition = (Vector3)newPosition;
		if(newPosition.x >= Background.rectTransform.rect.width)
		{
			sliderGoingRight = false;
		}
		else if(newPosition.x <= 0 )
		{
			sliderGoingRight = true;
		}
	}

	public RockPapeScizState ResolveResult()
	{
		float percentalValue = Marker.rectTransform.localPosition.x / Background.rectTransform.rect.width;
		if(percentalValue < rockPercentage)
		{
			return RockPapeScizState.Rock;
		}
		else if(percentalValue >= rockPercentage && percentalValue < scissorsPercentage)
		{
			return RockPapeScizState.Scissors;
		}
		else if(percentalValue >= scissorsPercentage && percentalValue<= paperPercentage)
		{
			return RockPapeScizState.Paper;
		}
		else
		{
			return RockPapeScizState.Paper;
		}
	}

	private void UpdateFillAmounts()
	{
		RockPivot.fillAmount = rockPercentage;
		ScissorsPivot.fillAmount = rockPercentage + scissorsPercentage;
		PaperPivot.fillAmount = rockPercentage + scissorsPercentage + paperPercentage;
	}

	private void ResolvePercentages()
	{
		float sumOfPartials = referencedObject.partialProportionPaper + 
			referencedObject.partialProportionRock +
			referencedObject.partialProportionScissors;

		rockPercentage = referencedObject.partialProportionRock / sumOfPartials;
		paperPercentage = referencedObject.partialProportionPaper / sumOfPartials;
		scissorsPercentage = referencedObject.partialProportionScissors / sumOfPartials;
	}
	


}

