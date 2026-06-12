using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
	public int Id { get; set; }
	//public Player Owner;

	public IList<Card> Cards;

	public Hand()
	{
		Cards = new List<Card>();
	}

	public void EmptyHand()
	{
		Cards.Clear();
	}
}
