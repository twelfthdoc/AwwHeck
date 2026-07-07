using System.Collections.Generic;
using UnityEngine;

public class MenuManager : ManagerBase
{
	private readonly Stack<Menu> _menus = new();

	public virtual void OpenMenu(Menu menu)
	{
		if (_menus.Count > 0)
		{
			_menus.Peek().gameObject.SetActive(false);
		}

		var instance = Instantiate(menu, transform, false);
		_menus.Push(instance);
		instance.Open();
	}

	public virtual void CloseMenu()
	{
		if (_menus.Count == 0)
		{
			Debug.LogWarning("No menus to close!");
			return;
		}

		var menu = _menus.Pop();
		menu.Close();

		_menus.Peek().gameObject.SetActive(true);
	}

	public virtual void CloseAllMenus()
	{
		while (_menus.Count > 0)
		{
			_menus.Pop().Close();
		}
	}
}
