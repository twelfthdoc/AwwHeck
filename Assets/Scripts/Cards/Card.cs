using UnityEngine;

public class Card : MonoBehaviour
{
	// Empty Base Constructor for Unity
	public Card() { }

	// Constructor with Property Initialisation
	public Card(CardRank rank, CardSuit suit)
	{
		Rank = rank;
		Suit = suit;
	}

	public CardRank Rank { get; set; }
	public CardSuit Suit { get; set; }

	public void PlayCard() => gameObject.GetComponentInParent<Hand>().PlayCard(this);

	public override string ToString() => $"{Rank} of {Suit}";
}
