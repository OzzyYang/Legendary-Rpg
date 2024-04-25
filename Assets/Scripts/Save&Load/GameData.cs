using System.Collections.Generic;

[System.Serializable]
public class GameData
{
	public int currency;
	public SerializableDictionary<string, int> inventory;
	public SerializableDictionary<string, int> stash;
	public List<string> equipments;
	public SerializableDictionary<string, bool> skills;

	public GameData()
	{
		currency = 0;
		inventory = new SerializableDictionary<string, int>();
		stash = new SerializableDictionary<string, int>();
		equipments = new List<string>();
		skills = new SerializableDictionary<string, bool>();
	}

	public void ClearData()
	{
		currency = 0;
		inventory.Clear();
		stash.Clear();
		equipments.Clear();
		skills.Clear();
	}
}
