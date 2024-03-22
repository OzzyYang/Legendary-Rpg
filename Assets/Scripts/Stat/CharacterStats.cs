using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
	[Header("Major Stats")]
	public Stat strength;// 1 point increase damage by 1 and critical multiplier by 1%
	public Stat agility;// 1 point increase evasion by 1% and critical rate by 1%
	public Stat intelligence;// 1 point increase magic damage by 1 and magic resistance by 3
	public Stat vitality;// 1 point increase health by 3 or 5 points

	[Header("Offensive Stats")]
	public Stat damage;
	public Stat criticalRate;
	public Stat criticalMultiplier;

	[Header("Defensive Stats")]
	public Stat maxHealth;
	public Stat evasionRate;
	public Stat armor;
	public Stat magicResistance;

	[Header("Magic Stats")]
	public Stat fireDamage;
	public Stat frostDamage;
	public Stat lightningDamge;

	public bool isFireIgnited { get; protected set; }
	public bool isIceFrozen { get; protected set; }
	public bool isElectricShocked { get; protected set; }

	[SerializeField] private float currentHealth;

	public CharacterController character { get; protected set; }

	protected virtual void Start()
	{
		this.currentHealth = this.maxHealth.GetValue();
		this.criticalMultiplier.SetDefaultValue(150);
		character = GetComponent<CharacterController>();
	}

	public virtual void DoDamage(CharacterStats _target)
	{
		if (CanAvoidAttack(_target)) return;

		float totalDamge = this.damage.GetValue() + this.strength.GetValue();

		if (CanCriticalStrike()) totalDamge = CaculateCriticalDamge(totalDamge);

		totalDamge = CheckArmorThenDamage(totalDamge, _target);

		float totalMagicalDamge = CaculateMagicalDamge(_target);
		_target.TakeDamage(totalMagicalDamge);
		_target.GetComponent<CharacterController>().playDamageEffect();
	}

	private float CheckArmorThenDamage(float _baseDamage, CharacterStats _target)
	{
		//ensure that the damage is greater than zero
		return Mathf.Clamp(_baseDamage - _target.armor.GetValue(), 0, float.MaxValue);
	}

	private bool CanAvoidAttack(CharacterStats _target)
	{
		float totalEvasion = _target.evasionRate.GetValue() + _target.agility.GetValue();

		//ensure that the evasion rate is between 0% and 80%
		totalEvasion = Mathf.Clamp(totalEvasion, 0, 80);
		return Random.Range(0, 99) < totalEvasion;
	}

	private bool CanCriticalStrike()
	{
		float totalCritRate = this.criticalRate.GetValue() + this.agility.GetValue();

		//ensure that the crite rate is between 0% and 40%
		totalCritRate = Mathf.Clamp(totalCritRate, 0, 40);
		return Random.Range(0, 99) < totalCritRate;
	}

	private float CaculateCriticalDamge(float _baseDamage)
	{
		float totalCriticalMultiplier = (this.criticalMultiplier.GetValue() + this.strength.GetValue()) * 0.01f;
		return Mathf.RoundToInt(_baseDamage * totalCriticalMultiplier);
	}

	private float CaculateMagicalDamge(CharacterStats _target)
	{
		float totalMagicalDamage = this.fireDamage.GetValue() + this.frostDamage.GetValue() + this.lightningDamge.GetValue() + this.intelligence.GetValue();

		totalMagicalDamage -= _target.magicResistance.GetValue() + _target.intelligence.GetValue() * 3;
		totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, float.MaxValue);

		//bool _isFireIgnited = (fireDamage.GetValue() > frostDamage.GetValue()) && (fireDamage.GetValue() > lightningDamge.GetValue());
		//bool _isIceFrozen = (frostDamage.GetValue() > fireDamage.GetValue()) && (frostDamage.GetValue() > lightningDamge.GetValue());
		//bool _isElectricShocked = (lightningDamge.GetValue() > fireDamage.GetValue()) && (lightningDamge.GetValue() > frostDamage.GetValue());

		//if (!_isElectricShocked && !_isFireIgnited && !_isIceFrozen)
		//{
		//	switch (Random.Range(0, 3))
		//	{
		//		case 0:
		//			{
		//				_isElectricShocked = true;
		//				break;
		//			}
		//		case 1:
		//			{
		//				_isIceFrozen = true;
		//				break;
		//			}
		//		case 2:
		//			{
		//				_isFireIgnited = true;
		//				break;
		//			}
		//		default:
		//			{
		//				break;
		//			}
		//	}
		//}

		SortedDictionary<string, float> magicalDamageList = new SortedDictionary<string, float>();

		magicalDamageList.Add("isFireIgnited", fireDamage.GetValue());
		magicalDamageList.Add("isIceFrozen", frostDamage.GetValue());
		magicalDamageList.Add("isElectricShocked", lightningDamge.GetValue());

		foreach (var item in magicalDamageList)
		{
			Debug.Log(item);
		}

		//this.ApplyAilments(_isFireIgnited, _isIceFrozen, _isElectricShocked);

		return totalMagicalDamage;
	}

	private void ApplyAilments(bool _isFireIgnited, bool _isIceFrozen, bool _isElectricShocked)
	{
		//if (!_isFireIgnited || !_isIceFrozen || !_isElectricShocked) return;

		isFireIgnited = _isFireIgnited;
		isIceFrozen = _isIceFrozen;
		isElectricShocked = _isElectricShocked;
		Debug.Log(isFireIgnited + " " + isIceFrozen + " " + isElectricShocked);
	}

	public virtual float TakeDamage(float _damage)
	{
		Debug.Log(_damage);
		this.currentHealth -= _damage;
		if (currentHealth <= 0) Die();
		return this.currentHealth;
	}

	protected virtual void Die() => character.BeDead();

	protected virtual void Update()
	{

	}

}
