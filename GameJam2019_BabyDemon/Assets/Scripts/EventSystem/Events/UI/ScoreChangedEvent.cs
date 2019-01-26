
namespace DB.EventSystem.UI
{
	public class ScoreChangedEvent : EventBase<ScoreChangedEvent.Args>
	{
		public class Args
		{
			public int Score;
			public int HiScore;

			public static Args Make(int score, int hiScore)
			{
				return new Args
				{
					Score = score,
					HiScore = hiScore,
				};
			}
		}
	}
}
