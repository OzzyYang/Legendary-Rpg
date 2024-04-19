using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackHoleSkill : Skill
{
	[Space]
	[Header("Skill Special Info")]
	[SerializeField] private GameObject blackHoleObject;
	[SerializeField] private GameObject hotKeyTextObject;
	[SerializeField] private float blackHoleMaxSize;
	[SerializeField] private float blackHoleGrowDuration;
	[SerializeField] private float qteDuration;
	[SerializeField] private AnimationCurve blackHoleGrowCurve;
	[SerializeField] private List<KeyCode> hotKetsSetting;
	[SerializeField] private UISkillTreeSlotController unlockBlackHoleButton;
	public bool isReleasingSkill { get; private set; }
	public GameObject blackHole { get; private set; }
	public override bool CanUseSkill()
	{
		return base.CanUseSkill() && !isReleasingSkill;
	}

	public override void UseSkill()
	{
		if (CanUseSkill())
		{
			isReleasingSkill = true;
			blackHole = blackHole == null ? Instantiate(blackHoleObject) : blackHole;
			blackHole.SetActive(false);
			StartCoroutine(nameof(FlyUpForAndReleaseBlackHole), 0.25f);
			coolDownTimer = skillCoolDownTime;
			Debug.Log(this.GetType() + " Used.");
		}

	}

	protected IEnumerator FlyUpForAndReleaseBlackHole(float _seconds)
	{
		player.SetVelocity(0, 24);
		yield return new WaitForSeconds(_seconds);
		player.stateMachine.ChangeState(player.blackHoleState);
		blackHole.SetActive(true);
		blackHole.GetComponent<BlackHoleController>().SetupBlackHole(blackHoleMaxSize, blackHoleGrowCurve, blackHoleGrowDuration, hotKetsSetting, qteDuration, player.transform.position);
		player.SetVelocity(0, -0.2f);
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		if (this.unlockBlackHoleButton != null)
		{
			this.unlockBlackHoleButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.unlocked = this.unlockBlackHoleButton.IsUnlocked();
			});
		}
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(shortcut))
		{
			UseSkill();
		}
		if (blackHole == null) isReleasingSkill = false;
	}

}
