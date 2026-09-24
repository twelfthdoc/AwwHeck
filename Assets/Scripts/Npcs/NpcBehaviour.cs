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

		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(o => o.Id == (int)PlayerPosition.West);
		var trumpSuit = CardExtensions.TrumpSuit;

		var bids = ServiceLocator.GetSingleton<Scoring>().Bids;

		var allowedToBidZero = ServiceLocator.GetManager<GameManager>().Dealer != PlayerPosition.West ||
			bids[(int)PlayerPosition.North] != 0 ||
			bids[(int)PlayerPosition.East] != 0 ||
			bids[(int)PlayerPosition.South] != 0;

		var points = hand.Cards.Count(o => o.Suit == trumpSuit);

		if (hand.Cards.Count() > 2)
		{
			points += hand.Cards.Count(o => o.Suit != trumpSuit && o.Rank > CardRank.Jack);
		}

		if (points > 3) points--;

		if (points == 0 && !allowedToBidZero)
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.West, 1);
			yield break;
		}

		ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.West, points);
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

		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(o => o.Id == (int)PlayerPosition.North);
		var trumpSuit = CardExtensions.TrumpSuit;

		var bids = ServiceLocator.GetSingleton<Scoring>().Bids;

		var allowedToBidZero = ServiceLocator.GetManager<GameManager>().Dealer != PlayerPosition.North ||
			bids[(int)PlayerPosition.East] != 0 ||
			bids[(int)PlayerPosition.South] != 0 ||
			bids[(int)PlayerPosition.West] != 0;

		var points = hand.Cards.Count(o => o.Suit == trumpSuit);

		if (hand.Cards.Count() > 2)
		{
			points += hand.Cards.Count(o => o.Suit != trumpSuit && o.Rank > CardRank.Ten);
		}

		if (points == 0 && !allowedToBidZero)
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.North, 1);
			yield break;
		}

		ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.North, points);

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

		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(o => o.Id == (int)PlayerPosition.East);
		var trumpSuit = CardExtensions.TrumpSuit;

		var bids = ServiceLocator.GetSingleton<Scoring>().Bids;

		var allowedToBidZero = ServiceLocator.GetManager<GameManager>().Dealer != PlayerPosition.East ||
			bids[(int)PlayerPosition.South] != 0 ||
			bids[(int)PlayerPosition.West] != 0 ||
			bids[(int)PlayerPosition.North] != 0;

		var points = hand.Cards.Count(o => o.Suit == trumpSuit || o.Rank > CardRank.Ten);

		if (hand.Cards.Count() > 3) points++;

		if (points > hand.Cards.Count()) points = hand.Cards.Count();

		if (points == 0 && !allowedToBidZero)
		{
			ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.East, 1);
			yield break;
		}

		ServiceLocator.GetSingleton<Scoring>().NewBid(PlayerPosition.East, points);
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

		var card = SelectCardToPlay(hand, tricksBid, tricksWon);
		hand.PlayCardFromHand(card);

		yield return _timeDelay;
		roundManager.waitingForWest = false;
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

		var card = SelectCardToPlay(hand, tricksBid, tricksWon);
		hand.PlayCardFromHand(card);

		yield return _timeDelay;
		roundManager.waitingForNorth = false;
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

		var card = SelectCardToPlay(hand, tricksBid, tricksWon);
		hand.PlayCardFromHand(card);

		yield return _timeDelay;
		roundManager.waitingForEast = false;
	}
	#endregion

	public Card SelectCardToPlay(Hand hand, int tricksBid, int tricksWon)
	{
		var cards = hand.Cards;

		var playedCards = ServiceLocator.GetManager<GameManager>().Hands.Select(o => o.transform.Find("PlayedCard").gameObject.GetComponentsInChildren<Card>().FirstOrDefault()).ToList();

		// If there isn't exactly one card left in hand
		if (cards.Count != 1)
		{
			// If no-one has yet played a card
			if (playedCards.All(o => o == null))
			{
				if (tricksWon < tricksBid)
				{
					cards = cards.OrderByDescending(o => o.Rank).ToList();
				}
				else
				{
					cards = cards.OrderBy(o => o.Rank).ToList();
				}
			}
			// If someone has played a card
			else
			{
				var suitToFollow = ServiceLocator.GetManager<RoundManager>().SuitToFollow();
				var winningCard = playedCards.GetHighestCard();

				// If there are cards in the led suit
				if (cards.Any(o => o.Suit == suitToFollow))
				{
					// If they're trying to win
					cards = cards.Where(o => o.Suit == suitToFollow).OrderByDescending(o => o.Rank).ToList();

					// If they're trying to discard
					if (tricksWon >= tricksBid)
					{
						cards = cards.Where(o => o.Suit == suitToFollow).OrderBy(o => o.Rank).ToList();
					}
				}
				// If there are any trumps and winning card is not a trump
				else if (cards.Any(o => o.Suit == CardExtensions.TrumpSuit) && winningCard.Suit != CardExtensions.TrumpSuit)
				{
					// If they're still trying to win
					if (tricksWon < tricksBid)
					{
						cards = cards.Where(o => o.Suit == CardExtensions.TrumpSuit).OrderBy(o => o.Rank).ToList();
					}
					// If they're trying to discard
					else
					{
						cards = cards.OrderByDescending(o => o.Rank).ToList();
					}
				}
				// If there are any trumps and the winning card is a trump
				else if (cards.Any(o => o.Suit == CardExtensions.TrumpSuit) && winningCard.Suit == CardExtensions.TrumpSuit)
				{
					// If they're still trying to win
					if (cards.Any(o => o.Suit == CardExtensions.TrumpSuit && o.Rank > winningCard.Rank))
					{
						cards = cards.Where(o => o.Rank > winningCard.Rank).OrderBy(o => o.Rank).ToList();
					}
					// If they're trying to discard
					else
					{
						cards = cards.Where(o => o.Rank < winningCard.Rank).OrderByDescending(o => o.Rank).ToList();
					}
				}
				// If there are only discards
				else
				{
					// If they want to discard
					cards = cards.OrderByDescending(o => o.Rank).ToList();

					// If they're still trying to win
					if (tricksWon < tricksBid)
					{
						cards = cards.OrderBy(o => o.Rank).ToList();
					}
				}
			}
		}

		// Failsafe after the above methods
		if (cards.Count == 0)
		{
			// Really should not drop into this case!
			Debug.LogWarning($"Algorithm failed for {hand.name}!");
			cards = hand.Cards.OrderBy(o => o.Rank).ToList();
		}

		return cards.First();
	}

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
