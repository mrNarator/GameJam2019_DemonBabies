using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DB.Extensions
{
	public static class TransformExtensions
	{

		public static GameObject FindClosest(this IEnumerable<GameObject> searchObjects, Vector3 referencePos)
		{
			GameObject closest = null;
			Vector3 thisPos = referencePos;
			float referenceDist = float.MaxValue;

			foreach (GameObject item in searchObjects)
			{
				float dist = Vector3.SqrMagnitude(item.transform.position - thisPos);
				if (dist < referenceDist)
				{
					referenceDist = dist;
					closest = item;
				}
			}
			return closest;
		}

		public static T FindClosest<T>(this IEnumerable<T> searchObjects, Vector3 referencePos) where T : MonoBehaviour
		{
			T closest = null;
			Vector3 thisPos = referencePos;
			float referenceDist = float.MaxValue;

			foreach (var item in searchObjects)
			{
				float dist = Vector3.SqrMagnitude(item.transform.position - thisPos);
				if (dist < referenceDist)
				{
					referenceDist = dist;
					closest = item;
				}
			}
			return closest;
		}

		public static T FindClosest<T>(this IEnumerable<T> searchObjects, Vector3 referencePos, Func<T, Vector3> posResolver) where T : class
		{
			T closest = null;
			Vector3 thisPos = referencePos;
			float referenceDist = float.MaxValue;

			foreach (var item in searchObjects)
			{
				float dist = Vector3.SqrMagnitude(posResolver(item) - thisPos);
				if (dist < referenceDist)
				{
					referenceDist = dist;
					closest = item;
				}
			}
			return closest;
		}
	}
}
