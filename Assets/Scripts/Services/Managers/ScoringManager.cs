using System;
using System.Collections.Generic;

public class ScoringManager : ManagerBase
{
	private int _currentRound = 0;

	public IDictionary<Tuple<int, int>, int> Scores = new Dictionary<Tuple<int, int>, int>();
	public int[] Bids;

	public void NewRound()
	{
		Bids = new int[4];
		_currentRound++;
		ServiceLocator.GetManager<RoundManager>();
	}

	public void NewBid(int playerId, int bid)
	{
		Bids[playerId] = bid;
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
