using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Menus
{
	public class MainMenu : MonoBehaviour
	{
		public void StartGame()
		{
			SceneManager.LoadScene("GameScene");
		}

		public void StartTutorial()
		{
			SceneManager.LoadScene("TutorialScene");
		}

		public void GoToSettings()
		{
			//
		}

		public void Quit()
		{
			Debug.Log("Quit button pressed!");
			Application.Quit();
		}
	}
}
