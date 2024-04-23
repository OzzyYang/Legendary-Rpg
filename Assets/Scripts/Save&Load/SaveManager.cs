using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance { get; private set; }
	[SerializeField] private string savedFileName;

	private GameData gameData;
	private List<ISaveManager> saveManagers;
	private FileDataHandler dataHandler;
	private void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);
		else
			Instance = this;
	}
	private void Start()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, savedFileName);
		saveManagers = FindAllSaveManagers();
		LoadGame();
	}

	public void NewGame()
	{
		gameData = new GameData();
	}

	public void LoadGame()
	{
		gameData = dataHandler.Load();

		if (gameData == null)
		{
			Debug.Log("No saved data found!");
			NewGame();
		}

		foreach (var saveManager in saveManagers)
		{
			saveManager.LoadData(gameData);
		}

		Debug.Log("Game loaded!");
		Debug.Log($"The currency is {gameData.currency}.");
	}
	public void SaveGame()
	{
		gameData.ClearData();
		foreach (var saveManager in saveManagers)
		{
			saveManager.SaveData(ref gameData);
		}
		dataHandler.Save(gameData);
		Debug.Log("Game was saved!");
		Debug.Log($"The currency is {gameData.currency}.");
	}

	private void OnApplicationQuit()
	{
		SaveGame();
#if UNITY_EDITOR
		System.Diagnostics.Process.Start(Path.Combine(Application.persistentDataPath, savedFileName));
#endif
	}

	private List<ISaveManager> FindAllSaveManagers()
	{
		IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
		return new List<ISaveManager>(saveManagers);

	}
}
