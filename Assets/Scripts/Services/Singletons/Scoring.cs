using System;
using System.Collections.Generic;

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

	public void NewRound()
	{
		Bids = new int[4];
		_currentRound++;
		ServiceLocator.GetManager<RoundManager>();
	}

	public void NewBid(PlayerPosition playerId, int bid)
	{
		Bids[(int)playerId] = bid;
	}

	public bool ScoreRound()
	{
		// Get # Tricks Won for each player

		// Compare Tricks Won vs Player's Bid

		// If equal, score points

		// If not equal, lose points

		// Compare round score vs running score

		// Add new scores to the Dictionary

		// Display score on screen

		// Await destruction of RoundManager
		ServiceLocator.DestroyManager<RoundManager>();

		return true;
	}

}
