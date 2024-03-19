using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
	[SerializeField] private bool CanGrow;
	[SerializeField] private float maxSize;

	[SerializeField] private AnimationCurve easeCurve;
	[SerializeField] private float growDuration;

	[SerializeField] private List<KeyCode> hotKeysSetting;
	[SerializeField] private GameObject hotKeyTextObject;

	private List<GameObject> hotKeysChoosen;
	private List<Transform> enemiesList;
	private Vector3 originScale;
	private float growTimer;
	private Vector2 originPosition;
	private float growPercentage;
	private bool cloneCanAttack;
	private float qteDuration = 3.0f;
	private float waitForQTETimer;

	// Start is called before the first frame update
	void Start()
	{
		originScale = transform.localScale;
		originPosition = transform.position;
		enemiesList = new List<Transform>();
		hotKeysChoosen = new List<GameObject>();
		waitForQTETimer = qteDuration;

	}

	// Update is called once per frame
	void Update()
	{
		QTETimer();

		if (cloneCanAttack)
		{
			CloneAttack();
		}

		if (CanGrow)
		{
			growPercentage = growTimer / growDuration;
			growPercentage = easeCurve.Evaluate(growPercentage);
			transform.localScale = Vector2.Lerp(originScale, Vector2.one * maxSize, growPercentage);
			if (growPercentage < 1) growTimer += Time.deltaTime;
		}
		else
		{
			growPercentage = growTimer / growDuration;
			growPercentage = easeCurve.Evaluate(growPercentage);
			transform.localScale = Vector2.Lerp(originScale, Vector2.one * maxSize, growPercentage);
			if (growPercentage > 0) growTimer -= Time.deltaTime;
			if (growPercentage <= 0) Destroy(this.gameObject);
		}
	}

	private void CloneAttack()
	{
		foreach (var target in enemiesList)
		{
			SkillManager.instance.cloneSkill.CreateClone(target, new Vector2(1, 0));
		}
		//hotKeysSetting
		foreach (var hotkey in hotKeysChoosen)
		{
			if (hotkey != null)
			{
				hotKeysSetting.Add(hotkey.GetComponent<BlackHoleHotKeyController>().myHotKey);
				Destroy(hotkey);
			}
		}
		CanGrow = false;
	}

	private void QTETimer()
	{
		if (growPercentage >= 1)
		{
			waitForQTETimer -= Time.deltaTime;
			if (waitForQTETimer <= 0)
			{
				cloneCanAttack = true;
				waitForQTETimer = qteDuration;
			}
			else
			{
				cloneCanAttack = false;
			}
		}
		else
		{
			waitForQTETimer = qteDuration;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyController>() != null)
		{
			collision.GetComponent<EnemyController>().FreezeMovement(true);
			if (!cloneCanAttack)
				CreateHotKey(collision);
		}
	}

	private void CreateHotKey(Collider2D collision)
	{
		if (hotKeysSetting.Count <= 0)
		{
			return;
		}
		KeyCode choosenKey = hotKeysSetting[Random.Range(0, hotKeysSetting.Count)];
		GameObject hotkey = GameObject.Instantiate(hotKeyTextObject, collision.transform.position + new Vector3(0, 1), Quaternion.identity);
		hotkey.GetComponent<BlackHoleHotKeyController>().SetupHotkey(choosenKey, this, collision.transform);
		hotKeysChoosen.Add(hotkey);
		hotKeysSetting.Remove(choosenKey);
	}

	public void AddEnemyAndKey(Transform _enemy, KeyCode _choosenKey)
	{
		enemiesList.Add(_enemy);
		hotKeysSetting.Add(_choosenKey);
	}

}
