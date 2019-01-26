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
			PlayerMovement Player;
			IRockPapeSciz Enemy;

			//public static Args Make(PlayerMovement player, IRockPapeSciz enemy)
			//{
			//	return new Args
			//	{
			//		Player = player,
			//		Enemy = enemy,
			//	};
			//}
			public static Args Make()
			{
				return new Args
				{
					//Player = player,
					//Enemy = enemy,
				};
			}
		}
	}
}
