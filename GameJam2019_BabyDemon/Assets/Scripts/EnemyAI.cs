using DB.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DB.Enemy
{
	public class EnemyAI : MonoBehaviour
	{

		[SerializeField]
		private EnemyMovement movementControl;
		[SerializeField]
		private List<EnemyBehaviour> behaviourCycle;


		Config _config;
		private bool moving;

		private void Awake()
		{
			_config = Settings.Get.EnemySettings.AI;
			GlobalEvents.GetEvent<LevelSetupEvent>().Subscribe(OnLevelSetup);
		}

		private void OnDestroy()
		{
			GlobalEvents.GetEvent<LevelSetupEvent>().UnSubscribe(OnLevelSetup);
		}

		private void OnLevelSetup()
		{
			StartCoroutine(IterateOverBehaviours());
		}

		private void FinishAction()
		{
			moving = false;
		}

		private IEnumerator IterateOverBehaviours()
		{
			for(int i = 0; i < behaviourCycle.Count; i++)
			{

				yield return new WaitWhile(() => moving);

				var behav = behaviourCycle[i % behaviourCycle.Count];
				var target = SelectNewTargetForBehaviourType(behav);
				if (target.HasValue)
				{
					moving = true;
				}

				movementControl.SetTarget(target, FinishAction);

				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, _config.BehaviourStepDelay));
			}
		}

		private Vector3? SelectNewTargetForBehaviourType(EnemyBehaviour behav)
		{
			switch(behav)
			{
				case EnemyBehaviour.Left:
					return ChooseTargetSideways(-1); 
				case EnemyBehaviour.Right:
					return ChooseTargetSideways(1);
				case EnemyBehaviour.LeftJump:
					return ChooseTargetSideways(-1, true);
				case EnemyBehaviour.RightJump:
					return ChooseTargetSideways(1, true);
				default:
					return null;
			}
		}

		private Vector3 ChooseTargetSideways(int direction, bool jump)
		{
			var result = ChooseTargetSideways(direction);

			if(jump)
			{
				result.y += _config.VerticalTargettingVal;
			}

			return result;
		}

		private Vector3 ChooseTargetSideways(int direction)
		{
			var pos = transform.position;
			pos.x += _config.HorizontalTargetingVal * direction;
			return pos;
		}


		[Serializable]
		public class Config
		{
			public float HorizontalTargetingVal;
			public float VerticalTargettingVal;
			public float BehaviourStepDelay;
		}

		public enum EnemyBehaviour
		{
			None = 0,
			Left = 1,
			Right = 2,
			LeftJump = 3,
			RightJump = 4,
		}

	}
}
