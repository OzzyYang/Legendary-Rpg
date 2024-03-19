using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
	public KeyCode myHotKey { get; private set; }
	[SerializeField] private TextMeshProUGUI myText;
	private BlackHoleController blackHole;
	private Transform enemy;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(myHotKey))
		{
			blackHole.AddEnemyAndKey(enemy, myHotKey);
			Destroy(this.gameObject);

		}
	}

	public void SetupHotkey(KeyCode _keyCode, BlackHoleController _blackHole, Transform _enemy)
	{
		this.myHotKey = _keyCode;
		this.myText.text = this.myHotKey.ToString();
		this.blackHole = _blackHole;
		this.enemy = _enemy;
	}


}
