using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
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
	public bool canCreateClone;
	public float damageMultiplier;
	[SerializeField] private UISkillTreeSlotController unlockCreateCloneButton;
	public bool isCloneAggresive;
	public bool applyWeaponEffect;
	[SerializeField] private UISkillTreeSlotController unlockAggresiveCloneButton;
	[SerializeField] private UISkillTreeSlotController unlockCrystakMirageButton;
	[SerializeField] private UISkillTreeSlotController unlockMutilpleMirageButton;

	public override bool CanUseSkill()
	{
		return base.CanUseSkill() && canCreateClone;
	}

	public override void UseSkill()
	{
		base.UseSkill();
	}

	public void UseSkill(Transform _newTransform, Vector3 _offSet)
	{
		if (this.CanUseSkill())
		{
			this.UseSkill();
			this.CreateClone(_newTransform, _offSet);
		}
	}

	private void CreateClone(Transform _newTransform, Vector3 _offSet)
	{
		if (createCrystalInsteadClone)
		{
			this.currentUltimateSkillCreateNum += ultimateSkillCreateNum;
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
		Debug.Log(this.createTimes);
		GameObject newClone = Instantiate(crystalObject, player.transform.position, Quaternion.identity);
		newClone.GetComponent<CrystalController>().SetupCrystal(player.skill.crystalSkill.getCrystalDuration(), true, true, player.skill.blackHoleSkill.isReleasingSkill ? 1 : 0, null);
		this.createTimes++;
		if (this.createTimes > (player.skill.blackHoleSkill.isReleasingSkill ? currentUltimateSkillCreateNum : 1))
		{
			CancelInvoke(nameof(CreateCrystal));
			this.createTimes = 1;
			this.currentUltimateSkillCreateNum = 0;
		}
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		if (this.unlockCreateCloneButton != null)
		{
			this.unlockCreateCloneButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canCreateClone = this.unlockCreateCloneButton.IsUnlocked();
				this.damageMultiplier = 0.3f;
			});
		}
		if (this.unlockAggresiveCloneButton != null)
		{
			this.unlockAggresiveCloneButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.isCloneAggresive = this.unlockAggresiveCloneButton.IsUnlocked();
				this.damageMultiplier = 0.8f;
				this.applyWeaponEffect = true;
			});
		}
		if (this.unlockCrystakMirageButton != null)
		{
			this.unlockCrystakMirageButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.createCrystalInsteadClone = this.unlockCrystakMirageButton.IsUnlocked();
			});
		}
		if (this.unlockMutilpleMirageButton != null)
		{
			this.unlockMutilpleMirageButton.GetComponent<Button>().onClick.AddListener(() =>
			{
				this.canDuplicate = this.unlockMutilpleMirageButton.IsUnlocked();
			});
		}
	}

	protected override void Update()
	{
		base.Update();

	}

}
