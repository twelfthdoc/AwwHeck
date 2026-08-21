using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class Hand : MonoBehaviour
{
	public int Id;
	public bool IsPlayer => Id == (int)PlayerPosition.South;

	public List<Card> Cards;

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
		card.transform.localScale = Vector3.one;

		return true;
	}

	public bool HasCard(Card card) => Cards.Any(c => c.Rank == card.Rank && c.Suit == card.Suit);

	public Card FindCard(CardRank rank, CardSuit suit) => Cards.FirstOrDefault(c => c.Rank == rank && c.Suit == suit);

	public bool RemoveCard(Card card)
	{
		var c = Cards.FirstOrDefault(c => c.Rank == card.Rank && c.Suit == card.Suit);

		if (c == null) return false;

		Cards.Remove(c);
		return true;
	}

	public void EmptyHand() => Cards.Clear();

	public void OrderHand()
	{
		Cards = Cards.OrderBy(c => c.Suit).ThenByDescending(c => c.Rank).ToList();

		var trumpSuit = CardExtensions.TrumpSuit;

		if (Cards.Any(c => c.Suit == trumpSuit))
		{
			var trumps = Cards.Where(c => c.Suit == trumpSuit).ToList();
			Cards = Cards.Where(c => !trumps.Contains(c)).ToList();
			Cards.InsertRange(0, trumps);
		}

		var rect = GetComponent<RectTransform>().rect;
		var width = rect.width / Cards.Count;
		var pos = -(rect.width * 0.5f) + (width * 0.5f);

		for (int i = 0; i < Cards.Count; i++)
		{
			Cards[i].transform.SetSiblingIndex(i);
			Cards[i].transform.SetLocalPositionAndRotation(new Vector3(pos + (i * width), 0.0f, 0.0f), Quaternion.identity);
		}
	}

	public void PlayCard(Card card)
	{
		if (!HasCard(card))
		{
			Debug.LogWarning("Card not found in hand!");
			return;
		}

		card.gameObject.GetComponent<Image>().sprite = ServiceLocator.GetManager<GameManager>().Deck.UpdateCardSprite(card.Rank, card.Suit);

		card.transform.SetParent(transform.Find("PlayedCard"));
		card.transform.localPosition = Vector3.zero;

		RemoveCard(card);
		OrderHand();

		if (IsPlayer)
		{
			ToggleCardButtons(false);

			if (ServiceLocator.GetManager<RoundManager>().forehand != (int)PlayerPosition.West)
			{
				ServiceLocator.GetManager<RoundManager>().GoToNextPlayer();
			}
		}
	}

	public void ToggleCardButtons(bool isPlayerTurn, CardSuit? suitLed = null)
	{
		if (IsPlayer)
		{
			if (suitLed != null)
			{
				var cards = Cards.Where(c => c.Suit == suitLed);

				if (cards.Count() != 0)
				{
					foreach (var card in cards)
					{
						card.GetComponent<Button>().enabled = isPlayerTurn;
					}

					return;
				}
			}

			foreach (var card in Cards)
			{
				card.GetComponent<Button>().enabled = isPlayerTurn;
			}
		}
	}
}
