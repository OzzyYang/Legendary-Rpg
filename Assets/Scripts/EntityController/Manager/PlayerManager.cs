using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance { get; private set; }
	[SerializeField] private PlayerController playerInstance;
	public PlayerController player
	{
		get { return playerInstance; }
		private set { playerInstance = value; }
	}


	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
