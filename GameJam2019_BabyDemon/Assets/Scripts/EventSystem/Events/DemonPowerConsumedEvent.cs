using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB.EventSystem
{
	public class DemonPowerConsumedEvent : EventBase<DemonPowerConsumedEvent.Args>
	{
		public class Args
		{
			public int Amount;

			public static Args Make(int amount)
			{
				return new Args
				{
					Amount = amount,
				};
			}
		}
	}
}
