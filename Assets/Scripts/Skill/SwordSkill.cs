using System.Collections.Generic;
using UnityEngine;
public enum SwordType
{
	Regular,
	Bounce,
	Pierce,
	Spin
}
public class SwordSkill : Skill
{
	[SerializeField] private GameObject swordObject;
	[SerializeField] private SwordType swordType;
	[SerializeField] private float throwForce = 12;
	private float currentThrowForce;

	[Header("Bounce Info")]
	[SerializeField] private UISkillTreeSlotController unlockBouncySwordButton;
	[Range(0f, 5f)]
	[SerializeField] private float bounceForcePercentage = 1.2f;
	[SerializeField] private int bounceTimes = 3;

	[Header("Pierce Info")]
	[SerializeField] private UISkillTreeSlotController unlockPierceSwordButton;
	[Range(0f, 5f)]
	[SerializeField] private float pierceForcePercentage = 3.5f;
	[SerializeField] private int pireceTimes = 2;

	[Header("Spin Info")]
	[SerializeField] private UISkillTreeSlotController unlockSpinSwordButton;
	[Range(0f, 5f)]
	[SerializeField] private float spinForcePercentage = 2f;
	[SerializeField] private float maxMoveDistance = 8;
	[SerializeField] private float spinningDamageTime = 0.2f;

	[Header("Aim Info")]
	[SerializeField] private GameObject dotObject;
	[SerializeField] private int dotsNum = 20;
	private List<GameObject> dots;

	[Header("Passive Skill Info")]
	[SerializeField] private UISkillTreeSlotController unlockSwordButton;
	public bool canTimeStop;
	[SerializeField] private UISkillTreeSlotController unlockTimeStopButton;
	public bool canVulnerability;
	[SerializeField] private UISkillTreeSlotController unlockVulnerabilityButton;


	public override bool CanUseSkill()
	{
		if (!Unlocked) return false;
		return player.sword == null || !player.sword.activeSelf;
	}

	public override void UseSkill()
	{
		if (CanUseSkill())
		{
			base.UseSkill();
			player.sword = player.sword == null ? GameObject.Instantiate(swordObject) : player.sword;
			ChangeSwordProperties(swordType);
			player.sword.GetComponent<SwordController>().SetupSword(CaculateAimDirection(), player.transform.position, currentThrowForce, swordType, canTimeStop, canVulnerability);
		}
	}

	private Vector2 CaculateAimDirection()
	{
		Vector2 launchPosition = player.transform.position;
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		return mousePosition - launchPosition;
	}

	protected override void Awake()
	{
		base.Awake();
		if (unlockSwordButton != null)
		{
			var buttonController = unlockSwordButton.GetComponent<UISkillTreeSlotController>();
			void UnlockSword()
			{
				Unlocked = unlockSwordButton.IsUnlocked();
				if (Unlocked) swordType = SwordType.Regular;
			}
			buttonController.OnUnlockedChanged += UnlockSword;
		}
		if (unlockTimeStopButton != null)
		{
			var buttonController = unlockTimeStopButton.GetComponent<UISkillTreeSlotController>();
			void UnlockTimeStop()
			{
				canTimeStop = unlockTimeStopButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockTimeStop;
		}
		if (unlockVulnerabilityButton != null)
		{
			var buttonController = unlockVulnerabilityButton.GetComponent<UISkillTreeSlotController>();
			void UnlockVulnerability()
			{
				canVulnerability = unlockVulnerabilityButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockVulnerability;
		}
		if (unlockBouncySwordButton != null)
		{
			var buttonController = unlockBouncySwordButton.GetComponent<UISkillTreeSlotController>();
			void UnlockBouncySword()
			{
				if (buttonController.IsUnlocked()) swordType = SwordType.Bounce;
			}
			buttonController.OnUnlockedChanged += UnlockBouncySword;
		}
		if (unlockPierceSwordButton != null)
		{
			var buttonController = unlockPierceSwordButton.GetComponent<UISkillTreeSlotController>();
			void UnlockPierceSword()
			{
				if (buttonController.IsUnlocked()) swordType = SwordType.Pierce;
			}
			buttonController.OnUnlockedChanged += UnlockPierceSword;
		}
		if (unlockSpinSwordButton != null)
		{
			var buttonController = unlockSpinSwordButton.GetComponent<UISkillTreeSlotController>();
			void UnlockSpinSword()
			{
				if (buttonController.IsUnlocked()) swordType = SwordType.Spin;
			}
			buttonController.OnUnlockedChanged += UnlockSpinSword;
		}
	}

	protected override void Start()
	{
		base.Start();
		GenerateDot();
		currentThrowForce = throwForce;
	}

	protected override void Update()
	{
		base.Update();
	}


	private Vector2 CaculateThrowDestination(Vector2 _throwVelovity, Vector2 _throwPosition, float _time)
	{
		return new Vector2(_throwPosition.x + _throwVelovity.x * _time, _throwPosition.y + _throwVelovity.y * _time + 0.5f * Physics2D.gravity.y * _time * _time);
	}

	private void GenerateDot()
	{
		dots = new List<GameObject>();
		for (int i = 0; i < dotsNum; i++)
		{
			dots.Add(GameObject.Instantiate(dotObject, player.attackCheck.position, Quaternion.identity));
			dots[i].SetActive(false);
		}
	}

	public void NeedAimLine(bool _needFlag)
	{
		ChangeSwordProperties(swordType);
		Vector2 throwVelocity = CaculateAimDirection().normalized * currentThrowForce;
		float projectileMostionVerticalVecolity = -Mathf.Sqrt((throwVelocity).y * (throwVelocity).y + 2 * Physics2D.gravity.y * -CameraManager.instance.mainCamera.orthographicSize);
		float projectileMotionTime = (0 - throwVelocity.y) / Physics2D.gravity.y + (projectileMostionVerticalVecolity - 0) / Physics2D.gravity.y;
		float timeBetween = projectileMotionTime / dotsNum;
		for (int i = 0; i < dotsNum; i++)
		{
			if (_needFlag)
			{
				dots[i].transform.position = CaculateThrowDestination(throwVelocity, player.transform.position, i * timeBetween);
			}
			dots[i].SetActive(_needFlag);
		}
	}

	public void ChangeSwordProperties(SwordType _swordType)
	{

		switch (_swordType)
		{
			case SwordType.Bounce:
				{
					player.sword?.GetComponent<SwordController>().SetupBounce(bounceForcePercentage, bounceTimes);
					currentThrowForce = throwForce * bounceForcePercentage;
					break;
				}
			case SwordType.Pierce:
				{
					player.sword?.GetComponent<SwordController>().SetupPierce(pierceForcePercentage, pireceTimes);
					currentThrowForce = throwForce * pierceForcePercentage;
					break;
				}
			case SwordType.Spin:
				{
					player.sword?.GetComponent<SwordController>().SetupSpin(spinForcePercentage, maxMoveDistance, spinningDamageTime);
					currentThrowForce = throwForce * spinForcePercentage;
					break;
				}
			default:
				{
					player.sword?.GetComponent<SwordController>().SetupRegular();
					currentThrowForce = throwForce;
					break;
				}
		}
	}


	public void ReturnToPlayer()
	{
		player.sword?.GetComponent<SwordController>().NeedReturn(currentThrowForce);
	}

}
