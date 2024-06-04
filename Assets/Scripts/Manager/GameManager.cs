using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
	[SerializeField] private UIVFX endScreen;
	[SerializeField] private Dictionary<string, CheckPointController> checkpoints;
	[SerializeField] private PlayerManager playerManager;
	[SerializeField] private Vector2 lastCheckointPosition;
	private static GameManager instance;

	public static GameManager Instance
	{
		get { return instance; }
		private set { instance = value; }
	}

	public Vector2 LastCheckointPosition { get => lastCheckointPosition; set => lastCheckointPosition = value; }

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

		checkpoints = new Dictionary<string, CheckPointController>();
		LastCheckointPosition = Vector2.zero;
		foreach (var checkpoint in FindObjectsByType<CheckPointController>(FindObjectsSortMode.InstanceID))
		{
			checkpoints.Add(checkpoint.CheckpointId, checkpoint);
		}
	}

	private void Start()
	{
		//AudioManager.insance.PlayBGMByIndex(0);
	}


	public void RestartGame()
	{
		SceneManager.LoadScene("MainScene");
	}


	public void BackToMenu()
	{
		SceneManager.LoadScene("StartGame");
	}

	public void LoadData(GameData data)
	{
		foreach (var checkpoint in data.checkpoints)
		{
			if (checkpoints.TryGetValue(checkpoint.Key, out CheckPointController checkpointController))
			{
				checkpointController.SetActivated(checkpoint.Value);
			}
		}
		lastCheckointPosition = data.lastCheckpointPosition;
	}

	public void SaveData(ref GameData data)
	{
		foreach (var checkpoint in checkpoints)
		{
			data.checkpoints.Add(checkpoint.Value.CheckpointId, checkpoint.Value.IsActivated);
		}
		data.lastCheckpointPosition = lastCheckointPosition;
	}
}
