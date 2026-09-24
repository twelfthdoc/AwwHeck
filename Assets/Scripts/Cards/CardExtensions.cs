
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

public static class CardExtensions
{
	public static CardSuit TrumpSuit { get; private set; }

	public static void UpdateTrumpSuit(this CardSuit newTrumpSuit) => TrumpSuit = newTrumpSuit;

	public static Card GetHighestCard(this Card[] cards, int forehand)
	{
		var firstIndex = (forehand + 4) % 4;
		var secondIndex = (firstIndex + 1) % 4;
		var thirdIndex = (secondIndex + 1) % 4;
		var fourthIndex = (thirdIndex + 1) % 4;

		return GetHighestCard(cards[firstIndex], cards[secondIndex], cards[thirdIndex], cards[fourthIndex]);
	}

	// Returns highest card in the trick
	public static Card GetHighestCard(this Card first, Card second, Card third, Card fourth) =>
		first.GetHigherCard(second).GetHigherCard(third).GetHigherCard(fourth);

	// Returns highest card for give number of cards
	public static Card GetHighestCard(this List<Card> cards)
	{
		cards = cards.Where(o => o != null).ToList();

		var highest = cards.FirstOrDefault();
		if (highest == null) return null;

		foreach (var card in cards)
		{
			highest = highest.GetHigherCard(card);
		}

		return highest;
	}

	// Returns the higher ranked card between two cards
	public static Card GetHigherCard(this Card left, Card right)
	{
		// If either card is null, the other wins by default
		if (right == null) return left;
		if (left == null) return right;

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
