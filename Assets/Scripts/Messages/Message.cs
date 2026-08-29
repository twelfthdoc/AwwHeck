using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class Message : MonoBehaviour
{
	public IEnumerator UpdateMessage(MessageOptions options)
	{
		gameObject.GetComponent<RectTransform>().sizeDelta = options.MessageBoxSize;
		var messageText = GetComponentInChildren<TextMeshProUGUI>();
		messageText.text = options.MessageText;
		messageText.fontSize = options.FontSize;

		gameObject.SetActive(true);
		yield break;
	}

	public void OnDisable() => ServiceLocator.GetManager<TutorialManager>().SendNextMessage = false;
}
