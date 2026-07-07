public class SettingsMenu : Menu
{
	public void Start()
	{
		ServiceLocator.GetSingleton<Settings>().Load();
	}
}
