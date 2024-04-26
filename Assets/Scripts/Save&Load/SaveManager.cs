using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
	public static SaveManager Instance { get; private set; }
	[SerializeField] private string savedFileName;
	[SerializeField] private bool encryptData;
	[SerializeField] private bool needToSaveData = true;
	[SerializeField] private bool needToLoadData = true;
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
	}

	private void Start()
	{
		Debug.Log(SceneManager.GetActiveScene().name);
		if (needToLoadData) LoadGame();
	}

	public void NewGame()
	{
		DeleteFileData();
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
	}

	private void OnApplicationQuit()
	{
		if (needToSaveData) SaveGame();
	}

	private List<ISaveManager> FindAllSaveManagers()
	{
		IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
		return new List<ISaveManager>(saveManagers);

	}

	public bool CheckForSavedFile() => dataHandler.Check();
}
