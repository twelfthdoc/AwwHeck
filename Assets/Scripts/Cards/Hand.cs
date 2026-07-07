using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
	public int Id { get; set; }
	public bool IsPlayer => Id == 0;

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

		return Cards.Remove(c);
	}

	public void EmptyHand()
	{
		Cards.Clear();
	}

	public void OrderHand()
	{
		Cards = Cards.OrderBy(c => c.Suit).ThenByDescending(c => c.Rank).ToList();
	}
}
