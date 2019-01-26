
namespace DB.EventSystem
{
	public class LoadLevelEvent : EventBase<LoadLevelEvent.Args>
	{
		public class Args
		{
			public string Reason;

			public static Args Make(string reason)
			{
				return new Args
				{
					Reason = reason,
				};
			}
		}
	}
}
