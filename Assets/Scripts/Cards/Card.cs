using UnityEngine;

public class Card : MonoBehaviour
{
	public Sprite cardBackground;
	public Sprite cardSprite;

	public CardRank Rank { get; set; }
	public CardSuit Suit { get; set; }

	public override string ToString() => $"{Rank} of {Suit}";
}
