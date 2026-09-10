using UnityEngine;

public abstract class Menu : MonoBehaviour
{
	public virtual void Open()
	{
		gameObject.SetActive(true);
	}

	public virtual void Close()
	{
		Destroy(gameObject);
	}

	public virtual void OpenMenu(Menu menu) => ServiceLocator.GetManager<MenuManager>().OpenMenu(menu);

	public virtual void ToPreviousMenu() => ServiceLocator.GetManager<MenuManager>().CloseMenu();
}
