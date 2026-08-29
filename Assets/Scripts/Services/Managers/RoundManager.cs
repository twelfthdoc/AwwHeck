using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoundManager : ManagerBase
{
	public bool waitingForPlayer = false;
	public bool waitingForWest = false;
	public bool waitingForNorth = false;
	public bool waitingForEast = false;
	public int dealerId;
	public int forehand;
	public int nextPlayer;
	public int tricksPlayed = 0;

	private CardSuit? _suitLed;
	private bool _waitingForBids = true;
	private bool _biddingInProgress = false;
	private readonly WaitForSeconds _timeDelay = new(1.0f);

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

		StartCoroutine(ServiceLocator.GetSingleton<NpcBehaviour>().PlayCardWest());
		StartCoroutine(ServiceLocator.GetSingleton<NpcBehaviour>().PlayCardNorth());
		StartCoroutine(ServiceLocator.GetSingleton<NpcBehaviour>().PlayCardEast());
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
			if (!_biddingInProgress)
			{
				_biddingInProgress = true;
				StartCoroutine(GetBids());
			}
		}
		else if (!_biddingInProgress && !waitingForPlayer && !waitingForWest && !waitingForNorth && !waitingForEast)
		{
			StartCoroutine(PlayCards());
		}
	}

	public int NextPlayer(int playerId) => (playerId + 1) % 4;
	public void GoToNextPlayer() => nextPlayer = NextPlayer(nextPlayer);

	public IEnumerator GetBids()
	{
		var isTutorial = ServiceLocator.GetManager<GameManager>().GameMode == "Tutorial";

		if (isTutorial)
		{
			yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
		}

		while (_waitingForBids)
		{
			yield return GetBid();
			if (nextPlayer == dealerId) _waitingForBids = false;
			GoToNextPlayer();
		}

		if (isTutorial)
		{
			yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
		}

		_biddingInProgress = false;
	}

	public IEnumerator GetBid()
	{
		yield return _timeDelay;

		switch (nextPlayer)
		{
			case (int)PlayerPosition.South:
				yield return ServiceLocator.GetManager<GameManager>().GetPlayerBid();
				break;
			case (int)PlayerPosition.West:
				yield return ServiceLocator.GetSingleton<NpcBehaviour>().BidWest();
				break;
			case (int)PlayerPosition.North:
				yield return ServiceLocator.GetSingleton<NpcBehaviour>().BidNorth();
				break;
			case (int)PlayerPosition.East:
				yield return ServiceLocator.GetSingleton<NpcBehaviour>().BidEast();
				break;
			default:
				Debug.LogError("Argument outside of range.");
				break;
		}

		yield return new WaitForEndOfFrame();
	}

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
		nextPlayer = forehand;
		_suitLed = null;

		switch (forehand)
		{
			case (int)PlayerPosition.South:
				waitingForPlayer = true;
				break;
			case (int)PlayerPosition.West:
				waitingForWest = true;
				break;
			case (int)PlayerPosition.North:
				waitingForNorth = true;
				break;
			case (int)PlayerPosition.East:
				waitingForEast = true;
				break;
		}
	}

	public IEnumerator PlayCards()
	{
		switch (nextPlayer)
		{
			case (int)PlayerPosition.South:
				if (!waitingForPlayer)
				{
					waitingForPlayer = true;
					yield return Hands.First(o => o.IsPlayer).ToggleCardButtons(waitingForPlayer, _suitLed);
					yield return new WaitUntil(() => !waitingForPlayer);
				}
				yield break;

			case (int)PlayerPosition.West:
				if (!waitingForWest)
				{
					waitingForWest = true;
					yield return new WaitUntil(() => !waitingForWest);
					yield return _timeDelay;
				}
				yield break;

			case (int)PlayerPosition.North:
				if (!waitingForNorth)
				{
					waitingForNorth = true;
					yield return _timeDelay;
					yield return new WaitUntil(() => !waitingForNorth);
				}
				yield break;

			case (int)PlayerPosition.East:
				if (!waitingForEast)
				{
					waitingForEast = true;
					yield return _timeDelay;
					yield return new WaitUntil(() => !waitingForEast);
				}
				yield break;
		}

		yield return new WaitForEndOfFrame();
	}
}
