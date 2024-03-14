using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public static PlayerManager instance;
	public PlayerController player;

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
