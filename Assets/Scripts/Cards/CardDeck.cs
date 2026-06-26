using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Assets.Scripts.Services;

namespace Assets.Scripts.Cards
{
	public class CardDeck
	{
		public IList<Card> Deck { get; set; }
		private readonly IList<Card> newDeck;

		private static readonly Random seed = new();

		public CardDeck()
		{
			NewDeck();
			ShuffleDeck();
		}

		public void ShuffleDeck() => Deck = newDeck.OrderBy(_ => seed.Next()).ToList();

		private void NewDeck()
		{
			var ranks = Enum.GetValues(typeof(CardRank));
			var suits = Enum.GetValues(typeof(CardSuit));

			foreach (CardRank rank in ranks)
			{
				foreach (CardSuit suit in suits)
				{
					newDeck.Add(new()
					{
						Rank = rank,
						Suit = suit,
						cardSprite = ServiceLocator.GetSingleton<ResourcesSingleton>().Sprites
							.FirstOrDefault(s => s.name == $"Cards/{suit}/{rank}")

						// Alternative code for Alt Sprite Tens
							//cardSprite = rank == CardRank.Ten ?
							//	ServiceLocator.GetSingleton<ResourcesSingleton>().Sprites.FirstOrDefault(s => s.name == $"Cards/{suit}/Ten_(Alt)") :
							//	ServiceLocator.GetSingleton<ResourcesSingleton>().Sprites.FirstOrDefault(s => s.name == $"Cards/{suit}/{rank}")
					});
				}
			}
		}

		public void Deal()
		{
			var handSize = ServiceLocator.GetManager<GameManager>().HandSize;

			foreach (var hand in ServiceLocator.GetManager<GameManager>().GetHands())
			{
				hand.Cards.AddRange(Deck.Take(handSize));
				Deck = Deck.Skip(handSize).ToList();
			}

			UpdateTrumpSuit();
		}

		public Card Peek() => Deck.First();

		public void UpdateTrumpSuit() => CardExtensions.UpdateTrumpSuit(Peek().Suit);
	}
}
