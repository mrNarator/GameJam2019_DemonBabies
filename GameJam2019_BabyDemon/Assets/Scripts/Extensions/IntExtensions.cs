using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DB.Extensions
{
	public static class IntExtensions
	{   /// <summary>
		/// Returns string of a number in range of 0-10, else null
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string NumberToText(int number)
		{
			switch (number)
			{
				case 1:
					return "one";
				case 2:
					return "two";
				case 3:
					return "three";
				case 4:
					return "four";
				case 5:
					return "five";
				case 6:
					return "six";
				case 7:
					return "seven";
				case 8:
					return "eight";
				case 9:
					return "nine";
				case 0:
					return "zero";
				default:
					return null;
			}
		}
		/// <summary>
		/// Returns string of a number in it's ordinal form in range of 0-10, else null
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string NumberToOrdinalText(int number)
		{
			switch (number)
			{
				case 1:
					return "first";
				case 2:
					return "second";
				case 3:
					return "third";
				case 4:
					return "fourth";
				case 5:
					return "fifth";
				case 6:
					return "sixth";
				case 7:
					return "seventh";
				case 8:
					return "eighth";
				case 9:
					return "ninth";
				case 0:
					return "zeroth";
				default:
					return null;
			}
		}
		public static T GetRandomItemFromList<T>(this IList<T> list)
		{
			if(list.Count == 0)
			{
				return default(T);
			}
			Random rnd = new Random();
			return list[rnd.Next(0, list.Count)];
		}

		static int LastIdRef = 3;
		public static int GetUniqueId()
		{
			return LastIdRef++;
		}
	}
}

