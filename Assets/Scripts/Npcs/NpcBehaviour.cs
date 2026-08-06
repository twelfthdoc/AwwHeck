using System.Collections.Generic;
using System.Linq;

public static class NpcBehaviour
{
	public static void PlayCardWest()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.West);

		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			PlayCardWestTutorial(hand);
			return;
		}

		var roundManager = ServiceLocator.GetManager<RoundManager>();
		var suitToFollow = roundManager.SuitToFollow();

		// West NPC behaviour here


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

		var roundManager = ServiceLocator.GetManager<RoundManager>();
		var suitToFollow = roundManager.SuitToFollow();

		// North NPC behaviour here

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

		var roundManager = ServiceLocator.GetManager<RoundManager>();
		var suitToFollow = roundManager.SuitToFollow();

		// East NPC behaviour here

		roundManager.GoToNextPlayer();
	}

	#region Tutorial Methods
	private static async void PlayCardWestTutorial(Hand west)
	{
		await foreach (var card in GetWestCards())
		{
			west.PlayCard(card);
		}
	}

	private static async void PlayCardNorthTutorial(Hand north)
	{
		await foreach (var card in GetNorthCards())
		{
			north.PlayCard(card);
		}
	}

	private static async void PlayCardEastTutorial(Hand east)
	{
		await foreach (var card in GetEastCards())
		{
			east.PlayCard(card);
		}
	}

	#region Hands
	private static async IAsyncEnumerable<Card> GetWestCards()
	{
		yield return new(CardRank.Queen, CardSuit.Spades);
		yield return new(CardRank.Six, CardSuit.Spades);
		yield return new(CardRank.Jack, CardSuit.Hearts);
		yield return new(CardRank.Eight, CardSuit.Clubs);
		yield return new(CardRank.Queen, CardSuit.Hearts);
	}

	private static async IAsyncEnumerable<Card> GetNorthCards()
	{
		yield return new(CardRank.Two, CardSuit.Diamonds);
		yield return new(CardRank.Five, CardSuit.Clubs);
		yield return new(CardRank.Three, CardSuit.Hearts);
		yield return new(CardRank.Nine, CardSuit.Hearts);
		yield return new(CardRank.Seven, CardSuit.Clubs);
	}

	private static async IAsyncEnumerable<Card> GetEastCards()
	{
		yield return new(CardRank.Eight, CardSuit.Diamonds);
		yield return new(CardRank.Jack, CardSuit.Clubs);
		yield return new(CardRank.Two, CardSuit.Hearts);
		yield return new(CardRank.Two, CardSuit.Clubs);
		yield return new(CardRank.Ten, CardSuit.Hearts);
	}
	#endregion

	#endregion
}
