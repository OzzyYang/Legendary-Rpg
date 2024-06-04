using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
	public int currency;
	public SerializableDictionary<string, int> inventory;
	public SerializableDictionary<string, int> stash;
	public List<string> equipments;
	public SerializableDictionary<string, bool> skills;
	public SerializableDictionary<string, bool> checkpoints;
	public Vector2 lastCheckpointPosition;
	//each volume setting is <AudioMixer Params, Volume Value>
	public SerializableDictionary<string, float> volumeSettings;

	public GameData()
	{
		currency = 0;
		inventory = new SerializableDictionary<string, int>();
		stash = new SerializableDictionary<string, int>();
		equipments = new List<string>();
		skills = new SerializableDictionary<string, bool>();
		checkpoints = new SerializableDictionary<string, bool>();
		lastCheckpointPosition = new Vector2();
		volumeSettings = new SerializableDictionary<string, float>();
	}

	public void ClearData()
	{
		currency = 0;
		inventory.Clear();
		stash.Clear();
		equipments.Clear();
		skills.Clear();
		checkpoints.Clear();
		lastCheckpointPosition = new Vector2();
		volumeSettings.Clear();
	}
}
