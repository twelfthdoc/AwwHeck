using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
	public int Id;
	public bool IsPlayer => Id == (int)PlayerPosition.South;

	public IList<Card> Cards;

	public void Awake()
	{
		Cards = new List<Card>();
	}

	public bool AddCard(Card card)
	{
		if (Cards.Any(c => c.Rank == card.Rank && c.Suit == card.Suit))
		{
			return false;
		}

		Cards.Add(card);

		var newCard = Instantiate(card, transform);
		newCard.name = card.name;

		return true;
	}

	public bool HasCard(Card card) => Cards.Any(c => c.Rank == card.Rank && c.Suit == card.Suit);

	public bool RemoveCard(Card card)
	{
		var c = Cards.FirstOrDefault(c => c.Rank == card.Rank && c.Suit == card.Suit);

		if (c == null)
		{
			return false;
		}

		Cards.Remove(c);
		Destroy(c.gameObject);

		return true;
	}

	public void EmptyHand()
	{
		foreach (var card in Cards)
		{
			Destroy(card.gameObject);
		}

		Cards.Clear();
	}

	public void OrderHand()
	{
		Cards = Cards.OrderBy(c => c.Suit).ThenByDescending(c => c.Rank).ToList();
	}
}

public enum PlayerPosition
{
	South = 0,
	West,
	North,
	East
}
