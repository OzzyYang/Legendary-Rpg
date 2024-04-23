using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
	#region Grow Info
	private bool CanGrow = true;
	private Vector3 originScale;
	private float maxSize;
	private Vector2 originPosition;
	private AnimationCurve growEaseCurve;
	private float growDuration;
	private float growTimer;
	private float growPercentage;
	#endregion

	#region Hotkey Info
	[SerializeField] private GameObject hotKeyTextObject;
	private List<KeyCode> hotKeysSetting;
	private List<GameObject> hotKeysChoosen;
	private List<Transform> enemiesList;
	#endregion

	#region QTE Info
	private bool cloneCanAttack;
	private float qteDuration = 0.5f;
	private float waitForQTETimer;
	#endregion
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
			growPercentage = growEaseCurve.Evaluate(growPercentage);
			transform.localScale = Vector2.Lerp(originScale, Vector2.one * maxSize, growPercentage);
			if (growPercentage < 1) growTimer += Time.deltaTime;
		}
		else
		{
			growPercentage = growTimer / growDuration;
			growPercentage = growEaseCurve.Evaluate(growPercentage);
			transform.localScale = Vector2.Lerp(originScale, Vector2.one * maxSize, growPercentage);
			if (growPercentage > 0) growTimer -= Time.deltaTime;
			if (growPercentage <= 0) Destroy(this.gameObject);
		}
	}

	public void SetupBlackHole(float _maxSize, AnimationCurve _growEaseCurve, float _growDuration, List<KeyCode> _hotKeysSetting, float _qteDuration, Vector2 _position)
	{
		this.maxSize = _maxSize;
		this.growDuration = _growDuration;
		this.growEaseCurve = _growEaseCurve;
		this.hotKeysSetting = _hotKeysSetting;
		this.qteDuration = _qteDuration;
		transform.position = this.originPosition = _position;
	}

	private void CloneAttack()
	{
		foreach (var target in enemiesList)
		{
			SkillManager.instance.cloneSkill.UseSkill(target, new Vector2(1, 0));
		}
		//reset hotkeys that haven't been choosen, and destroy the hotkey text object
		foreach (var hotkey in hotKeysChoosen)
		{
			if (hotkey != null)
			{
				hotKeysSetting.Add(hotkey.GetComponent<BlackHoleHotKeyController>().hotKeyCode);
				Destroy(hotkey);
			}
		}

		if (enemiesList.Count > 0)
		{
			StartCoroutine("ShrinkBlackHoleAfter", 0.8f);
		}
		else
		{
			ShrinkBlackHole();
		}

	}
	private IEnumerator ShrinkBlackHoleAfter(float _seconds)
	{
		yield return new WaitForSeconds(_seconds);
		ShrinkBlackHole();
	}
	private void ShrinkBlackHole()
	{
		CanGrow = false;
		PlayerManager.Instance.Player.stateMachine.ChangeState(PlayerManager.Instance.Player.levitateState);

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
			if (CanGrow)
			{
				collision.GetComponent<EnemyController>().FreezeMovement(true);
				CreateHotKey(collision);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<EnemyController>() != null)
		{
			collision.GetComponent<EnemyController>()?.FreezeMovement(false);
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

	public bool isAttackingFinished() => !CanGrow;

}
