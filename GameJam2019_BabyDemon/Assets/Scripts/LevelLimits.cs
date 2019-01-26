using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DB
{
	public class LevelLimits : MonoBehaviour
	{
		private static LevelLimits _instance;
		public static LevelLimits UnsafeGet => _instance;

		[SerializeField]
		private Transform topLeft;
		[SerializeField]
		private Transform bottomRight;

		public Transform TopLeft => topLeft;
		public Transform BottomRight => bottomRight;

		private void Awake()
		{
			if (_instance == null)
			{
				_instance = this;
			}
			else
			{
				Debug.LogError($@"{this.GetType()} trying to assign self to non-null instance: {name} @ scene idx: {
					UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}");
			}
		}

		private void OnDestroy()
		{
			if(_instance == this)
			{
				_instance = null;
			}
			else
			{
				Debug.LogError($"{this.GetType()} trying to null not mine instance: {this} other: {_instance}" +
					$" @ scene idx: {UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex}");
			}
		}
	}
}
