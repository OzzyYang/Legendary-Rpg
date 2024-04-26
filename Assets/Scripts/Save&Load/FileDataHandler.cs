using System;

using System.IO;
using System.Text;
using UnityEngine;

public class FileDataHandler
{
	private string dataDirPath = "";
	private string dataFileName = "";

	private string codeWords = "ozzyyang";
	private bool encryptData;

	public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
	{
		if (dataDirPath == null || dataFileName == null)
			throw new ArgumentNullException("Error: directory of data can't be null!");
		if (dataFileName == "" || dataFileName == "")
			throw new ArgumentException("Error: directory of data can't be empty!");
		this.dataDirPath = dataDirPath;
		this.dataFileName = dataFileName;
		this.encryptData = encryptData;
	}

	public void Save(GameData data)
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
			string dataToStore = JsonUtility.ToJson(data, false);
			if (encryptData) dataToStore = EncryptData(dataToStore);
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
				if (encryptData) dataToLoad = EncryptData(dataToLoad);
				loadData = JsonUtility.FromJson<GameData>(dataToLoad);
			}
			catch (Exception e)
			{
				Debug.LogError($"Error: can't load data from file: {fullPath}\n{e}");
			}
		}
		return loadData;
	}

	public bool Check()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);
		return File.Exists(fullPath);
	}

	public void Delete()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);

		if (File.Exists(fullPath)) File.Delete(fullPath);
	}

	public void OpenInEditor()
	{
		string fullPath = Path.Combine(dataDirPath, dataFileName);

		if (File.Exists(fullPath)) System.Diagnostics.Process.Start(fullPath);
		else Debug.Log("The data file is not existed.");
	}

	private string EncryptData(string data)
	{
		var result = new StringBuilder();

		for (int i = 0; i < data.Length; i++)
		{
			result.Append((char)(data[i] ^ codeWords[i % codeWords.Length]));
		}

		return result.ToString();
	}
}
