using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class TutorialManager : GameManager
{
	public const CardSuit TrumpSuit = CardSuit.Diamonds;

	public override void Awake()
	{
		GameMode ??= "Tutorial";
		base.Awake();

		//Dealer = PlayerPosition.South;
		//Deck.UpdateDealer(Dealer);
	}

	public void Start()
	{
		var trumpCard = Deck.GetSpecificCard(CardRank.Four, CardSuit.Diamonds);
		Deck.Cards.Insert(0, trumpCard);
		Deck.UpdateTrumpSuit();

		SetPlayerHands();
	}

	#region Helper Methods
	private void SetPlayerHands()
	{
		foreach (var (rank, suit) in GetPlayerCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			Hands.First(h => h.IsPlayer).AddCard(card);
		}

		foreach (var (rank, suit) in GetWestCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<Image>().sprite = cardBack;
			Hands.First(h => h.Id == (int)PlayerPosition.West).AddCard(card);
		}

		foreach (var (rank, suit) in GetNorthCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<Image>().sprite = cardBack;
			Hands.First(h => h.Id == (int)PlayerPosition.North).AddCard(card);
		}

		foreach (var (rank, suit) in GetEastCards())
		{
			var card = Deck.GetSpecificCard(rank, suit);
			card.gameObject.GetComponent<Image>().sprite = cardBack;
			Hands.First(h => h.Id == (int)PlayerPosition.East).AddCard(card);
		}

		foreach (var hand in Hands)
		{
			hand.OrderHand();
		}
	}

	#region Hands
	private IEnumerable<(CardRank rank, CardSuit suit)> GetPlayerCards()
	{
		yield return (CardRank.Three, CardSuit.Spades);
		yield return (CardRank.Four, CardSuit.Clubs);
		yield return (CardRank.Six, CardSuit.Diamonds);
		yield return (CardRank.Jack, CardSuit.Spades);
		yield return (CardRank.Six, CardSuit.Clubs);
	}

	private IEnumerable<(CardRank rank, CardSuit suit)> GetWestCards()
	{
		yield return (CardRank.Queen, CardSuit.Spades);
		yield return (CardRank.Six, CardSuit.Spades);
		yield return (CardRank.Jack, CardSuit.Hearts);
		yield return (CardRank.Eight, CardSuit.Clubs);
		yield return (CardRank.Queen, CardSuit.Hearts);
	}

	private IEnumerable<(CardRank rank, CardSuit suit)> GetNorthCards()
	{
		yield return (CardRank.Two, CardSuit.Diamonds);
		yield return (CardRank.Five, CardSuit.Clubs);
		yield return (CardRank.Three, CardSuit.Hearts);
		yield return (CardRank.Nine, CardSuit.Hearts);
		yield return (CardRank.Seven, CardSuit.Clubs);
	}

	private IEnumerable<(CardRank rank, CardSuit suit)> GetEastCards()
	{
		yield return (CardRank.Eight, CardSuit.Diamonds);
		yield return (CardRank.Three, CardSuit.Diamonds);
		yield return (CardRank.Jack, CardSuit.Clubs);
		yield return (CardRank.Two, CardSuit.Clubs);
		yield return (CardRank.Ten, CardSuit.Hearts);
	}
	#endregion

	#endregion
}
