using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
