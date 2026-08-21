using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NpcBehaviour
{
	#region NPC Bids
	public static void BidWest()
	{
		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.West, 1);
		}

		// West NPC behaviour here
		// Cautious play, will play certain winners in own hand, but not take many chances
		// Will go for nullo bids more often than the other NPCs

	}

	public static void BidNorth()
	{
		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.North, 1);
		}

		// North NPC behaviour here
		// Balanced play, tries to win but isn't overambitious

	}

	public static void BidEast()
	{
		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.East, 3);
		}

		// East NPC behaviour here
		// Greedy play, will try to slam a hand if they can get away with it.
		// Likely prone to overbid

	}
	#endregion

	#region NPC Play Cards
	public static void PlayCardWest()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.West);

		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			PlayCardWestTutorial(hand);
			return;
		}

		var tricksBid = ServiceLocator.GetSingleton<Scoring>().Bids[(int)PlayerPosition.West];
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[(int)PlayerPosition.West];

		var roundManager = ServiceLocator.GetManager<RoundManager>();
		var cardsPlayed = PlayerPosition.West.CardsPlayed(roundManager.forehand);

		// West NPC behaviour here
		// Cautious play, will play certain winners in own hand, but not take many chances
		// Will go for nullo bids more often than the other NPCs

		if (cardsPlayed != 0)
		{
			var suitToFollow = roundManager.SuitToFollow();
		}
		else
		{

		}

		roundManager.GoToNextPlayer();
	}

	public static void PlayCardNorth()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.North);

		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			PlayCardNorthTutorial(hand);
			return;
		}

		var tricksBid = ServiceLocator.GetSingleton<Scoring>().Bids[(int)PlayerPosition.North];
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[(int)PlayerPosition.North];

		var roundManager = ServiceLocator.GetManager<RoundManager>();
		var cardsPlayed = PlayerPosition.North.CardsPlayed(roundManager.forehand);

		// North NPC behaviour here
		// Balanced play, tries to win but isn't overambitious

		if (cardsPlayed != 0)
		{
			var suitToFollow = roundManager.SuitToFollow();
		}
		else
		{

		}

		roundManager.GoToNextPlayer();
	}

	public static void PlayCardEast()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.East);

		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			PlayCardEastTutorial(hand);
			return;
		}

		var tricksBid = ServiceLocator.GetSingleton<Scoring>().Bids[(int)PlayerPosition.East];
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[(int)PlayerPosition.East];

		var roundManager = ServiceLocator.GetManager<RoundManager>();
		var cardsPlayed = PlayerPosition.East.CardsPlayed(roundManager.forehand);

		// East NPC behaviour here
		// Greedy play, will try to slam a hand if they can get away with it.
		// Likely prone to overbid

		if (cardsPlayed != 0)
		{
			var suitToFollow = roundManager.SuitToFollow();
		}
		else
		{

		}

		roundManager.GoToNextPlayer();
	}
	#endregion

	public static int CardsPlayed(this PlayerPosition playerId, int forehand) => forehand + 4 - (int)playerId;


	#region Tutorial Methods
	private static IEnumerator PlayCardWestTutorial(Hand west)
	{
		foreach (var (rank, suit) in GetWestCards())
		{
			var card = west.FindCard(rank, suit);
			west.PlayCard(card);
			yield break;
		}
	}

	private static IEnumerator PlayCardNorthTutorial(Hand north)
	{
		foreach (var (rank, suit) in GetNorthCards())
		{
			var card = north.FindCard(rank, suit);
			north.PlayCard(card);
			yield break;
		}
	}

	private static IEnumerator PlayCardEastTutorial(Hand east)
	{
		foreach (var (rank, suit) in GetEastCards())
		{
			var card = east.FindCard(rank, suit);
			east.PlayCard(card);
			yield break;
		}
	}

	#region Hands
	private static IEnumerable<(CardRank rank, CardSuit suit)> GetWestCards()
	{
		yield return (CardRank.Queen, CardSuit.Spades);
		yield return (CardRank.Six, CardSuit.Spades);
		yield return (CardRank.Jack, CardSuit.Hearts);
		yield return (CardRank.Eight, CardSuit.Clubs);
		yield return (CardRank.Queen, CardSuit.Hearts);
	}

	private static IEnumerable<(CardRank rank, CardSuit suit)> GetNorthCards()
	{
		yield return (CardRank.Two, CardSuit.Diamonds);
		yield return (CardRank.Five, CardSuit.Clubs);
		yield return (CardRank.Three, CardSuit.Hearts);
		yield return (CardRank.Nine, CardSuit.Hearts);
		yield return (CardRank.Seven, CardSuit.Clubs);
	}

	private static IEnumerable<(CardRank rank, CardSuit suit)> GetEastCards()
	{
		yield return (CardRank.Eight, CardSuit.Diamonds);
		yield return (CardRank.Jack, CardSuit.Clubs);
		yield return (CardRank.Three, CardSuit.Diamonds);
		yield return (CardRank.Two, CardSuit.Clubs);
		yield return (CardRank.Ten, CardSuit.Hearts);
	}
	#endregion

	#endregion
}
