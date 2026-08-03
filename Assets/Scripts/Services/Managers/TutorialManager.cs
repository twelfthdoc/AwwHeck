using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class TutorialManager : GameManager
{
	public const CardSuit TrumpSuit = CardSuit.Diamonds;

	public override void Awake()
	{
		base.Awake();

		GameMode = "Tutorial";
		handSize = 5;
		_maxHandSize = 5;
	}

	public void Start()
	{
		var trumpCard = Deck.GetSpecificCard(CardRank.Four, CardSuit.Diamonds);
		Deck.Cards.Insert(0, trumpCard);
		Deck.UpdateTrumpSuit();

		Dealer = PlayerPosition.South;
		Deck.UpdateDealer(Dealer);

		SetPlayerHands();
	}

	#region Helper Methods
	private void SetPlayerHands()
	{
		foreach ((CardRank rank, CardSuit suit) in GetPlayerCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			Hands.First(o => o.Id == (int)PlayerPosition.South).AddCard(card);
		}

		foreach ((CardRank rank, CardSuit suit) in GetWestCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<Image>().sprite = cardBack;
			Hands.First(o => o.Id == (int)PlayerPosition.West).AddCard(card);
		}

		foreach ((CardRank rank, CardSuit suit) in GetNorthCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<Image>().sprite = cardBack;
			Hands.First(o => o.Id == (int)PlayerPosition.North).AddCard(card);
		}

		foreach ((CardRank rank, CardSuit suit) in GetEastCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<Image>().sprite = cardBack;
			Hands.First(o => o.Id == (int)PlayerPosition.East).AddCard(card);
		}

		foreach (var hand in Hands)
		{
			hand.OrderHand();
		}
	}

	#region Hands
	private IEnumerable<(CardRank, CardSuit)> GetPlayerCards()
	{
		yield return new(CardRank.Three, CardSuit.Spades);
		yield return new(CardRank.Four, CardSuit.Clubs);
		yield return new(CardRank.Six, CardSuit.Diamonds);
		yield return new(CardRank.Jack, CardSuit.Spades);
		yield return new(CardRank.Six, CardSuit.Clubs);
	}

	private IEnumerable<(CardRank, CardSuit)> GetWestCards()
	{
		yield return new(CardRank.Queen, CardSuit.Spades);
		yield return new(CardRank.Six, CardSuit.Spades);
		yield return new(CardRank.Jack, CardSuit.Hearts);
		yield return new(CardRank.Eight, CardSuit.Clubs);
		yield return new(CardRank.Queen, CardSuit.Hearts);
	}

	private IEnumerable<(CardRank, CardSuit)> GetNorthCards()
	{
		yield return new(CardRank.Two, CardSuit.Diamonds);
		yield return new(CardRank.Five, CardSuit.Clubs);
		yield return new(CardRank.Three, CardSuit.Hearts);
		yield return new(CardRank.Nine, CardSuit.Hearts);
		yield return new(CardRank.Seven, CardSuit.Clubs);
	}

	private IEnumerable<(CardRank, CardSuit)> GetEastCards()
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
