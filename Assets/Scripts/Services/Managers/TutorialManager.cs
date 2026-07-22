using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialManager : GameManager
{
	public CardSuit TrumpSuit = CardSuit.Diamonds;
	public CardDeck deckPrefab;
	public CardDeck deck;

	public override void Awake()
	{
		base.Awake();

		handSize = 5;
		_maxHandSize = 5;
		GameMode = "Tutorial";

		deck = Instantiate(deckPrefab, FindFirstObjectByType<Canvas>().transform);
		deck.name = deckPrefab.name;
	}

	public void Start()
	{
		var trumpCard = deck.GetSpecificCard(CardRank.Four, CardSuit.Diamonds);

		var card = Instantiate(trumpCard, deck.transform);
		card.name = trumpCard.name;
		deck.Cards.Insert(0, trumpCard);
		deck.UpdateTrumpSuit();

		StartCoroutine(SetPlayerHands());
	}

	#region Helper Methods
	private IEnumerator SetPlayerHands()
	{
		foreach ((CardRank rank, CardSuit suit) in GetPlayerCards())
		{
			var card = deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<SpriteRenderer>().enabled = true;
			Hands.First(o => o.Id == (int)PlayerPosition.South).AddCard(card);
		}

		foreach ((CardRank rank, CardSuit suit) in GetWestCards())
		{
			var card = deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponentsInChildren<SpriteRenderer>().First(o => o.name == "Background").sprite = cardBack;
			Hands.First(o => o.Id == (int)PlayerPosition.West).AddCard(card);
		}

		foreach ((CardRank rank, CardSuit suit) in GetNorthCards())
		{
			var card = deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponentsInChildren<SpriteRenderer>().First(o => o.name == "Background").sprite = cardBack;
			Hands.First(o => o.Id == (int)PlayerPosition.North).AddCard(card);
		}

		foreach ((CardRank rank, CardSuit suit) in GetEastCards())
		{
			var card = deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponentsInChildren<SpriteRenderer>().First(o => o.name == "Background").sprite = cardBack;
			Hands.First(o => o.Id == (int)PlayerPosition.East).AddCard(card);
		}

		foreach (var hand in Hands)
		{
			hand.OrderHand();
		}

		yield return null;
	}

	#region Hands
	private IEnumerable<(CardRank, CardSuit)> GetPlayerCards()
	{
		yield return new(CardRank.Six, CardSuit.Clubs);
		yield return new(CardRank.Four, CardSuit.Clubs);
		yield return new(CardRank.Six, CardSuit.Diamonds);
		yield return new(CardRank.Jack, CardSuit.Spades);
		yield return new(CardRank.Three, CardSuit.Spades);
	}

	private IEnumerable<(CardRank, CardSuit)> GetWestCards()
	{
		yield return new(CardRank.Eight, CardSuit.Clubs);
		yield return new(CardRank.Queen, CardSuit.Hearts);
		yield return new(CardRank.Jack, CardSuit.Hearts);
		yield return new(CardRank.Queen, CardSuit.Spades);
		yield return new(CardRank.Six, CardSuit.Spades);
	}

	private IEnumerable<(CardRank, CardSuit)> GetNorthCards()
	{
		yield return new(CardRank.Seven, CardSuit.Clubs);
		yield return new(CardRank.Five, CardSuit.Clubs);
		yield return new(CardRank.Two, CardSuit.Diamonds);
		yield return new(CardRank.Nine, CardSuit.Hearts);
		yield return new(CardRank.Three, CardSuit.Hearts);
	}

	private IEnumerable<(CardRank, CardSuit)> GetEastCards()
	{
		yield return new(CardRank.Jack, CardSuit.Clubs);
		yield return new(CardRank.Two, CardSuit.Clubs);
		yield return new(CardRank.Eight, CardSuit.Diamonds);
		yield return new(CardRank.Ten, CardSuit.Hearts);
		yield return new(CardRank.Two, CardSuit.Hearts);
	}
	#endregion

	#endregion
}
