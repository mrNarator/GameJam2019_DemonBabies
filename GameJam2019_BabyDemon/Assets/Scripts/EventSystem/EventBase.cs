using System;

namespace DB.EventSystem
{
	public class EventBase
	{
		Action callQueue;

		public EventBase()
		{
			callQueue = () => { };
		}

		public void Publish()
		{
			callQueue.Invoke();
		}

		public void Subscribe(Action handler)
		{
			callQueue += handler;
		}

		public void UnSubscribe(Action handler)
		{
			callQueue -= handler;
		}
	}

	public class EventBase<T> : EventBase
	{
		Action<T> callQueue;

		public EventBase()
		{
			callQueue = (arg) => { };
		}

		public void Publish(T value)
		{
			callQueue.Invoke(value);
		}

		public void Subscribe(Action<T> handler)
		{
			callQueue += handler;
		}

		public void UnSubscribe(Action<T> handler)
		{
			callQueue -= handler;
		}
	}
}
