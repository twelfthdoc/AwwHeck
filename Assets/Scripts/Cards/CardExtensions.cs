using System;
using Unity.VisualScripting;

public static class CardExtensions
{
	public static CardSuit TrumpSuit { get; private set; }

	public static void UpdateTrumpSuit(this CardSuit newTrumpSuit) => TrumpSuit = newTrumpSuit;

	public static Card GetHighestCard(this Card[] cards) => cards[0].GetHighestCard(cards[1], cards[2], cards[3]);

	// Returns highest card in the trick
	public static Card GetHighestCard(this Card first, Card second, Card third, Card fourth) =>
		first.GetHigherCard(second).GetHigherCard(third).GetHigherCard(fourth);

	// Returns the higher ranked card between two cards
	public static Card GetHigherCard(this Card left, Card right)
	{
		// If only one card is a trump, trumps win
		if (left.Suit == TrumpSuit && right.Suit != TrumpSuit) return left;
		if (left.Suit != TrumpSuit && right.Suit == TrumpSuit) return right;

		// If the second card doesn't follow suit, it is a discard - left wins
		if (left.Suit != right.Suit) return left;

		// Finally, compare rank. There are no ties.
		return left.Rank > right.Rank ? left : right;
	}

	// Returns true if supplied suit and rank are equal to that of the card
	public static bool Equals(this Card card, CardRank rank, CardSuit suit) => card.Rank == rank && card.Suit == suit;

	// Override method for assigning rank symbols to text
	public static string ToString(this CardRank rank) => rank switch
	{
		CardRank.Ace => "A",
		CardRank.King => "K",
		CardRank.Queen => "Q",
		CardRank.Jack => "J",
		_ => $"{(int)rank}"
	};
}
