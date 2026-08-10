using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoundManager : ManagerBase
{
	public int dealerId;
	public int forehand;
	public int nextPlayer;
	public int tricksPlayed = 0;

	private CardSuit? _suitLed;

	public IList<Hand> Hands { get; set; }
	public bool IsPlayerTurn => nextPlayer == (int)PlayerPosition.South;

	public void Awake()
	{
		if (ServiceLocator.GetManager<GameManager>().RoundNumber == 1)
		{
			dealerId = Random.Range(0, 3);
		}
		else
		{
			dealerId = NextPlayer(dealerId);
		}

		forehand = NextPlayer(dealerId);

		ServiceLocator.GetManager<GameManager>().UpdateDealer(dealerId);
		Hands = ServiceLocator.GetManager<GameManager>().Hands.ToList();
	}

	public void Update()
	{
		if (_suitLed == null && Hands.First(h => h.Id == forehand).transform.Find("PlayedCard").childCount != 0)
		{
			_suitLed = Hands.First(h => h.Id == forehand).transform.Find("PlayedCard").GetChild(0).gameObject.GetComponent<Card>().Suit;
		}

		if (Hands.All(h => h.transform.Find("PlayedCard").childCount > 0))
		{
			EvaluateTrick();

			foreach (var hand in Hands)
			{
				Destroy(hand.transform.Find("PlayedCard").GetChild(0).gameObject);
			}
		}

		if (tricksPlayed == ServiceLocator.GetManager<GameManager>().handSize)
		{
			ServiceLocator.GetSingleton<Scoring>().ScoreRound();
		}

		if (!IsPlayerTurn)
		{
			GetNpcToPlayCard(nextPlayer);
		}
		else
		{
			Hands.First(h => h.IsPlayer).ToggleCardButtons(IsPlayerTurn);
		}
	}

	public int NextPlayer(int playerId) => (playerId + 1) % 4;

	public void GoToNextPlayer() => NextPlayer(nextPlayer);

	public CardSuit SuitToFollow() => _suitLed.Value;

	public void EvaluateTrick()
	{
		var hands = ServiceLocator.GetManager<GameManager>().Hands;
		var cards = new Card[4];

		for (var i = 0; i < 4; i++)
		{
			var hand = hands.First(h => h.Id == (forehand + i) % 4);
			var playedCard = hand.transform.Find("PlayedCard").GetComponentInChildren<Card>();
			cards[hand.Id] = playedCard;
		}

		var winningCard = cards.GetHighestCard();
		var winningPlayer = Array.IndexOf(cards, winningCard);
		tricksPlayed++;

		ServiceLocator.GetSingleton<Scoring>().TrickWon(winningPlayer);
		forehand = winningPlayer;
		_suitLed = null;
	}

	public void GetNpcToPlayCard(int playerId)
	{
		switch (playerId)
		{
			case 1:
				NpcBehaviour.PlayCardWest();
				break;
			case 2:
				NpcBehaviour.PlayCardNorth();
				break;
			case 3:
				NpcBehaviour.PlayCardEast();
				break;
			default:
				Debug.LogWarning($"Player with ID {playerId} passed through. This does not correspond to an NPC player.");
				break;
		}
	}
}
