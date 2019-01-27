
namespace DB.EventSystem.UI
{
	public class LifeChangedEvent : EventBase<LifeChangedEvent.Args>
	{
		public class Args
		{
			public int Newlife;
			public bool Gained;

			public static Args Make(int newLife, bool gained)
			{
				return new Args
				{
					Newlife = newLife,
					Gained = gained,
				};
			}
		}
	}
}
