using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NpcBehaviour : SingletonBase
{
	private readonly WaitForSeconds _timeDelay = new(1.0f);

	#region NPC Bids
	public IEnumerator BidWest()
	{
		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.West, 2);
			yield break;
		}

		// West NPC behaviour here
		// Cautious play, will play certain winners in own hand, but not take many chances
		// Will go for nullo bids more often than the other NPCs

	}

	public IEnumerator BidNorth()
	{
		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.North, 1);
			yield break;
		}

		// North NPC behaviour here
		// Balanced play, tries to win but isn't overambitious

	}

	public IEnumerator BidEast()
	{
		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.East, 3);
			yield break;
		}

		// East NPC behaviour here
		// Greedy play, will try to slam a hand if they can get away with it.
		// Likely prone to overbid

	}
	#endregion

	#region NPC Play Cards
	public IEnumerator PlayCardWest()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.West);
		var roundManager = ServiceLocator.GetManager<RoundManager>();

		while (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			yield return PlayCardWestTutorial(hand);
			yield return new WaitWhile(() => roundManager.waitingForWest);
		}

		var tricksBid = ServiceLocator.GetSingleton<Scoring>().Bids[(int)PlayerPosition.West];
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[(int)PlayerPosition.West];

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

		//yield return _timeDelay;
		roundManager.GoToNextPlayer();
	}

	public IEnumerator PlayCardNorth()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.North);
		var roundManager = ServiceLocator.GetManager<RoundManager>();

		while (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			yield return PlayCardNorthTutorial(hand);
			yield return new WaitWhile(() => roundManager.waitingForNorth);
		}

		var tricksBid = ServiceLocator.GetSingleton<Scoring>().Bids[(int)PlayerPosition.North];
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[(int)PlayerPosition.North];

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

		//yield return _timeDelay;
		roundManager.GoToNextPlayer();
	}

	public IEnumerator PlayCardEast()
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(h => h.Id == (int)PlayerPosition.East);
		var roundManager = ServiceLocator.GetManager<RoundManager>();

		while (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			yield return PlayCardEastTutorial(hand);
			yield return new WaitWhile(() => roundManager.waitingForEast);
		}

		var tricksBid = ServiceLocator.GetSingleton<Scoring>().Bids[(int)PlayerPosition.East];
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[(int)PlayerPosition.East];

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

		//yield return _timeDelay;
		roundManager.GoToNextPlayer();
	}
	#endregion

	#region Tutorial Methods
	private IEnumerator PlayCardWestTutorial(Hand west)
	{
		foreach (var (rank, suit) in GetWestCards())
		{
			yield return new WaitUntil(() =>
				ServiceLocator.GetManager<RoundManager>().waitingForWest &&
				ServiceLocator.GetManager<RoundManager>().nextPlayer == (int)PlayerPosition.West);

			yield return _timeDelay;

			var card = west.FindCard(rank, suit);
			if (card != null)
			{
				if (card.Equals(CardRank.Jack, CardSuit.Hearts) ||
					card.Equals(CardRank.Six, CardSuit.Spades))
				{
					yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
					yield return _timeDelay;
				}

				west.PlayCardFromHand(card);
			}

			yield return _timeDelay;

			if (card.Equals(CardRank.Queen, CardSuit.Spades))
			{
				yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
				yield return _timeDelay;
			}

			ServiceLocator.GetManager<RoundManager>().waitingForWest = false;
		}
	}

	private IEnumerator PlayCardNorthTutorial(Hand north)
	{
		foreach (var (rank, suit) in GetNorthCards())
		{
			yield return new WaitUntil(() =>
				ServiceLocator.GetManager<RoundManager>().waitingForNorth &&
				ServiceLocator.GetManager<RoundManager>().nextPlayer == (int)PlayerPosition.North);

			var card = north.FindCard(rank, suit);
			if (card != null)
			{
				north.PlayCardFromHand(card);
			}

			yield return _timeDelay;

			if (card.Equals(CardRank.Five, CardSuit.Diamonds) ||
				card.Equals(CardRank.Five, CardSuit.Clubs))
			{
				yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
				yield return _timeDelay;
			}

			ServiceLocator.GetManager<RoundManager>().waitingForNorth = false;
		}
	}

	private IEnumerator PlayCardEastTutorial(Hand east)
	{
		foreach (var (rank, suit) in GetEastCards())
		{
			yield return new WaitUntil(() =>
				ServiceLocator.GetManager<RoundManager>().waitingForEast &&
				ServiceLocator.GetManager<RoundManager>().nextPlayer == (int)PlayerPosition.East);

			var card = east.FindCard(rank, suit);
			if (card != null)
			{
				east.PlayCardFromHand(card);
			}

			yield return _timeDelay;

			if (card.Equals(CardRank.Eight, CardSuit.Diamonds) ||
				card.Equals(CardRank.Nine, CardSuit.Hearts))
			{
				yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
				yield return _timeDelay;
			}

			ServiceLocator.GetManager<RoundManager>().waitingForEast = false;
		}
	}

	#region Hands
	private IEnumerable<(CardRank rank, CardSuit suit)> GetWestCards()
	{
		yield return (CardRank.Queen, CardSuit.Spades);
		yield return (CardRank.Six, CardSuit.Clubs);
		yield return (CardRank.Jack, CardSuit.Hearts);
		yield return (CardRank.Six, CardSuit.Spades);
		yield return (CardRank.Queen, CardSuit.Hearts);
	}

	private IEnumerable<(CardRank rank, CardSuit suit)> GetNorthCards()
	{
		yield return (CardRank.Five, CardSuit.Diamonds);
		yield return (CardRank.Five, CardSuit.Clubs);
		yield return (CardRank.Three, CardSuit.Hearts);
		yield return (CardRank.Seven, CardSuit.Clubs);
		yield return (CardRank.Three, CardSuit.Diamonds);
	}

	private IEnumerable<(CardRank rank, CardSuit suit)> GetEastCards()
	{
		yield return (CardRank.Eight, CardSuit.Diamonds);
		yield return (CardRank.Jack, CardSuit.Clubs);
		yield return (CardRank.Ten, CardSuit.Hearts);
		yield return (CardRank.Two, CardSuit.Clubs);
		yield return (CardRank.Nine, CardSuit.Hearts);
	}
	#endregion

	#endregion
}
