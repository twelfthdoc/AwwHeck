using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMessages : SingletonBase
{
    public IEnumerator UpdateMessage()
	{
		var message = ServiceLocator.GetManager<TutorialManager>().message.GetComponent<Message>();

		if (message.name == "Scoring Message") yield break;

		foreach (var options in Messages())
		{
			yield return new WaitUntil(() => ServiceLocator.GetManager<TutorialManager>().SendNextMessage);
			yield return message.UpdateMessage(options);
			yield return new WaitWhile(() => ServiceLocator.GetManager<TutorialManager>().SendNextMessage);
		}
	}

	private IEnumerable<MessageOptions> Messages()
	{
		yield return new() { MessageText = "<b><u><smallcaps>Aww Heck!</smallcaps></u></b> is a card game for 4 players, similar to Hearts.\nThe goal is to score points by correctly predicting the exact number of tricks you will win.\nAfter all rounds have been played, the winner is the player with the most points!" };
		yield return new() { MessageText = "For this tutorial, we will play only one round with a hand size of 5 cards. In a full game, the hand size varies between 1 and 5 cards.\nEach round has 3 sections:\n  • bidding,\n  • playing, and\n  • scoring.", MessageBoxSize = new Vector2(750.0f, 475.0f) };
		yield return new() { MessageText = "The dealer is chosen at random, and the top card of the deck is turned over to determine the <i><u>trump suit</u></i>.\nTrump cards are special, and we will explain them later.", MessageBoxSize = new Vector2(750.0f, 325.0f) };
		yield return new() { MessageText = "You have been selected as the dealer!\nDealer's left bids first. The bid is a prediction of how many rounds of cards (<i><u>tricks</u></i>) they will win with the cards they hold.\nYou can bid any number between 0 and the number of cards in your hand." };
		yield return new() { MessageText = "Each player bids in turn. It's now your bid!\nYou have one high card (<color=#008080FF>♠️J</color>), one trump card (<color=orange>♦️6</color>), and no <color=red>Hearts</color> (<color=red>♥️</color>).\nLet's bid that we will win 2 tricks.", MessageBoxSize = new Vector2(750.0f, 300.0f) };
		yield return new() { MessageText = "Now that bidding is complete, we can start playing!\nDealer's left leads first, and can play any card from their hand.\nEveryone else <u>must</u> follow suit if they can.", MessageBoxSize = new Vector2(750.0f, 350.0f) };
		yield return new() { MessageText = "West leads with a high <color=#008080FF>Spade</color> (<color=#008080FF>♠️Q</color>). North does not have any <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>), so they have two options: discard or trump.\nA discard is a card from another suit and it ranks as a Zero - it can <u>never</u> win.", MessageBoxSize = new Vector2(750.0f, 325.0f) };
		yield return new() { MessageText = "However, North plays a <color=orange>Diamond</color> (<color=orange>♦️</color>) - a card that is in the trump suit! Trumps rank higher than every other suit, and the only way to beat a trump is with a higher one!", MessageBoxSize = new Vector2(750.0f, 275.0f) };
		yield return new() { MessageText = "Unfortunately for North, East has one! They also have no <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>), and their trump is a higher one (<color=orange>♦️8</color>) - East <i><u>overtrumps</u></i> North and is currently winning the trick!", MessageBoxSize = new Vector2(750.0f, 275.0f) };
		yield return new() { MessageText = "We have <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>), and so we must follow suit. There's no way for us to win this trick, so the best thing to do is play a low card.", MessageBoxSize = new Vector2(750.0f, 225.0f) };
		yield return new() { MessageText = "As East won the last trick, they now get to lead the next. They play a high <color=#0080FFFF>Club</color> (<color=#0080FFFF>♣️J</color>). We cannot win, so we play another low card.", MessageBoxSize = new Vector2(750.0f, 250.0f) };
		yield return new() { MessageText = "Each of the other players also follow suit with low cards. East wins Trick 2 as well, and is only one away from making their bid!", MessageBoxSize = new Vector2(750.0f, 250.0f) };
		yield return new() { MessageText = "East now plays a new suit, <color=red>Hearts</color> (<color=red>♥️10</color>), hoping to win easily. Which card do you think we should play?", MessageBoxSize = new Vector2(750.0f, 200.0f) };
		yield return new() { MessageText = "Yes! We are able to trump this trick because we have no <color=red>Hearts</color> (<color=red>♥️</color>)!", MessageBoxSize = new Vector2(750.0f, 175.0f) };
		yield return new() { MessageText = "You have won your first trick!\nYou now need to choose between which card to lead: the <color=#008080FF>Spade</color> (<color=#008080FF>♠️J</color>) or the <color=#0080FFFF>Club</color> (<color=#0080FFFF>♣️8</color>).\n<i>(One of these is slightly more optimal than the other.)</i>", MessageBoxSize = new Vector2(750.0f, 375.0f) };
		yield return new() { MessageText = "The only player that could still hold <color=#008080FF>Spades</color> (<color=#008080FF>♠️</color>) is West, but North or East may still have <color=orange>Diamonds</color> (<color=orange>♦️</color>) to trump.\nBy leading <color=#0080FFFF>Clubs</color> (<color=#0080FFFF>♣️</color>), we are attempting to win in a suit where there may be few cards remaining that can beat it.", MessageBoxSize = new Vector2(750.0f, 350.0f) };
		yield return new() { MessageText = "West discards a low <color=#008080FF>Spade</color> (<color=#008080FF>♠️</color>), indicating they had no trump cards. North and East both follow suit, and you win your second trick!\nThere's only one card left in hand, and we're hoping that North or East still has one more trump card - otherwise, we will go over our bid!" };
		yield return new() { MessageText = "North rescues our scoreline, and bags themselves points as well, by trumping the last trick!\nThe play is now complete, and so the round is now scored.\nRemember, only exact bids score points - everything else loses points!", MessageBoxSize = new Vector2(750.0f, 400.0f) };
		yield return new() { MessageText = "We lead the scoring for this round!\nWest and East lose points for their incorrect bids.\nNorth gains points, but not as many as us, as we successfully bid and made a higher number of tricks.", MessageBoxSize = new Vector2(750.0f, 350.0f) };
		yield return new() { MessageText = "This concludes the tutorial level. In a full game, several rounds are played, and the winner is the player with the highest score after all rounds.\nGood luck playing <b><u><smallcaps>Aww Heck!</smallcaps></u></b>", MessageBoxSize = new Vector2(750.0f, 350.0f) };
	}
}
