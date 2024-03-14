using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
	public SkeletonController enemy;
	// Start is called before the first frame update
	void Start()
	{
		enemy = GetComponentInParent<SkeletonController>();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void AnimationTrigger() => enemy.AnimationTriggerCalled();


	private void AttackTrigger() => enemy.AttackTriggerCalled();

	private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindown();

	private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindown();

}
