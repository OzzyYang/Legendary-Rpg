using UnityEngine;

public class CloneSkill : Skill
{
	[SerializeField] private GameObject cloneObject;
	[SerializeField] private float cloneDuration;
	public override bool CanUseSkill()
	{
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{
		base.UseSkill();
	}

	public void CreateClone(Transform _newTransform, Vector3 _offSet)
	{
		GameObject newClone = GameObject.Instantiate(cloneObject);
		newClone.GetComponent<CloneObjectController>().SetUpClone(_newTransform, _offSet, cloneDuration);
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void Update()
	{
		base.Update();
	}

}
