using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu
{
	[SerializeField] private Menu settingsMenu;

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
