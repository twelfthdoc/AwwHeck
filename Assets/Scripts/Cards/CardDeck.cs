using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour
{
	private readonly IList<Card> newDeck = new List<Card>();
	private static readonly System.Random seed = new();

	public IList<Card> Cards { get; set; }
	public GameObject cardPrefab;

	public void Awake()
	{
		if (newDeck.Count == 0)
		{
			NewDeck();
		}

		RefreshDeck();
		ShuffleDeck();
	}

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

	public void RefreshDeck() => Cards = newDeck;

	public void ShuffleDeck()
	{
		Cards = Cards.OrderBy(_ => seed.Next()).ToList();

		foreach (var card in Cards)
		{
			card.transform.SetAsFirstSibling();
		}
	}

	private void CreateCard(CardRank rank, CardSuit suit)
	{
		var cardObject = Instantiate(cardPrefab, transform);

		var card = cardObject.AddComponent<Card>();
		card.Rank = rank;
		card.Suit = suit;

		cardObject.name = card.ToString();
		cardObject.GetComponent<Image>().sprite = UpdateCardSprite(rank, suit);

		newDeck.Add(card);
	}

	public Sprite UpdateCardSprite(CardRank rank, CardSuit suit) =>
		rank == CardRank.Ten && ServiceLocator.GetSingleton<Settings>().displayTenAsLetter ?
			ServiceLocator.GetSingleton<Resources>().Sprites.FirstOrDefault(s => s.name == $"{suit} Ten (Alt)") :
			ServiceLocator.GetSingleton<Resources>().Sprites.FirstOrDefault(s => s.name == $"{suit} {rank}");

	public void UpdateDealer(PlayerPosition dealer)
	{
		var backgrounds = ServiceLocator.GetSingleton<Resources>().Dealers;

		gameObject.GetComponents<Image>().First(o => o.name == "Deck").sprite = dealer switch
		{
			PlayerPosition.South => backgrounds.FirstOrDefault(d => d.name == $"Dealer South"),
			PlayerPosition.East => backgrounds.FirstOrDefault(d => d.name == $"Dealer East"),
			PlayerPosition.North => backgrounds.FirstOrDefault(d => d.name == $"Dealer North"),
			PlayerPosition.West => backgrounds.FirstOrDefault(d => d.name == $"Dealer West"),
			_ => backgrounds.FirstOrDefault(d => d.name == $"Dealer None"),
		};
	}

	public void Deal()
	{
		var handSize = ServiceLocator.GetManager<GameManager>().handSize;

		foreach (var hand in ServiceLocator.GetManager<GameManager>().GetHands())
		{
			hand.Cards.AddRange(Cards.Take(handSize));
			Cards = Cards.Skip(handSize).ToList();
		}

		UpdateTrumpSuit();
	}

	public Card GetSpecificCard(Card card) => GetSpecificCard(card.Rank, card.Suit);

	public Card GetSpecificCard(CardRank rank, CardSuit suit)
	{
		var foundCard = Cards.FirstOrDefault(o => o.Rank == rank && o.Suit == suit);

		if (foundCard == null)
		{
			Debug.LogError($"{rank} of {suit} not found!");
			throw new NullReferenceException($"{rank} of {suit} not found!");
		}

		Cards.Remove(foundCard);

		return foundCard;
	}

	public Card Peek() => Cards.First();

	public void UpdateTrumpSuit()
	{
		var card = Peek();
		card.transform.SetAsLastSibling();
		card.Suit.UpdateTrumpSuit();

		gameObject.GetComponent<Button>().interactable = false;
	}
}
