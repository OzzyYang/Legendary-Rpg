using UnityEngine;

public class MirageSkill : Skill
{
	[SerializeField] private GameObject playerCloneObject;
	[SerializeField] private GameObject crystalObject;
	[SerializeField] private float cloneDuration;

	[SerializeField] private bool canDuplicate;
	[Range(0f, 1f)]
	[SerializeField] private float duplicateProbability;

	[SerializeField] private bool createCrystalInsteadClone;
	[SerializeField] private int ultimateSkillCreateNum;
	private int currentUltimateSkillCreateNum;
	private int createTimes = 1;

	[Header("Unlock Info")]
	public float damageMultiplier;
	[SerializeField] private UISkillTreeSlotController unlockCreateCloneButton;
	public bool isCloneAggresive;
	public bool applyWeaponEffect;
	[SerializeField] private UISkillTreeSlotController unlockAggresiveCloneButton;
	[SerializeField] private UISkillTreeSlotController unlockCrystalMirageButton;
	[SerializeField] private UISkillTreeSlotController unlockMutipleMirageButton;

	public void UseSkill(Transform _newTransform, Vector3 _offSet)
	{
		if (CanUseSkill())
		{
			UseSkill();
			CreateClone(_newTransform, _offSet);
		}
	}

	private void CreateClone(Transform _newTransform, Vector3 _offSet)
	{
		if (createCrystalInsteadClone)
		{
			currentUltimateSkillCreateNum += ultimateSkillCreateNum;
			InvokeRepeating(nameof(CreateCrystal), 0, 0.1f);
		}
		else
		{
			GameObject newClone = Instantiate(playerCloneObject);
			newClone.GetComponent<PlayerCloneController>().SetUpClone(_newTransform, _offSet, cloneDuration, canDuplicate, duplicateProbability, damageMultiplier, applyWeaponEffect);
		}

	}

	private void CreateCrystal()
	{
		GameObject newClone = Instantiate(crystalObject, player.transform.position, Quaternion.identity);
		newClone.GetComponent<CrystalController>().SetupCrystal(player.skill.CrystalSkill.GetCrystalDuration(), true, true, player.skill.BlackHoleSkill.IsReleasingSkill ? 1 : 0, null);
		createTimes++;
		if (createTimes > (player.skill.BlackHoleSkill.IsReleasingSkill ? currentUltimateSkillCreateNum : 1))
		{
			CancelInvoke(nameof(CreateCrystal));
			createTimes = 1;
			currentUltimateSkillCreateNum = 0;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		if (unlockCreateCloneButton != null)
		{
			var buttonController = unlockCreateCloneButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCreateCloneSkill()
			{
				Unlocked = unlockCreateCloneButton.IsUnlocked();
				damageMultiplier = 0.3f;
			}
			buttonController.OnUnlockedChanged += UnlockCreateCloneSkill;
		}
		if (unlockAggresiveCloneButton != null)
		{
			var buttonController = unlockAggresiveCloneButton.GetComponent<UISkillTreeSlotController>();
			void UnlockAggresiveClone()
			{
				isCloneAggresive = unlockAggresiveCloneButton.IsUnlocked();
				damageMultiplier = 0.8f;
				applyWeaponEffect = true;
			}
			buttonController.OnUnlockedChanged += UnlockAggresiveClone;
		}
		if (unlockCrystalMirageButton != null)
		{
			var buttonController = unlockCrystalMirageButton.GetComponent<UISkillTreeSlotController>();
			void UnlockCrystalMirage()
			{
				createCrystalInsteadClone = unlockCrystalMirageButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockCrystalMirage;
		}
		if (unlockMutipleMirageButton != null)
		{
			var buttonController = unlockMutipleMirageButton.GetComponent<UISkillTreeSlotController>();
			void UnlockMutipleMirage()
			{
				canDuplicate = unlockMutipleMirageButton.IsUnlocked();
			}
			buttonController.OnUnlockedChanged += UnlockMutipleMirage;
		}
	}

	protected override void Start()
	{
		base.Start();
	}

}
