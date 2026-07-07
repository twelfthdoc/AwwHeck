using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
	private readonly IList<Card> newDeck;
	private static readonly System.Random seed = new();

	public IList<Card> Deck { get; set; }
	public GameObject CardPrefab;

	public void Start()
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
				CreateCard(rank, suit);
			}
		}
	}

	private void CreateCard(CardRank rank, CardSuit suit)
	{
		var cardObject = Instantiate(CardPrefab, transform);

		var card = cardObject.GetComponent<Card>();
		card.Rank = rank;
		card.Suit = suit;
		card.cardSprite = ServiceLocator.GetSingleton<Resources>().Sprites
				.FirstOrDefault(s => s.name == $"Cards/{suit}/{rank}");

		// Alternative code for Alt Sprite Tens
		// card.cardSprite = rank == CardRank.Ten ?
		//		ServiceLocator.GetSingleton<Resources>().Sprites.FirstOrDefault(s => s.name == $"Cards/{suit}/Ten_(Alt)") :
		//		ServiceLocator.GetSingleton<Resources>().Sprites.FirstOrDefault(s => s.name == $"Cards/{suit}/{rank}")


		newDeck.Add(card);
	}

	public void Deal()
	{
		var handSize = ServiceLocator.GetManager<GameManager>().handSize;

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
