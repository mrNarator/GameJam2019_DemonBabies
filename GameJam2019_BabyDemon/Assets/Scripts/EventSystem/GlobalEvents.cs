using System;
using System.Collections.Generic;
using UnityEngine;

namespace DB.EventSystem
{
	public class GlobalEvents
	{
		static readonly Dictionary<Type, EventBase> eventMap = new Dictionary<Type, EventBase>();

		public static T GetEvent<T, U>() where T : EventBase<U>, new()
		{
			EventBase evt;
			var tType = typeof(T);
			if (!eventMap.TryGetValue(tType, out evt)) {
				evt = Activator.CreateInstance<T>();
				eventMap[tType] = evt;
			}

			return (T)evt;
		}

		public static T GetEvent<T>() where T : EventBase, new()
		{
			EventBase evt;
			var tType = typeof(T);
			if (!eventMap.TryGetValue(tType, out evt)) {
				evt = Activator.CreateInstance<T>();
				eventMap[tType] = evt;
			}
			return (T)evt;
		}
	}
}
