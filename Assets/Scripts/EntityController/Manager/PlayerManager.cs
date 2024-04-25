using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
	public static PlayerManager Instance { get; private set; }
	[SerializeField] private PlayerController playerInstance;
	public PlayerController Player
	{
		get { return playerInstance; }
		private set { playerInstance = value; }
	}


	private void Awake()
	{
		Debug.Log("Player Manager");
		if (Instance != null)
			Destroy(Instance.gameObject);
		else
			Instance = this;
	}

	public void LoadData(GameData data)
	{

	}

	public void SaveData(ref GameData data)
	{

	}
}
