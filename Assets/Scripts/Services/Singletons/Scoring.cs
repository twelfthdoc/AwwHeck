using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class Scoring : SingletonBase
{
	private int _currentRound = 0;

	public IDictionary<(PlayerPosition player, int roundNumber), int> Scores =
		new Dictionary<(PlayerPosition, int), int>
		{
			{ new(PlayerPosition.South, 0), 10 },
			{ new(PlayerPosition.West, 0),  10 },
			{ new(PlayerPosition.North, 0), 10 },
			{ new(PlayerPosition.East, 0),  10 },
		};

	public int[] Bids;
	public int[] Tricks;

	public void NewRound()
	{
		Bids = new int[4];
		Tricks = new int[4];
		_currentRound++;

		var roundManager = Object.Instantiate(ServiceLocator.GetManager<GameManager>().roundManagerPrefab);
		roundManager.name = "RoundManager";
		ServiceLocator.GetManager<RoundManager>();
	}

	public void NewBid(PlayerPosition playerId, int bid)
	{
		Bids[(int)playerId] = bid;
		ServiceLocator.GetManager<GameManager>().UpdateLabel((int)playerId);
	}

	public void TrickWon(int playerId)
	{
		Tricks[playerId]++;
		ServiceLocator.GetManager<GameManager>().UpdateLabel(playerId);
	}

	public bool ScoreRound()
	{
		foreach (PlayerPosition player in Enum.GetValues(typeof(PlayerPosition)))
		{
			var bid = Bids[(int)player];
			var tricksWon = Tricks[(int)player];

			var points = 0;

			if (bid == tricksWon)
			{
				// If equal, score points
			}
			else if (bid > tricksWon)
			{
				// If underbid, lose points
			}
			else
			{
				// If overbid, lose more points
			}

			var roundNumber = ServiceLocator.GetManager<GameManager>().RoundNumber;

			// Compare round score vs running score
			var previousScore = Scores[(player, roundNumber - 1)];

			var newScore = previousScore + points;

			// Add new scores to the Dictionary
			Scores.Add((player, roundNumber), newScore);
		}

		// Display score on screen


		// Await destruction of RoundManager
		ServiceLocator.DestroyManager<RoundManager>();

		return true;
	}
}
