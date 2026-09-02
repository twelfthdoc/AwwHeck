using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : GameManager
{
	public GameObject message;

	public bool SendNextMessage { get; set; }

	private bool _inSetup = true;

	public override void Awake()
	{
		GameMode ??= "Tutorial";
		StartCoroutine(ServiceLocator.GetSingleton<TutorialMessages>().UpdateMessage());
		base.Awake();
	}

	public void Update()
	{
		if (_inSetup)
		{
			_inSetup = false;
			StartCoroutine(Setup());
		}
	}

	public IEnumerator Setup()
	{
		yield return SendMessage();
		yield return SendMessage();

		SetPlayerHands();

		yield return SendMessage();

		var trumpCard = Deck.GetSpecificCard(CardRank.Four, CardSuit.Diamonds);
		Deck.Cards.Insert(0, trumpCard);
		Deck.UpdateTrumpSuit();
		OrderHands();

		ServiceLocator.GetSingleton<Scoring>().StartRound();
	}

	public IEnumerator SendMessage()
	{
		SendNextMessage = true;
		yield return new WaitUntil(() => !SendNextMessage);
		yield return new WaitForEndOfFrame();
	}

	public void OnDestroy() => ServiceLocator.DestroySingleton<TutorialMessages>();

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

		OrderHands();
	}

	#region Hands
	private IEnumerable<(CardRank rank, CardSuit suit)> GetPlayerCards()
	{
		yield return (CardRank.Three, CardSuit.Spades);
		yield return (CardRank.Four, CardSuit.Clubs);
		yield return (CardRank.Six, CardSuit.Diamonds);
		yield return (CardRank.Eight, CardSuit.Clubs);
		yield return (CardRank.Jack, CardSuit.Spades);
	}

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
