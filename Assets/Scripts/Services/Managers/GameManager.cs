using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : ManagerBase
{
	protected int _maxHandSize;

	public int handSize;
	public Sprite cardBack;

	public ICollection<Hand> Hands { get; protected set; }
	public string GameMode { get; protected set; }
	public int RoundNumber { get; protected set; } = 1;

	public virtual void Awake()
	{
		// Create Hands
		Hands = FindFirstObjectByType<Canvas>().GetComponentsInChildren<Hand>();

		// GameMode is determined by the settings/pre-game game mode selection
		GameMode = ServiceLocator.GetSingleton<Settings>().gameMode;

		// GameMode determines hand size and maximum hand size
		switch (GameMode)
		{
			case "Tutorial":
				// This should never happen! Tutorial Manager explicitly overrides these settings
				handSize = 5;
				_maxHandSize = 5;
				break;
			default:
				handSize = 5;
				_maxHandSize = 10;
				break;
		}
	}

	public ICollection<Hand> GetHands() => Hands;
	public Hand GetPlayerHand() => Hands.First(h => h.IsPlayer);

	public void DealCards(int handSize) => ServiceLocator.GetManager<RoundManager>().DealCards(handSize);

}
