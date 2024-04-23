using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum StatType
{
	Strength,
	Agility,
	Intelligence,
	Vitality,
	Damage,
	CriticalRate,
	CriticalMultiplier,
	MaxHealth,
	EvasionRate,
	Armor,
	MagicResistance,
	FireDamage,
	FrostDamage,
	LightningDamge
}

public class CharacterStats : MonoBehaviour
{
	#region Stats Info
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
	#endregion

	#region Buff Info
	public bool isVulnerable { get; private set; }
	private float vulnerableMultiplier = 0;
	private float damagaMultiplier = 0;
	#endregion

	public CharacterController character { get; protected set; }
	private EntityVFX fX;
	public Action OnCurrentHealthChanged { get; set; }
	public Action OnBaseEvasionRateChanged { get; set; }
	public Action OnAvoidAttack { get; set; }

	[SerializeField] private GameObject lightningPrefab;
	[SerializeField] private LayerMask whatIsGround;

	protected virtual void Start()
	{
		currentHealth = maxHealth.GetValue();
		maxHealth.OnStatValueChanged += ClampHealth;
		criticalMultiplier.SetDefaultValue(150);

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
		ReduceHealth(ignitedDamge, "fire");
		//Debug.Log("Burned");
		if (this.currentHealth <= 0) Die();
	}

	public virtual void DoDamageWithMultiplier(CharacterStats _target, float multiplier)
	{
		this.damagaMultiplier = multiplier;
		this.DoDamage(_target);
		this.damagaMultiplier = 0;
	}
	public virtual void DoDamage(CharacterStats _target)
	{
		if (_target == null || CanAvoidAttack(_target)) return;

		float totalDamge = this.damage.GetValue() + this.strength.GetValue();

		if (CanCriticalStrike()) totalDamge = CaculateCriticalDamge(totalDamge);

		totalDamge += (totalDamge * damagaMultiplier + totalDamge * _target.vulnerableMultiplier);

		totalDamge = CheckArmorThenDamage(totalDamge, _target);

		//this.DoMagicalDamage(_target);

		_target.ReduceHealth(totalDamge, this.name);
		_target.GetComponent<CharacterController>().playDamageEffect();
	}

	public virtual void DoMagicalDamage(CharacterStats _target)
	{
		float totalMagicalDamge = CaculateMagicalDamge(_target);
		_target.ReduceHealth(totalMagicalDamge, this.name);
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
		if (Random.Range(0, 99) < totalEvasion)
		{
			//this.OnAvoidAttack?.Invoke();
			//fix bug:the target is the one who will take damage, so the subject of avoidance attack is target !!!!! 
			//took me 30 minutes to fix it,  can't beleive it >o< !!!!!!
			_target.OnAvoidAttack?.Invoke();
			return true;
		}
		else
			return false;
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


	public virtual float IncreseHealth(float healthToIncrese, string increaseReason)
	{
		if (healthToIncrese < 0) throw new ArgumentOutOfRangeException("Damage must be Non-negative!");
		if (currentHealth == maxHealth.GetValue()) return currentHealth;
		Debug.Log(character.name + "'s current health been increased " + healthToIncrese + " points because of " + increaseReason);
		healthToIncrese = Mathf.RoundToInt(healthToIncrese);
		healthToIncrese = Mathf.Clamp(healthToIncrese, 0, maxHealth.GetValue() - currentHealth);
		currentHealth += healthToIncrese;
		OnCurrentHealthChanged?.Invoke();
		return currentHealth;
	}
	public virtual float ReduceHealth(float _damage, string attackerName)
	{
		if (_damage < 0) throw new ArgumentOutOfRangeException("Damage must be Non-negative!");
		Debug.Log(character.name + " has been damaged " + _damage + " points by " + attackerName);
		currentHealth -= _damage;
		OnCurrentHealthChanged?.Invoke();
		if (currentHealth <= 0) Die();
		return currentHealth;
	}

	private void ClampHealth()
	{
		if (currentHealth > maxHealth.GetValue())
		{
			currentHealth = maxHealth.GetValue();
			OnCurrentHealthChanged?.Invoke();
		}
	}
	protected virtual void Die()
	{
		character.BeDead();
		character.GetComponent<ItemDropController>().DropItem();
	}


	private struct CoroutineParams
	{
		public float seconds;
		public float multiplier;
	}

	public void SetVulnerableFor(float seconds, float multiplier)
	{
		StopCoroutine(nameof(IESetVulnerableFor));
		this.StartCoroutine(nameof(IESetVulnerableFor), new CoroutineParams
		{
			seconds = seconds,
			multiplier = multiplier
		});
	}

	private IEnumerator IESetVulnerableFor(CoroutineParams parameters)
	{
		SetVulnerable(true, parameters.multiplier);
		yield return new WaitForSeconds(parameters.seconds);
		SetVulnerable(false, parameters.multiplier);
	}

	public void SetVulnerable(bool setBool, float multiplier)
	{
		this.isVulnerable = setBool;
		this.vulnerableMultiplier = this.isVulnerable ? multiplier : 0;
	}
	public Stat GetStatByType(StatType statType)
	{
		switch (statType)
		{
			case StatType.Strength: return strength;
			case StatType.Agility: return agility;
			case StatType.Intelligence: return intelligence;
			case StatType.Vitality: return vitality;
			case StatType.Damage: return damage;
			case StatType.CriticalRate: return criticalRate;
			case StatType.CriticalMultiplier: return criticalMultiplier;
			case StatType.MaxHealth: return maxHealth;
			case StatType.EvasionRate: return evasionRate;
			case StatType.Armor: return armor;
			case StatType.MagicResistance: return magicResistance;
			case StatType.FireDamage: return fireDamage;
			case StatType.FrostDamage: return frostDamage;
			case StatType.LightningDamge: return lightningDamge;
			default: return null;
		}
	}

	public string GetStatDescriptionByType(StatType statType)
	{

		//var result = new StringBuilder();

		return statType switch
		{
			StatType.Strength or StatType.Agility or StatType.Intelligence or StatType.Vitality or StatType.Damage or StatType.FireDamage or StatType.FrostDamage or StatType.LightningDamge or StatType.Armor or StatType.MagicResistance or StatType.MaxHealth => $"",
			_ => ""
		};


	}
}
