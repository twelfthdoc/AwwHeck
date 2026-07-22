using System;
using System.Linq;

public class RoundManager : ManagerBase	// Possibly make into a singleton?
{
	public int dealerId;
	public int tricksPlayed = 0;
	public int roundNumber;
	public CardSuit trumpSuit;
	public CardDeck deck;

	public void Awake()
	{
		deck = new();

		roundNumber = ServiceLocator.GetManager<GameManager>().RoundNumber;
		if (roundNumber == 1)
		{
			dealerId = DetermineDealer();
		}
		else
		{
			dealerId = (dealerId + 1) % 4;
		}
	}

	public int DetermineDealer()
	{
		DealCards(1);

		var hands = ServiceLocator.GetManager<GameManager>().Hands;

		foreach (int position in Enum.GetValues(typeof(PlayerPosition)))
		{
			PlayCard(position, hands.First(o => o.Id == position).Cards.First());
		}

		// Compare cards, return id of player with highest card

		return 0;
	}

	public void DealCards(int handSize) => deck.Deal(handSize);

	public void UpdateTrumpSuit()
	{
		trumpSuit = deck.Peek().Suit;
	}

	public Card PlayCard(int playerId, Card playerCard)
	{
		var hand = ServiceLocator.GetManager<GameManager>().Hands.First(o => o.Id == playerId);
		return hand.RemoveCard(playerCard) ? playerCard : null;
	}






}
