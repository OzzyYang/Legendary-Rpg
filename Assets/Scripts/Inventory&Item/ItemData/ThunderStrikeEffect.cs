using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/Thunder Strike Effect")]
public class ThunderStrikeEffect : ItemEffectData
{
	public GameObject thunderStrikePrefab;
	public float damage;
	public override void NegativeEffect(Transform target)
	{
		GameObject newThunderStrike = Instantiate(thunderStrikePrefab, target.position, Quaternion.identity);
		newThunderStrike.GetComponent<LightningController>().SetupDamage(damage);
	}
}
