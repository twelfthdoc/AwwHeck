using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
	[SerializeField] private Menu settingsMenu;

	private void Awake()
	{
		// Ensures default values are saved to PlayerPrefs before first game
		ServiceLocator.GetSingleton<Settings>().Save();
	}

	public void StartGame()
	{
		ServiceLocator.DestroyManager<MenuManager>();
		SceneManager.LoadScene("Game");
	}

	public void StartTutorial()
	{
		ServiceLocator.DestroyManager<MenuManager>();
		SceneManager.LoadScene("Tutorial");
	}

	public void GoToSettings()
	{
		ServiceLocator.GetManager<MenuManager>().OpenMenu(settingsMenu);
	}

	public void Quit()
	{
		Debug.Log("Quit button pressed!");
		Application.Quit();
	}
}
