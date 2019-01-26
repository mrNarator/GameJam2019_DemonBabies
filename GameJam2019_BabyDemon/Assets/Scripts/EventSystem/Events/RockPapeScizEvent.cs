using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.EventSystem
{
	public class RockPapeScizEvent : EventBase<RockPapeScizEvent.Args>
	{
		public class Args
		{
			public PlayerMovement Player;
			public BaseConsumable Enemy;

			public static Args Make(PlayerMovement player, BaseConsumable baseConsumable )
			{
				return new Args
				{
					Player = player,
					Enemy = baseConsumable,
				};
			}
		}
	}
}
