using System.Collections.Generic;
using System.Linq;

public class GameManager : ManagerBase
{
	protected ICollection<Hand> Hands;
	protected int _maxHandSize;
	protected CardDeck _deck;

	public int handSize;

	public string GameMode { get; protected set; }
	public int RoundNumber { get; protected set; } = 1;

	public virtual void Awake()
	{
		// Create Hands
		Hands = new List<Hand>
		{
			new() { Id = 0 }, // South - the Player
			new() { Id = 1 }, // West - NPC 1
			new() { Id = 2 }, // North - NPC 2
			new() { Id = 3 }  // East - NPC 3
		};

		// Create Deck
		_deck = new();

		// GameMode is determined by the settings/pre-game game mode selection
		GameMode = ServiceLocator.GetSingleton<Settings>().gameMode;

		// GameMode determines hand size and maximum hand size
		switch (GameMode)
		{
			default:
				handSize = 5;
				_maxHandSize = 10;
				break;
		}
	}

	public ICollection<Hand> GetHands() => Hands;
	public Hand GetPlayerHand() => Hands.First(h => h.IsPlayer);
}
