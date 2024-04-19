using UnityEngine;

public class EntityAnimationEventTriggers : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private void LightingAttackTrigger()
	{
		GetComponentInParent<LightningController>().TakeDamage();
	}

	private void AnimationFinishTrigger()
	{
		GetComponentInParent<LightningController>().DestroySelf();
	}
}
