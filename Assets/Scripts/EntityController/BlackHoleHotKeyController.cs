using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
	public KeyCode hotKeyCode { get; private set; }
	private TextMeshProUGUI hotKeyText;
	private BlackHoleController blackHole;
	private Transform enemy;

	private void Awake()
	{
		hotKeyText = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(hotKeyCode))
		{
			blackHole.AddEnemyAndKey(enemy, hotKeyCode);
			Destroy(this.gameObject);

		}
	}

	public void SetupHotkey(KeyCode _keyCode, BlackHoleController _blackHole, Transform _enemy)
	{
		this.hotKeyCode = _keyCode;
		this.hotKeyText.text = this.hotKeyCode.ToString();
		this.blackHole = _blackHole;
		this.enemy = _enemy;
	}

	private void OnDestroy()
	{
		//Unfreeze enemy who haven't been choosen
		//enemy?.GetComponent<EnemyController>().FreezeMovement(false);
	}


}
