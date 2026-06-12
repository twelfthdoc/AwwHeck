using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Cards
{
	public class CardDeck
	{
		public IList<Card> Deck { get; set; }

		private static readonly Random seed = new();

		public CardDeck()
		{
			NewDeck();
			ShuffleDeck();
		}

		public void ShuffleDeck() => Deck = Deck.OrderBy(_ => seed.Next()).ToList();

		public void NewDeck()
		{
			var ranks = Enum.GetValues(typeof(CardRank));
			var suits = Enum.GetValues(typeof(CardSuit));

			foreach (CardRank rank in ranks)
			{
				foreach (CardSuit suit in suits)
				{
					Deck.Add(new() { Rank = rank, Suit = suit });
				}
			}
		}
	}
}
