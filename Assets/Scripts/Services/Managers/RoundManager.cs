using Random = UnityEngine.Random;

public class RoundManager : ManagerBase	// Possibly make into a singleton?
{
	public int dealerId;
	public int tricksPlayed = 0;
	public int roundNumber;

	public void Awake()
	{
		roundNumber = ServiceLocator.GetManager<GameManager>().RoundNumber;

		if (roundNumber == 1)
		{
			dealerId = Random.Range(0, 3);
		}
		else
		{
			dealerId = (dealerId + 1) % 4;
		}

		ServiceLocator.GetManager<GameManager>().UpdateDealer(dealerId);
	}

	//public void DealCards(int handSize) => ServiceLocator.GetManager<GameManager>().Deck.Deal(handSize);

	//public void UpdateTrumpSuit()
	//{
		
	//}

	//public Card PlayCard(int playerId, Card playerCard)
	//{
	//	var hand = ServiceLocator.GetManager<GameManager>().Hands.First(o => o.Id == playerId);
	//	return hand.RemoveCard(playerCard) ? playerCard : null;
	//}
}
