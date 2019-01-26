using DB.EventSystem;
using System;
using UnityEngine;

namespace DB.Enemy
{
	public class EnemyMovement : MonoBehaviour
	{
		Config _config;

		Action finishedCallback;
		Rigidbody2D _rigidBody;
		Vector3? target;
		Vector3 startPos;
		float startTime;

		private bool interacting;

		private void Awake()
		{
			_config = Settings.Get.EnemySettings.Movement;
			_rigidBody = GetComponent<Rigidbody2D>();
			GlobalEvents.GetEvent<InteractionTrigerredEvent>().Subscribe(OnInteractionStarted);
			GlobalEvents.GetEvent<FightFInishedEvent>().Subscribe(OnFightEnded);
		}

		private void OnDestroy()
		{
			GlobalEvents.GetEvent<InteractionTrigerredEvent>().Subscribe(OnInteractionStarted);
			GlobalEvents.GetEvent<FightFInishedEvent>().Subscribe(OnFightEnded);
		}

		private void OnInteractionStarted(Transform withWhat)
		{
			interacting = true;
		}

		private void OnFightEnded()
		{
			interacting = false;
		}

		public void SetTarget(Vector3? target, Action finishedMove)
		{
			if(this.target.HasValue)
			{
				finishedCallback();
			}
			this.finishedCallback = finishedMove;
			startTime = Time.time;
			startPos = transform.position;
			var tmp = target.Value;
			tmp.z = startPos.z;
			this.target = tmp;
		}

		private void FixedUpdate()
		{
			if(!target.HasValue || interacting)
			{
				return;
			}

			ApplyMovement();
		}

		private void ApplyMovement()
		{
			if(Mathf.Approximately(target.Value.x, transform.position.x))
			{
				finishedCallback();
				target = null;
				return;
			}
			if(Time.time - startTime > _config.TargetTimeout)
			{
				finishedCallback();
				target = null;
				return;
			}
			var movementReq = target.Value - startPos;

			var vel = _rigidBody.velocity;
			vel.x = movementReq.x;
			_rigidBody.velocity = vel;
		}

		private void OnDrawGizmos()
		{
			if(target.HasValue)
			{
				Gizmos.DrawSphere(target.Value, .1f);
			}
		}

		[Serializable]
		public class Config
		{
			public float MovementValue;
			public float JumpForce;
			public float TargetTimeout;
		}

		[Flags]
		public enum MovementTypes
		{
			None = 0,
			ConstTime = 1 << 0,
			ConstSpeed = 1 << 1,

			Jumping = 1 << 5,
		}
	}
}
