using UnityEngine;

public class Card : MonoBehaviour
{
	public CardRank Rank { get; set; }
	public CardSuit Suit { get; set; }

	public void PlayCard() => StartCoroutine(GetComponentInParent<Hand>().PlayCard(this));

	public override string ToString() => $"{Rank} of {Suit}";
}
