using UnityEngine;

public class Card : MonoBehaviour
{
	public CardRank Rank { get; set; }
	public CardSuit Suit { get; set; }

	public override string ToString() => $"{Rank} of {Suit}";

	public void OnMouseEnter()
	{
		
	}

	public void OnMouseExit()
	{
		
	}
}
