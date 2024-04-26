using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameSceneManager : MonoBehaviour
{
	[SerializeField] private Button continueButton;
	[SerializeField] UIVFX darkScreen;

	private void Start()
	{
		continueButton.gameObject.SetActive(SaveManager.Instance.CheckForSavedFile());
	}

	public async void Continue()
	{
		await darkScreen.FadeInAsync(1.2f);
		SceneManager.LoadScene("MainScene");
	}


	public async void NewGame()
	{
		await darkScreen.FadeInAsync(1.2f);
		SaveManager.Instance.NewGame();
		Continue();
	}

	public void Exit()
	{
		Debug.Log("Exit game.");
	}
}
