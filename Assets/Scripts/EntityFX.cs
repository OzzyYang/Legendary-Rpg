using System;
using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
	[SerializeField] private Material hitMaterial;
	[SerializeField] private float hitFlashDuration;

	[SerializeField] private Color[] ignitedColor;
	[SerializeField] private Color[] frozenColor;
	[SerializeField] private Color[] shockedColor;

	[SerializeField] private Color[] hittedColor;

	private SpriteRenderer sR;
	private Material originMaterial;
	private CharacterStats characterStats;


	// Start is called before the first frame update
	void Start()
	{
		sR = GetComponent<SpriteRenderer>();
		originMaterial = sR.material;
		characterStats = GetComponentInParent<CharacterStats>();
		hittedColor = new Color[2] { Color.white, Color.red };
		ignitedColor = new Color[2] { new Color(1, 0.2f, 0), new Color(1, 0.5f, 0.4f) };
		frozenColor = new Color[2] { new Color(0, 0.4f, 1), new Color(0.5f, 0.7f, 1) };
		shockedColor = new Color[2] { new Color(1, 1, 0), new Color(1, 0.8f, 0) };
	}

	// Update is called once per frame
	void Update()
	{

	}

	public IEnumerator FlashFX()
	{
		sR.material = hitMaterial;
		yield return new WaitForSeconds(hitFlashDuration);
		sR.material = originMaterial;
	}



	public void RedColorBlink()
	{
		if (sR.color == Color.white)
		{
			sR.color = Color.red;
		}
		else
		{
			sR.color = Color.white;
		}
	}

	public void IgnitedColorBlink()
	{
		if (characterStats.isFireIgnited)
		{
			if (sR.color == ignitedColor[0])
			{
				sR.color = ignitedColor[1];
			}
			else
			{
				sR.color = ignitedColor[0];
			}

		}
	}

	public void ShockedColorBlink()
	{
		if (characterStats.isElectricShocked)
		{
			if (sR.color == shockedColor[0])
			{
				sR.color = shockedColor[1];
			}
			else
			{
				sR.color = shockedColor[0];
			}

		}
	}

	public void FrozenColorBlink()
	{
		if (characterStats.isIceFrozen)
		{
			if (sR.color == frozenColor[0])
			{
				sR.color = frozenColor[1];
			}
			else
			{
				sR.color = frozenColor[0];
			}

		}
	}
	public void CancelColorBlink(System.Action action)
	{
		if (action is null)
		{
			throw new ArgumentNullException(nameof(action));
		}

		CancelInvoke(action.Method.Name);
		sR.color = Color.white;
	}
}
