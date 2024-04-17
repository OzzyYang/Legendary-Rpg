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
	private bool canCreateBlackHole;
	[SerializeField] private UISkillTreeSlotController unlockBlackHoleButton;
	public bool isReleasingSkill { get; private set; }
	public GameObject blackHole { get; private set; }
	public override bool CanUseSkill()
	{
		if (!canCreateBlackHole) return false;
		if (coolDownTimer <= 0 && !isReleasingSkill)
		{
			coolDownTimer = skillCoolDownTime;
			return true;
		}

		return false;
	}

	public override void UseSkill()
	{
		if (CanUseSkill())
		{
			base.UseSkill();
			isReleasingSkill = true;
			blackHole = blackHole == null ? Instantiate(blackHoleObject) : blackHole;
			blackHole.SetActive(false);
			StartCoroutine("FlyUpForAndReleaseBlackHole", 0.25f);
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
		this.skillData.unlocked = true;
	}

	protected override void Start()
	{
		base.Start();
		if (this.unlockBlackHoleButton != null)
		{
			this.unlockBlackHoleButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canCreateBlackHole = this.unlockBlackHoleButton.IsUnlocked();
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
