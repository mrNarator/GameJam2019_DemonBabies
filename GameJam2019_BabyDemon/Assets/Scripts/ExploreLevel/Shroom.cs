using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Shroom : BaseConsumable
{
	public override void Awake()
	{
		base.Awake();
	}
	public override void Start()
	{
		base.Start();
		state.isDead = true;
	}
}
