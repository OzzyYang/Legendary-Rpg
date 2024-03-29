using System.Collections.Generic;
using System.Linq;
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

	public bool isFireIgnited { get; protected set; } // being damaged over time
	public bool isIceFrozen { get; protected set; } //reduce armor by 50%
	public bool isElectricShocked { get; protected set; } // reduce accuracy by 50%

	private float ignitedTimer;
	private float ignitedDuration;
	private float ignitedDamge;

	private float frozenTimer;
	private float frozenDuration;

	private float electricShockedTimer;
	private float electricShockedDuration;
	public float currentHealth { get; protected set; }

	public CharacterController character { get; protected set; }
	private EntityVFX fX;
	public System.Action onHealthChanged { get; set; }

	[SerializeField] private GameObject lightningPrefab;
	[SerializeField] private LayerMask whatIsGround;

	protected virtual void Start()
	{
		this.currentHealth = this.maxHealth.GetValue();
		this.criticalMultiplier.SetDefaultValue(150);

		ignitedDuration = 4;
		frozenDuration = 4;
		electricShockedDuration = 4;

		character = GetComponent<CharacterController>();
		fX = GetComponentInChildren<EntityVFX>();
	}

	protected virtual void Update()
	{
		if (isFireIgnited)
		{
			if (ignitedTimer >= 0) ignitedTimer -= Time.deltaTime;
			else
			{
				CancelInvoke(nameof(DoDamageOnce));
				fX.CancelColorBlink(fX.IgnitedColorBlink);
				isFireIgnited = false;
			}
		}
		if (isIceFrozen)
		{
			if (frozenTimer >= 0) frozenTimer -= Time.deltaTime;
			else
			{
				fX.CancelColorBlink(fX.FrozenColorBlink);
				character.RevertSlow();
				isIceFrozen = false;
			}
		}
		if (isElectricShocked)
		{
			if (electricShockedTimer >= 0) electricShockedTimer -= Time.deltaTime;
			else
			{
				fX.CancelColorBlink(fX.ShockedColorBlink);
				isElectricShocked = false;
			}
		}
	}

	private void DoDamageOnce()
	{
		ReduceDamage(ignitedDamge, "fire");
		//Debug.Log("Burned");
		if (this.currentHealth <= 0) Die();
	}

	public virtual void DoDamage(CharacterStats _target)
	{
		if (CanAvoidAttack(_target)) return;

		float totalDamge = this.damage.GetValue() + this.strength.GetValue();

		if (CanCriticalStrike()) totalDamge = CaculateCriticalDamge(totalDamge);

		totalDamge = CheckArmorThenDamage(totalDamge, _target);

		//this.DoMagicalDamage(_target);

		_target.ReduceDamage(totalDamge, this.name);
		_target.GetComponent<CharacterController>().playDamageEffect();
	}

	public virtual void DoMagicalDamage(CharacterStats _target)
	{
		float totalMagicalDamge = CaculateMagicalDamge(_target);
		_target.ReduceDamage(totalMagicalDamge, this.name);
	}

	private float CheckArmorThenDamage(float _baseDamage, CharacterStats _target)
	{

		//ensure that the damage is greater than zero
		return Mathf.RoundToInt(Mathf.Clamp(_baseDamage - (_target.armor.GetValue() * (_target.isIceFrozen ? 0.5f : 1)), 0, float.MaxValue));
	}

	private bool CanAvoidAttack(CharacterStats _target)
	{
		float totalEvasion = _target.evasionRate.GetValue() + _target.agility.GetValue();
		totalEvasion = this.isElectricShocked ? totalEvasion + 50 : totalEvasion;

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

		CaculateAilementByDamage(_target);

		return totalMagicalDamage;
	}

	private void CaculateAilementByDamage(CharacterStats _target)
	{
		if (fireDamage.GetValue() <= 0 && lightningDamge.GetValue() <= 0 && frostDamage.GetValue() <= 0)
		{
			_target.ApplyAilments(false, false, false, this);
			return;
		}

		var sortedmagicalDamageList = new Dictionary<string, float>
		{
			{ "isFireIgnited", fireDamage.GetValue() },
			{ "isIceFrozen", frostDamage.GetValue() },
			{ "isElectricShocked", lightningDamge.GetValue() }
		}.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair
			=> pair.Value);


		var currentPair = new KeyValuePair<string, float>();
		var maxValueList = new List<KeyValuePair<string, float>>();
		foreach (var item in sortedmagicalDamageList)
		{
			if (currentPair.Key == null)
			{
				currentPair = item;
				maxValueList.Add(item);
				continue;
			}
			if (currentPair.Value == item.Value)
			{
				maxValueList.Add(item);
			}
			else
			{
				break;
			}
		}

		if (maxValueList.Count > 1)
		{
			currentPair = maxValueList[Random.Range(0, maxValueList.Count)];
		}


		bool _isFireIgnited = false, _isIceFrozen = false, _isElectricShocked = false;

		switch (currentPair.Key)
		{
			case "isFireIgnited":
				{
					_isFireIgnited = true;

					break;
				}
			case "isIceFrozen":
				{
					_isIceFrozen = true;
					break;
				}
			case "isElectricShocked":
				{
					_isElectricShocked = true;
					break;
				}

		}
		_target.ApplyAilments(_isFireIgnited, _isIceFrozen, _isElectricShocked, this);
	}

	private void ApplyAilments(bool _isFireIgnited, bool _isIceFrozen, bool _isElectricShocked, CharacterStats _attacker)
	{

		if (_isFireIgnited)
		{
			ignitedDamge = Mathf.RoundToInt(_attacker.fireDamage.GetValue() * 0.8f);
			ignitedTimer = ignitedDuration;
			if (!isFireIgnited)
			{
				InvokeRepeating(nameof(this.DoDamageOnce), 1, 1);
				fX.InvokeRepeating(nameof(fX.IgnitedColorBlink), 0, 0.2f);
			}
			isFireIgnited = _isFireIgnited;

		}

		if (_isIceFrozen)
		{
			frozenTimer = frozenDuration;
			if (!isIceFrozen)
			{
				fX.InvokeRepeating(nameof(fX.FrozenColorBlink), 0, 0.2f);
				character.SlowCharacter();
			}
			isIceFrozen = _isIceFrozen;
		}

		if (_isElectricShocked)
		{
			electricShockedTimer = electricShockedDuration;
			float yOffset = lightningPrefab.GetComponentInChildren<BoxCollider2D>().size.y - character.GetComponent<CapsuleCollider2D>().size.y;
			Instantiate(lightningPrefab, character.transform.position + new Vector3(0, yOffset, 0), Quaternion.identity).GetComponent<LightningController>().SetupDamage(_attacker.lightningDamge.GetValue());
			if (!isElectricShocked)
			{
				fX.InvokeRepeating(nameof(fX.ShockedColorBlink), 0, 0.2f);
			}
			isElectricShocked = _isElectricShocked;
		}


	}

	public virtual void TakeDamage(CharacterStats attacker)
	{
		attacker.DoDamage(this);
	}

	public virtual void TakeMagicalDamage(CharacterStats attack)
	{
		attack.DoMagicalDamage(this);
	}

	public virtual float ReduceDamage(float _damage, string attackerName)
	{
		Debug.Log(this.character.name + " has been damaged " + _damage + " points by " + attackerName);
		this.currentHealth -= _damage;
		if (this.onHealthChanged != null) onHealthChanged();
		if (currentHealth <= 0) Die();
		return this.currentHealth;
	}

	protected virtual void Die()
	{
		character.BeDead();
		character.GetComponent<ItemDropController>().DropItem();
	}


}
