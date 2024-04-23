[System.Serializable]
public class GameData
{
	public int currency;
	public SerializableDictionary<string, int> inventory;
	public SerializableDictionary<string, int> stash;

	public GameData()
	{
		currency = 0;
		inventory = new SerializableDictionary<string, int>();
		stash = new SerializableDictionary<string, int>();
	}

	public void ClearData()
	{
		currency = 0;
		inventory.Clear();
		stash.Clear();
	}
}
