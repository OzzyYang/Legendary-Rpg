using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Effect Data", menuName = "Data/Equipment Effect/Thunder Effect")]
public class ThunderStrikeEffect : ItemEffectData
{
	public GameObject thunderStrikePrefab;
	public float damage;
	public override void Effect(Transform target)
	{
		GameObject newThunderStrike = Instantiate(thunderStrikePrefab, target.position, Quaternion.identity);
		newThunderStrike.GetComponent<LightningController>().SetupDamage(damage);
	}
}
