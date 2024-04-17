using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
	[SerializeField] private float baseValue;
	[SerializeField] private List<float> modifiers;

	public Action OnStatValueChanged { get; set; }

	public float GetValue() => this.baseValue + this.modifiers.Sum();

	public void AddModifier(float _modifier)
	{
		this.modifiers.Add(_modifier);
		this.OnStatValueChanged?.Invoke();
	}
	public void RemoveModifier(float _modifier)
	{
		this.modifiers.Remove(_modifier);
		this.OnStatValueChanged?.Invoke();
	}

	public void SetDefaultValue(float _value)
	{
		this.baseValue = _value;
		this.OnStatValueChanged?.Invoke();
	}
}
