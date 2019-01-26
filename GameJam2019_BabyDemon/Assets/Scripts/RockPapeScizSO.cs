using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "rockPapeScizSO", menuName = "Create RockPapeScizSO")]
public class RockPapeScizSO : ScriptableObject
{
	public RockPapeScizState state;
	public Sprite icon;
	public Sprite cardBackGroundSprite;
	public GameObject Model3D;
}
