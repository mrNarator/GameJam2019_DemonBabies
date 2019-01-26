using DB.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DB
{
	public class CameraFollow : MonoBehaviour
	{
		private Camera cam;
		private Transform target;
		private Vector3 topLeft;
		private Vector3 bottomRight;

		private float camhWidth;
		private float camhHeight;

		[SerializeField]
		private Vector3 targetOffeset;
		[SerializeField]
		[Range(0f, 1f)]
		private float lerpSetting;

		private void Start()
		{
			cam = GetComponent<Camera>();
			var limits = LevelLimits.UnsafeGet;
			topLeft = limits.TopLeft.position;
			bottomRight = limits.BottomRight.position;


			camhWidth = cam.orthographicSize * cam.aspect;
			camhHeight = cam.orthographicSize;

			GlobalEvents.GetEvent<CameraFollowEvent>().Subscribe(OnTargetReceived);
		}

		private void OnDestroy()
		{
			GlobalEvents.GetEvent<CameraFollowEvent>().UnSubscribe(OnTargetReceived);
		}

		private void LateUpdate()
		{
			if(target == null)
			{
				return;
			}

			var targetPos = target.position + targetOffeset;
			var camPos = Vector3.Lerp(transform.position, targetPos, lerpSetting);

			camPos = EnforceLimits(camPos);

			transform.position = camPos;
		}

		private Vector3 EnforceLimits(Vector3 camPos)
		{
			var leftLimit = topLeft.x + camhWidth;
			var rightLimit = bottomRight.x - camhWidth;
			if (leftLimit > camPos.x)
			{
				camPos.x = leftLimit;
			}
			if (rightLimit < camPos.x)
			{
				camPos.x = rightLimit;
			}

			var topLimit = topLeft.y - camhHeight;
			var bottomLimit = bottomRight.y + camhHeight;
			if (topLimit < camPos.y)
			{
				camPos.y = topLimit;
			}
			if (bottomLimit > camPos.y)
			{
				camPos.y = bottomLimit;
			}

			return camPos;
		}

		private void OnTargetReceived(Transform target)
		{
			this.target = target;
		}
	}
}
