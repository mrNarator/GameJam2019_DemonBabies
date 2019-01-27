
namespace DB.EventSystem
{
	public class GameLostEvent : EventBase<GameLostEvent.Args>
	{
		public class Args
		{
			public int Score;

			public static Args Make(int score)
			{
				return new Args
				{
					Score = score,
				};
			}
		}
	}
}
