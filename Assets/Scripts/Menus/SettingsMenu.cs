using UnityEngine.UI;

public class SettingsMenu : Menu
{
	public bool displayTenAsLetter;

	public void OnEnable()
	{
		ServiceLocator.GetSingleton<Settings>().Load();
		displayTenAsLetter = ServiceLocator.GetSingleton<Settings>().displayTenAsLetter;
		gameObject.GetComponentInChildren<Toggle>().isOn = displayTenAsLetter;
	}

	public void ToggleCardDisplayControl()
	{
		displayTenAsLetter = !displayTenAsLetter;
		gameObject.GetComponentInChildren<Toggle>().isOn = displayTenAsLetter;
	}

	public override void ToPreviousMenu()
	{
		ServiceLocator.GetSingleton<Settings>().displayTenAsLetter = displayTenAsLetter;
		ServiceLocator.GetSingleton<Settings>().Save();
		base.ToPreviousMenu();
	}
}
