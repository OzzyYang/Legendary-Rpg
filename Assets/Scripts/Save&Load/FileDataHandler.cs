using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
	private string dataDirPath = "";
	private string dataFileName = "";

	public FileDataHandler(string dataDirPath, string dataFileName)
	{
		if (dataDirPath == null || dataFileName == null)
			throw new ArgumentNullException("Error: directory of data can't be null!");
		if (dataFileName == "" || dataFileName == "")
			throw new ArgumentException("Error: directory of data can't be empty!");
		this.dataDirPath = dataDirPath;
		this.dataFileName = dataFileName;
	}

	public void Save(GameData data)
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			string dataToStore = JsonUtility.ToJson(data, true);
			using (FileStream stream = new(fullPath, FileMode.Create))
			{
				using (StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(dataToStore);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"Error: can't save data to file: {fullPath}\n{e}");
		}
	}

	public GameData Load()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		GameData loadData = null;

		if (File.Exists(fullPath))
		{
			try
			{
				string dataToLoad = "";
				using (FileStream stream = new FileStream(fullPath, FileMode.Open))
				{
					using (StreamReader reader = new StreamReader(stream))
					{
						dataToLoad = reader.ReadToEnd();
					}
				}
				loadData = JsonUtility.FromJson<GameData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError($"Error: can't load data from file: {fullPath}\n{e}");
			}
		}
		return loadData;
	}
}
