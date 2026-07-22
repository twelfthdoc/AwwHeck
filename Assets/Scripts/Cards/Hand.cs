using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
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
		card.transform.SetParent(transform);

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

		return true;
	}

	public void EmptyHand() => Cards.Clear();

	public void OrderHand()
	{
		Cards = Cards.OrderBy(c => c.Suit).ThenByDescending(c => c.Rank).ToList();

		var rect = GetComponent<RectTransform>().rect;
		var width = rect.width / Cards.Count;
		var pos = -(rect.width * 0.5f) + (width * 0.5f);

		for (int i = 0; i < Cards.Count; i++)
		{
			Cards[i].transform.SetSiblingIndex(i);
			Cards[i].transform.SetLocalPositionAndRotation(new Vector3(pos + (i * width), 0.0f, 0.0f), Quaternion.identity);
		}
	}
}
