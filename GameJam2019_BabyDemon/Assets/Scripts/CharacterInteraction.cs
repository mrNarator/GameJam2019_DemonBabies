using DB.EventSystem;
using DB.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DB
{
	public class CharacterInteraction : MonoBehaviour
	{
		private bool InRange;
		private List<Transform> interactableInRange;
		private bool Interacting { get; set; }

		private Config _config;

		private void Awake()
		{
			_config = Settings.Get.InteractionSettings;
			SetupTriggerSize();
		}

		private void Start()
		{
			interactableInRange = new List<Transform>();
			GlobalEvents.GetEvent<FightFInishedEvent>().Subscribe(OnFightFinished);
		}

		private void OnDestroy()
		{
			GlobalEvents.GetEvent<FightFInishedEvent>().UnSubscribe(OnFightFinished);
		}

		private void Update()
		{
			if(Input.GetButtonDown(DB.Const.Controls.INTERACTION) && !Interacting && InRange)
			{

				Interacting = true;
				// potentionally wrong, as this transform might not be the transform of the hero/char
				var interactable = interactableInRange.FindClosest(transform.position);
				UnityEngine.Debug.LogFormat("<color=#0066cc>Start Interaction with: {0}</color>", interactable.name);
				GlobalEvents.GetEvent<InteractionTrigerredEvent>().Publish(interactable);
			}
		}

		private void OnFightFinished()
		{

			UnityEngine.Debug.LogFormat("<color=#ff66cc>Interaction/Fight finished</color>");
			Interacting = false;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			interactableInRange.Add(collision.gameObject.transform);
			InRange = interactableInRange.Any();
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			interactableInRange.Remove(collision.gameObject.transform);
			InRange = interactableInRange.Any();
		}

		private void SetupTriggerSize()
		{
			var col = GetComponent<Collider2D>();

			if ((col as CircleCollider2D) != null)
			{
				var shapeCol = (CircleCollider2D)col;
				shapeCol.radius = _config.InteractionRange;
			}
			else if ((col as BoxCollider2D) != null)
			{
				var shapeCol = (BoxCollider2D)col;
				shapeCol.size = new Vector2(_config.InteractionRange, _config.InteractionWidth);
				shapeCol.edgeRadius = _config.InteractionEdgeRadius;
			}
			else
			{
				throw new InvalidOperationException($"Unknown collider handling type: {col.GetType()}");
			}
		}

		[Serializable]
		public class Config
		{
			public float InteractionRange;
			public float InteractionWidth;
			public float InteractionEdgeRadius;
		}
	}
}
