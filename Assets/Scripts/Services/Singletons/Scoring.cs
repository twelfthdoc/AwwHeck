using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class Scoring : SingletonBase
{
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
	public int[] CurrentScores;

	public void NewRound()
	{
		Bids = new int[4];
		Tricks = new int[4];
		CurrentScores = new int[4];

		if (ServiceLocator.GetManager<GameManager>().GameMode != "Tutorial") StartRound();
	}

	public void StartRound()
	{
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

	public IEnumerator ScoreRound()
	{
		// Create Scoring Message
		var scoringMessage = ServiceLocator.GetManager<GameManager>().InstantiateScoreMessagePrefab();
		scoringMessage.name = "Scoring Message";

		foreach (PlayerPosition player in Enum.GetValues(typeof(PlayerPosition)))
		{
			var bid = Bids[(int)player];
			var tricksWon = Tricks[(int)player];
			var handSize = ServiceLocator.GetManager<GameManager>().handSize;

			var points = 0;

			if (bid == tricksWon)
			{
				// If equal, score points
				points += 10 + tricksWon;

				// If handSize is 5 or more, bonus points for a nullo or slam
				if (handSize >= 5)
				{
					if (tricksWon == 0) points += (handSize % 2) + 1;
					if (tricksWon == handSize) points += 10;
				}
			}
			else
			{
				// If not equal, lose points
				// The bigger the difference, the more points are lost (triangle number)
				var difference = Math.Abs(bid - tricksWon);
				points -= difference * (difference + 1) / 2;
			}

			var roundNumber = ServiceLocator.GetManager<GameManager>().RoundNumber;

			// Compare round score vs running score
			var previousScore = Scores[(player, roundNumber - 1)];

			var newScore = previousScore + points;

			// Add new scores to the Dictionary
			Scores.Add((player, roundNumber), newScore);
			CurrentScores[(int)player] = newScore;

			// Update scores for the player
			var messageColumn = scoringMessage.transform.Find(player.ToString()).gameObject.GetComponent<TextMeshProUGUI>();

			var message = $"<s>";
			for (var i = 0; i < roundNumber; i++)
			{
				var score = Scores[(player, i)];

				message += $"{score}\n";
			}
			message += $"</s><b>{newScore}</b>";
			messageColumn.text = message;
		}

		// Update to show which player is winning
		var topScore = CurrentScores.First(score => score == CurrentScores.Max());
		var indices = CurrentScores.Where(o => o == topScore).Select(i => Array.IndexOf(CurrentScores, topScore));

		foreach (var player in indices)
		{
			var messageColumn = scoringMessage.transform.Find(((PlayerPosition)player).ToString()).gameObject.GetComponent<TextMeshProUGUI>();
			messageColumn.color = Color.gold;
		}

		// Wait for Player to dismiss scorebox
		yield return new WaitUntil(() => !scoringMessage.activeSelf);

		// Await destruction of RoundManager
		ServiceLocator.DestroyManager<RoundManager>();

		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetManager<TutorialManager>().SendFinalMessages();
		}
	}
}
