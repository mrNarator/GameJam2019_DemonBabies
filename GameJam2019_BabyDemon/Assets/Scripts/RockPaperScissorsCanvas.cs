using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RockPaperScissorsCanvas: MonoBehaviour
{
	public Image enemyImage;
	public List<RockPapeSciz> cardsVisualOptions = new List<RockPapeSciz>();

    public Image Win;
    public Image Lose;
    public Image Draw;

	public AudioClip ClickClick;
	public AudioClip WinAudio;
	public AudioClip DrawAudio;
	public AudioClip LooseAudio;

}
