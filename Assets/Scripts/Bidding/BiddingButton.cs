using UnityEngine;
using UnityEngine.EventSystems;

public class BiddingButton : MonoBehaviour, IPointerClickHandler
{
	public bool IsClicked { get; private set; } = false;

	public void OnPointerClick(PointerEventData eventData)
	{
		IsClicked = true;
	}

	public void Reset()
	{
		IsClicked = false;
	}
}
