using DB;
using DB.EventSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Config _config;

	private Rigidbody2D _rigidBody;

	private SpriteRenderer spriteRenderer;
	private bool walkingRight = true;

	public bool IsGrounded { get; private set; }
	private bool tryJump;

	private bool interacting;
	private float jumpDirection;

	private void Awake()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		if(spriteRenderer == null)
		{
			spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		}
	}

	private void Start()
	{
		_config = Settings.Get.PlayerMovementSettings;
		GlobalEvents.GetEvent<CameraFollowEvent>().Publish(transform);
		GlobalEvents.GetEvent<InteractionTrigerredEvent>().Subscribe(OnInteractionTrigerred);
		GlobalEvents.GetEvent<FightFinishedEvent>().Subscribe(OnFightFinished);
	}

	private void OnDestroy()
	{
		GlobalEvents.GetEvent<FightFinishedEvent>().Subscribe(OnFightFinished);
		GlobalEvents.GetEvent<InteractionTrigerredEvent>().Subscribe(OnInteractionTrigerred);
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
		if(interacting)
		{
			return;
		}
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
		if(vel.x>=0.01)
		{
			spriteRenderer.flipX = false;
		}
		else if( vel.x<-0.01)
		{
			spriteRenderer.flipX = true;
		}
		if(!IsGrounded)
		{
			if(Mathf.Approximately(Mathf.Epsilon, moveInput))
			{
				vel.x = vel.x * _config.moveDampening;
			}
			else
			{
				if(Mathf.Sign(jumpDirection) != Mathf.Sign(moveInput))
				{
					vel.x = moveInput * _config.moveForce * Time.fixedDeltaTime * _config.moveDampening;
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
		moveInput = moveInput * _config.moveForce * Time.fixedDeltaTime;
		vel.x = moveInput;


		vel.x = Mathf.Clamp(vel.x, -_config.velocityLimit, _config.velocityLimit);
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
		vel.y = _config.jumpVelocity * Time.deltaTime;
		_rigidBody.velocity = vel;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		IsGrounded = true;
		
		if(collision.collider.gameObject.layer == LayerMask.NameToLayer(DB.Const.Layers.InteractionReceiver))
		{

			BaseConsumable baseConsumable = collision.collider.GetComponent<BaseConsumable>();
			if(baseConsumable ==null )
			{
				return;
			}
			GlobalEvents.GetEvent<RockPapeScizEvent>().Publish(RockPapeScizEvent.Args.Make(GetComponentInChildren<CharacterInteraction>(), baseConsumable));
			GlobalEvents.GetEvent<InteractionTrigerredEvent>().Publish(collision.transform);
		}
	}

	private void OnFightFinished()
	{
		interacting = false;
		_rigidBody.simulated = true;
	}

	private void OnInteractionTrigerred(Transform withWhat)
	{
		interacting = true;
		_rigidBody.simulated = false;
	}

	[System.Serializable]
	public class Config
	{
		public float moveForce;
		public float velocityLimit;
		[Range(0f, 1f)]
		public float moveDampening;

		public float jumpVelocity;
	}
}
