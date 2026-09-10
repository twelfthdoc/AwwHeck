public class SettingsMenu : Menu
{
	public bool displayTenAsLetter;

	public void OnEnable()
	{
		ServiceLocator.GetSingleton<Settings>().Load();
		displayTenAsLetter = ServiceLocator.GetSingleton<Settings>().displayTenAsLetter;
	}

	public void ToggleCardDisplayControl()
	{
		displayTenAsLetter = !displayTenAsLetter;
	}

	public override void ToPreviousMenu()
	{
		ServiceLocator.GetSingleton<Settings>().displayTenAsLetter = displayTenAsLetter;
		ServiceLocator.GetSingleton<Settings>().Save();
		base.ToPreviousMenu();
	}
}
