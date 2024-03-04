using UnityEngine;

public class PlayerAnimationTrrigers : MonoBehaviour
{
	private PlayerController player => GetComponentInParent<PlayerController>();

	private void AnimationTrigger()
	{
		player.AnimationTrrier();
	}
}
