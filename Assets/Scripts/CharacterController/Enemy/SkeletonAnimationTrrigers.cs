using UnityEngine;

public class SkeletonAnimationTrrigers : MonoBehaviour
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

	public void AnimationTrigger()
	{
		enemy.TriggerCalled();
	}
}
