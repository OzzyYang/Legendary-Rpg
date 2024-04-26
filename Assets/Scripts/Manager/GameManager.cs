using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private UIVFX endScreen;
	private GameManager instance;



	public GameManager Instance
	{
		get { return instance; }
		private set { instance = value; }
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(instance.gameObject);
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("MainScene");
	}


	public void BackToMenu()
	{
		SceneManager.LoadScene("StartGame");
	}
}
