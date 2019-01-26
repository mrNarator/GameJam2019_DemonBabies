using DB.EventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPapeScizSolver
{
	public static RockPapeScizResult solveOutcome(RockPapeScizState who, RockPapeScizState against)
	{
		if(who == against)
		{
			return RockPapeScizResult.Draw;
		}

		switch(who)
		{
			case RockPapeScizState.Paper:
				return PaperAgainst(against);
			case RockPapeScizState.Rock:
				return PaperAgainst(against);
			case RockPapeScizState.Scissors:
				return PaperAgainst(against);
			default:
				return RockPapeScizResult.Draw;
		}
	}

	private static RockPapeScizResult PaperAgainst(RockPapeScizState against)
	{
		switch(against)
		{
			case RockPapeScizState.Rock:
				return RockPapeScizResult.Win;
			case RockPapeScizState.Scissors:
				return RockPapeScizResult.Loose;
			default:
				return RockPapeScizResult.Draw;
		}
	}
	private static RockPapeScizResult RockAgainst(RockPapeScizState against)
	{
		switch (against)
		{
			case RockPapeScizState.Paper:
				return RockPapeScizResult.Loose;
			case RockPapeScizState.Scissors:
				return RockPapeScizResult.Win;
			default:
				return RockPapeScizResult.Draw;
		}
	}
	private static RockPapeScizResult ScissorsAgainst(RockPapeScizState against)
	{
		switch (against)
		{
			case RockPapeScizState.Paper:
				return RockPapeScizResult.Win;
			case RockPapeScizState.Rock:
				return RockPapeScizResult.Loose;
			default:
				return RockPapeScizResult.Draw;
		}
	}
}

public enum RockPapeScizState
{
	Paper = 0,
	Rock = 1,
	Scissors = 1 << 1,
}
public enum RockPapeScizResult
{
	Draw = 0,
	Win = 1,
	Loose = 1 << 1,
}
