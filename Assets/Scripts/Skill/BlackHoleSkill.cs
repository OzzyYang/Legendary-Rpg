using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public bool IsReleasingSkill { get; private set; }
	public GameObject BlackHole { get; private set; }
	public override bool CanUseSkill()
	{
		return base.CanUseSkill() && !IsReleasingSkill;
	}

	public override void UseSkill()
	{
		if (CanUseSkill())
		{
			IsReleasingSkill = true;
			BlackHole = BlackHole == null ? Instantiate(blackHoleObject) : BlackHole;
			BlackHole.SetActive(false);
			StartCoroutine(nameof(FlyUpForAndReleaseBlackHole), 0.25f);
			CoolDownTimer = skillCoolDownTime;
			Debug.Log(this.GetType() + " Used.");
		}
	}
	protected IEnumerator FlyUpForAndReleaseBlackHole(float _seconds)
	{
		player.SetVelocity(0, 24);
		yield return new WaitForSeconds(_seconds);
		player.stateMachine.ChangeState(player.blackHoleState);
		BlackHole.SetActive(true);
		BlackHole.GetComponent<BlackHoleController>().SetupBlackHole(blackHoleMaxSize, blackHoleGrowCurve, blackHoleGrowDuration, hotKetsSetting, qteDuration, player.transform.position);
		player.SetVelocity(0, -0.2f);
	}

	protected override void Awake()
	{
		base.Awake();
		if (unlockBlackHoleButton != null)
		{
			var buttonController = unlockBlackHoleButton.GetComponent<UISkillTreeSlotController>();
			void UnlockBlackHole()
			{
				Unlocked = buttonController.IsUnlocked();
				if (Unlocked) Unlocked = unlockBlackHoleButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockBlackHole;
		}
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(shortcut))
		{
			UseSkill();
		}
		if (BlackHole == null) IsReleasingSkill = false;
	}

}
