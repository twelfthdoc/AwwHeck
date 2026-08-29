using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessages : SingletonBase
{
    public IEnumerator UpdateMessage()
	{
		var message = ServiceLocator.GetManager<TutorialManager>().message.GetComponent<Message>();

		foreach (var options in Messages())
		{
			yield return new WaitUntil(() => ServiceLocator.GetManager<TutorialManager>().SendNextMessage);
			yield return message.UpdateMessage(options);
			yield return new WaitWhile(() => ServiceLocator.GetManager<TutorialManager>().SendNextMessage);
		}
	}

	private IEnumerable<MessageOptions> Messages()
	{
		yield return new() { MessageText = "<b>Aww Heck!</b> is a card game for 4 players, similar to Hearts.\nThe goal is to score points by correctly predicting the exact number of tricks you will win.\nAfter all rounds have been played, the winner is the player with the most points!" };
		yield return new() { MessageText = "For this tutorial, we will play only one round with a hand size of 5 cards. In a full game, the hand size varies between 1 and 5 cards.\nEach round has 3 sections:\n  • bidding,\n  • playing, and\n  • scoring.", MessageBoxSize = new Vector2(750.0f, 475.0f) };
		yield return new() { MessageText = "The dealer is chosen at random, and the top card of the deck is turned over to determine the <i><u>trump suit</u></i>.\nTrump cards are special, and we will explain them later.", MessageBoxSize = new Vector2(750.0f, 325.0f) };
		yield return new() { MessageText = "You have been selected as the dealer!\nDealer's left bids first. The bid is a prediction of how many rounds of cards (<i><u>tricks</u></i>) they will win with the cards they hold.\nYou can bid any number between 0 and the number of cards in your hand." };
		yield return new() { MessageText = "Each player bids in turn. It's now your bid!\nYou have one high card (<color=#008080FF>♠️J</color>), one trump card (<color=orange>♦️6</color>), and no <color=red>Hearts</color> (<color=red>♥️</color>).\nLet's bid that we will win 2 tricks.", MessageBoxSize = new Vector2(750.0f, 300.0f) };
		yield return new() { MessageText = "Now that bidding is complete, we can start playing!\nDealer's left leads first, and can play any card from their hand.\nEveryone else <u>must</u> follow suit if they can.", MessageBoxSize = new Vector2(750.0f, 350.0f) };
		yield return new() { MessageText = "West leads with a high <color=#008080FF>Spade</color> (<color=#008080FF>♠️Q</color>). North does not have any <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>), so they have two options: discard or trump.\nA discard is a card from another suit and it ranks as a Zero - it can <u>never</u> win.", MessageBoxSize = new Vector2(750.0f, 325.0f) };
		yield return new() { MessageText = "However, North plays a <color=orange>Diamond</color> (<color=orange>♦️</color>) - a card that is in the trump suit! Trumps rank higher than every other suit, and the only way to beat a trump is with a higher one!", MessageBoxSize = new Vector2(750.0f, 275.0f) };
		yield return new() { MessageText = "Unfortunately for North, East has one! They also have no <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>), and their trump is a higher one (<color=orange>♦️8</color>)! East <i><u>overtrumps</u></i> North and is currently winning the trick!", MessageBoxSize = new Vector2(750.0f, 275.0f) };
		yield return new() { MessageText = "We have <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>), and so we must follow suit. There's no way for us to win this trick, so the best thing to do is play a low card.", MessageBoxSize = new Vector2(750.0f, 225.0f) };
		yield return new() { MessageText = "As East won the last trick, they now get to lead the next." };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
		yield return new() { MessageText = "" };
	}
}
