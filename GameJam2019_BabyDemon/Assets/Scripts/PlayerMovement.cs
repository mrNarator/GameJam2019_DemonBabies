using DB.EventSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField]
	private float moveForce;
	[SerializeField]
	private float velocityLimit;
	[SerializeField]
	[Range(0f, 1f)]
	private float moveDampening;

	[SerializeField]
	private float jumpVelocity;

	private Rigidbody2D _rigidBody;
	public bool IsGrounded { get; private set; }
	private bool tryJump;
	private float jumpDirection;

	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		GlobalEvents.GetEvent<CameraFollowEvent>().Publish(transform);
	}

	private void OnDestroy()
	{
		GlobalEvents.GetEvent<CameraFollowEvent>().Publish(null);
	}

	private void Update()
	{
		// jumps
		if(Input.GetButton(DB.Const.Controls.JUMP))
		{
			tryJump = true;
		}
	}

	private void FixedUpdate()
	{
		ApplyMovement();
		if(tryJump)
		{
			TryJump();
		}
	}

	// this is totaly not physics based movement
	private void ApplyMovement()
	{
		// horiz movement
		var moveInput = Input.GetAxis(DB.Const.Controls.HORIZONTAL);

		var vel = _rigidBody.velocity;
		if(!IsGrounded)
		{
			if(Mathf.Approximately(Mathf.Epsilon, moveInput))
			{
				vel.x = vel.x * moveDampening;
			}
			else
			{
				if(Mathf.Sign(jumpDirection) != Mathf.Sign(moveInput))
				{
					vel.x = moveInput * moveForce * Time.fixedDeltaTime * moveDampening;
				}
			}
			_rigidBody.velocity = vel;
			return;
		}
		if (Mathf.Approximately(Mathf.Epsilon, moveInput))
		{
			vel.x = 0f;
			_rigidBody.velocity = vel;
			return;
		}
		moveInput = moveInput * moveForce * Time.fixedDeltaTime;
		vel.x = moveInput;


		vel.x = Mathf.Clamp(vel.x, -velocityLimit, velocityLimit);
		_rigidBody.velocity = vel;
	}

	private void TryJump()
	{
		tryJump = false;
		if(!IsGrounded)
		{
			return;
		}

		IsGrounded = false;
		var vel = _rigidBody.velocity;
		jumpDirection = vel.x;
		vel.y = jumpVelocity * Time.deltaTime;
		_rigidBody.velocity = vel;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		IsGrounded = true;
	}
}
