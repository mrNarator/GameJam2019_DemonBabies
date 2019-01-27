using DB.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BaseConsumable : MonoBehaviour
{
	public int demonPowerAmount;

	public float throwUpForce;
	public float angularVelocity;

	[Header("ROckPapeScizSliderOptions")]
	public float sliderSpeed;
	public float partialProportionPaper;
	public float partialProportionRock;
	public float partialProportionScissors;


	public int ZIndex;
	public AudioClip killSound;

	public State state { get; private set; }
	public Rigidbody2D _rigibody { get; private set; }

	
	protected virtual void Awake()
	{
		_rigibody = GetComponent<Rigidbody2D>();
		transform.position = new Vector3(transform.position.x, transform.position.y, ZIndex);
		state = new State();
	}

	protected virtual void Start()
	{

	}

	protected virtual void OnDestroy()
	{

	}

	public virtual void Kill()
	{
		GlobalSound.Get.PlaySingle(killSound);
		state.isDead = true;
		Consume();
	}

	public virtual void Consume()
	{
		if(!state.isDead)
		{
			return;
		}

		_rigibody.AddForce(new Vector2(0, throwUpForce), ForceMode2D.Impulse);
		_rigibody.angularVelocity = angularVelocity;

		Destroy(gameObject, 4);
	}

	[System.Serializable]
	public class State
	{
		public bool isDead = false;
	}
}
