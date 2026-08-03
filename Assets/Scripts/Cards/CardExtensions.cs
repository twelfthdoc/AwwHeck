public static class CardExtensions
{
	public static CardSuit TrumpSuit { get; private set; }

	public static void UpdateTrumpSuit(this CardSuit newTrumpSuit) => TrumpSuit = newTrumpSuit;

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
