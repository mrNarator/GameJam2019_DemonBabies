using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
	public static class Const
	{
		public static class Controls
		{
			public const string JUMP = "Jump";
			public const string HORIZONTAL = "Horizontal";
			public const string RIGHT = "right";
			public const string LEFT = "left";
			public const string SUBMIT = "Fire1";
			public const string INTERACTION = "Fire1";
		}

		public static class Prefs
		{
			public const string HIScore = "key_HiScore";
		}

		public static class Levels
		{
			public const int MainMenu = 0;
			public const int MainLevel = 1;
		}

		// please generate this auto in productino if actually needed in game
		public static class Layers
		{
			public const string InteractionSearcher = "InteractionSearcher";
			public const string InteractionReceiver = "InteractionReceiver";
		}
		public static class PlayerAnimations
		{
			public const string Attacking = "Attacking";
		}
	}
}
