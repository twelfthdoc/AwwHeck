using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : ManagerBase
{
	public int handSize;
	public Sprite cardBack;
	public CardDeck deckPrefab;
	public GameObject biddingButtons;
	public GameObject roundManagerPrefab;
	public GameObject scoringMessagePrefab;

	public PlayerPosition Dealer { get; protected set; }
	public CardDeck Deck { get; protected set; }
	public ICollection<Hand> Hands { get; protected set; }
	public string GameMode { get; protected set; }
	public int RoundNumber { get; protected set; } = 1;

	protected int _maxHandSize;
	private static bool _validBid;
	private Coroutine _animateLabels;

	public virtual void Start()
	{
		// Create New Scoring Singleton
		ServiceLocator.GetSingleton<Scoring>();

		// Create New NpcBehaviour Singleton
		ServiceLocator.GetSingleton<NpcBehaviour>();

		// Create Hands
		Hands = FindFirstObjectByType<Canvas>().GetComponentsInChildren<Hand>();

		// GameMode is determined by the settings/pre-game game mode selection
		GameMode ??= ServiceLocator.GetSingleton<Settings>().gameMode;

		// GameMode determines hand size and maximum hand size
		switch (GameMode)
		{
			case "Tutorial":
				handSize = 5;
				_maxHandSize = 5;
				break;
			default:
				handSize = 1;
				_maxHandSize = 5;
				break;
		}

		Deck = Instantiate(deckPrefab, new Vector3(360, 173), Quaternion.identity, FindFirstObjectByType<Canvas>().transform);
		Deck.transform.localScale = new Vector3(0.75f, 0.75f);
		Deck.name = deckPrefab.name;
		Deck.transform.SetSiblingIndex(Deck.transform.parent.childCount - 2);

		ServiceLocator.GetSingleton<Scoring>().NewRound();
	}

	public ICollection<Hand> GetHands() => Hands;
	public Hand GetPlayerHand() => Hands.First(h => h.IsPlayer);

	public IEnumerator GetPlayerBid()
	{
		if (GameMode == "Tutorial")
		{
			yield return ServiceLocator.GetManager<TutorialManager>().SendMessage();
		}

		_validBid = false;

		ShowBiddingButtons();

		yield return ButtonPressed();
		yield return new WaitUntil(() => _validBid);

		HideBiddingButtons();
	}

	public void ShowBiddingButtons()
	{
		biddingButtons.SetActive(true);

		if (GameMode == "Tutorial")
		{
			var button = biddingButtons.GetComponentsInChildren<Button>().First(b => b.name == "2");
			button.interactable = true;
			return;
		}

		foreach (var button in biddingButtons.GetComponentsInChildren<Button>())
		{
			if (int.Parse(button.name) <= handSize)
			{
				button.interactable = true;
			}
		}
	}

	public IEnumerator ButtonPressed()
	{
		yield return new WaitUntil(() =>
			biddingButtons.GetComponentsInChildren<Button>()
				.Any(b => b.gameObject.GetComponent<BiddingButton>().IsClicked));
	}

	public void HideBiddingButtons()
	{
		foreach (var button in biddingButtons.GetComponentsInChildren<Button>())
		{
			button.interactable = false;
			button.gameObject.GetComponent<BiddingButton>().Reset();
		}

		biddingButtons.SetActive(false);
	}

	public void SetPlayerBid(int bid)
	{
		if (bid == 0)
		{
			_validBid = false;
			return;
		}

		bid %= 10;
		ServiceLocator.GetSingleton<Scoring>().NewBid(0, bid);
		_validBid = true;
	}

	public void UpdateDealer(int dealerId)
	{
		Dealer = (PlayerPosition)dealerId;
		Deck.UpdateDealer(Dealer);
	}

	public void UpdateLabel(int playerId)
	{
		var tricksWon = ServiceLocator.GetSingleton<Scoring>().Tricks[playerId];
		var bid = ServiceLocator.GetSingleton<Scoring>().Bids[playerId];

		var textObject = Hands.First(h => h.Id == playerId).GetComponentsInChildren<TextMeshProUGUI>().First(o => o.name == "Label");
		textObject.text = $"{(PlayerPosition)playerId}\n{tricksWon}/{bid}";

		if (tricksWon > bid)
		{
			textObject.color = Color.red;
		}

		if (tricksWon == bid)
		{
			textObject.color = Color.green;
		}

		if (tricksWon == bid -1)
		{
			textObject.color = Color.yellow;
		}

		if (_animateLabels != null)
		{
			StopCoroutine(_animateLabels);
		}

		_animateLabels = StartCoroutine(AnimateText(textObject));
	}

	public IEnumerator AnimateText(TMP_Text text)
	{
		var timer = 0.0f;
		var shrinkTime = 0.5f;

		var baseSize = text.fontSize;
		text.fontSize += 20.0f;

		while (timer < shrinkTime)
		{
			timer += Time.deltaTime;
			text.fontSize = Mathf.Lerp(baseSize + 20.0f, baseSize, timer / shrinkTime);
			yield return null;
		}
	}

	public GameObject InstantiateScoreMessagePrefab() => Instantiate(scoringMessagePrefab, FindFirstObjectByType<Canvas>().transform);

	// Should only be called at end of game, or when user quits game from submenu
	public void AtEndOfGame()
	{
		// Tidyup / await any user input


		// Destroy dependencies
		ServiceLocator.DestroySingleton<NpcBehaviour>();
		ServiceLocator.DestroySingleton<Scoring>();

		// Return to Main Menu
		SceneManager.LoadScene("MainMenu");
	}

	public void OrderHands()
	{
		foreach (var hand in Hands)
		{
			hand.OrderHand();
		}
	}
}
