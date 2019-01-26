using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : BaseConsumable
{
	//var target:Vector3;
	private Vector2 target;
	private float sec = 3;	

	public override void Awake()
	{
		base.Awake();
	}
	public override void Start()
	{
		base.Start();
		StartCoroutine(movementPatter());
	}
	
	Vector2 ResetTarget()
	{
		return new Vector2(Random.Range(transform.position.x + -2, transform.position.x + 2), Random.Range(transform.position.y + -2, transform.position.y + 2));
	}

	float ResetSec()
	{
		return Random.Range(1f, 3f);
	}

	IEnumerator movementPatter()
	{
		if(base.state.isDead)
		{
			base._rigibody.gravityScale = 0.1f;
			StopCoroutine(movementPatter());
		}
		target = ResetTarget();
		ResetSec();
		Vector2 forceVector = target - (Vector2)transform.position;
		_rigibody.AddForce(forceVector, ForceMode2D.Impulse);
		yield return new WaitForSeconds(sec);
		StartCoroutine(movementPatter());
	}	
}