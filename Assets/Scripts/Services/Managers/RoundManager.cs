using System;
using System.Collections;
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
	private bool _waitingForBids;

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

		if (ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial")
		{
			dealerId = 0;
		}

		forehand = NextPlayer(dealerId);
		nextPlayer = forehand;

		ServiceLocator.GetManager<GameManager>().UpdateDealer(dealerId);
		Hands = ServiceLocator.GetManager<GameManager>().Hands.ToList();

		_waitingForBids = true;
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

		if (_waitingForBids)
		{
			StartCoroutine(nameof(GetBids));
		}
		else
		{
			if (!IsPlayerTurn)
			{
				StartCoroutine(GetNpcToPlayCard(nextPlayer));
			}
			else
			{
				Hands.First(h => h.IsPlayer).ToggleCardButtons(IsPlayerTurn, _suitLed);
			}
		}
	}

	public int NextPlayer(int playerId) => (playerId + 1) % 4;

	public void GetBids()
	{
		// This works for the tutorial, but will probably need some sort of refactor for full game

		var timeDelay = 0.0f;

		do
		{
			GetBid(nextPlayer, timeDelay);
			GoToNextPlayer();
			timeDelay += 2.0f;
		}
		while (nextPlayer != forehand);

		_waitingForBids = false;
	}

	public void GetBid(int playerId, float timeDelay)
	{
		switch (playerId)
		{
			case 0:
				Invoke(nameof(GetPlayerBid), timeDelay);
				break;
			case 1:
				Invoke(nameof(GetBidWest), timeDelay);
				break;
			case 2:
				Invoke(nameof(GetBidNorth), timeDelay);
				break;
			case 3:
				Invoke(nameof(GetBidEast), timeDelay);
				break;
			default:
				Debug.LogError("");
				break;
		}
	}

	public void GetPlayerBid() => StartCoroutine(ServiceLocator.GetManager<GameManager>().GetPlayerBid());
	public void GetBidWest() => NpcBehaviour.BidWest();
	public void GetBidNorth() => NpcBehaviour.BidNorth();
	public void GetBidEast() => NpcBehaviour.BidEast();

	public void GoToNextPlayer() => nextPlayer = NextPlayer(nextPlayer);

	public CardSuit? SuitToFollow() => _suitLed;

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

	public IEnumerator GetNpcToPlayCard(int playerId)
	{
		switch (playerId)
		{
			case 1:
				NpcBehaviour.PlayCardWest();
				yield break;
			case 2:
				NpcBehaviour.PlayCardNorth();
				yield break;
			case 3:
				NpcBehaviour.PlayCardEast();
				yield break;
			default:
				Debug.LogWarning($"Player with ID {playerId} passed through. This does not correspond to an NPC player.");
				yield break;
		}
	}
}
