using System;
using UnityEngine;

namespace Assets.Script.Attributes
{
	[AttributeUsage(AttributeTargets.Field, Inherited = true)]
	public class ReadonlyAttribute : PropertyAttribute { }
}
