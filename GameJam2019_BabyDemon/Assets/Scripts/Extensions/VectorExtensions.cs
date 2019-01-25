using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using compile = System.Runtime.CompilerServices;

namespace DB.Extensions
{
	public static class VectorExtensions
	{
		// TODO: uncomment, when switched to 2018 with 4.5 support
		//[compile.MethodImpl(compile.MethodImplOptions.AggresiveInlining)]
		public static Vector3 ToV3Z(this Vector2 vec, float z = 0)
		{
			return new Vector3(vec.x, vec.y, z);
		}
	}
}
