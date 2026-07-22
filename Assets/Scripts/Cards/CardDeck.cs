using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardDeck : MonoBehaviour
{
	private readonly IList<Card> newDeck = new List<Card>();
	private static readonly System.Random seed = new();

	public IList<Card> Cards { get; set; } = new List<Card>();
	public GameObject cardPrefab;
	public Sprite cardBackground;

	public void Awake()
	{
		NewDeck();
		ShuffleDeck();
	}

	public void ShuffleDeck() => Cards = Cards.OrderBy(_ => seed.Next()).ToList();

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

		Cards = newDeck;
	}

	private void CreateCard(CardRank rank, CardSuit suit)
	{
		var cardObject = Instantiate(cardPrefab, transform);
		var card = cardObject.AddComponent<Card>();
		card.Rank = rank;
		card.Suit = suit;
		cardObject.name = card.ToString();

		cardObject.GetComponent<SpriteRenderer>().sprite =
			rank == CardRank.Ten && ServiceLocator.GetSingleton<Settings>().displayTenAsLetter ?
			   ServiceLocator.GetSingleton<Resources>().Sprites.FirstOrDefault(s => s.name == $"{suit} Ten (Alt)") :
			   ServiceLocator.GetSingleton<Resources>().Sprites.FirstOrDefault(s => s.name == $"{suit} {rank}");

		newDeck.Add(card);
	}

	public void Deal(int handSize)
	{
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
		CardExtensions.UpdateTrumpSuit(Peek().Suit);

		gameObject.GetComponent<Image>().sprite = cardBackground;
		var card = GetSpecificCard(Peek());

		var instance = Instantiate(card, transform.parent);
		instance.name = card.name;
		instance.gameObject.GetComponent<SpriteRenderer>().enabled = true;
		instance.gameObject.GetComponent<SpriteRenderer>().sortingOrder++;
		instance.gameObject.GetComponentsInChildren<SpriteRenderer>().First(o => o.name == "Background").sortingOrder++;

		gameObject.GetComponentInChildren<Button>().interactable = false;
	}
}
