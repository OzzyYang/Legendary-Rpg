using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance { get; private set; }
	[SerializeField] private string savedFileName;
	[SerializeField] private bool encryptData;
	private GameData gameData;
	private List<ISaveManager> saveManagers;
	private FileDataHandler dataHandler;


	[ContextMenu("Delete Data")]
	private void DeleteFileData()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, savedFileName, encryptData);
		dataHandler.Delete();
	}
	[ContextMenu("Open Data")]
	private void OpenFileData()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, savedFileName, encryptData);
		dataHandler.OpenInEditor();
	}

	private void Awake()
	{
		if (Instance != null)
			Destroy(gameObject);
		else
			Instance = this;

		dataHandler = new FileDataHandler(Application.persistentDataPath, savedFileName, encryptData);
		saveManagers = FindAllSaveManagers();
		//LoadGame();
	}

	private void Start()
	{
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
	}

	private List<ISaveManager> FindAllSaveManagers()
	{
		IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
		return new List<ISaveManager>(saveManagers);

	}
}
