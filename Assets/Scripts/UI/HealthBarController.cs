using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
	private CharacterController character;
	private CharacterStats characterStats;

	private Slider slider;
	// Start is called before the first frame update
	void Start()
	{
		character = GetComponentInParent<CharacterController>();
		characterStats = GetComponentInParent<CharacterStats>();
		slider = GetComponentInChildren<Canvas>().GetComponentInChildren<Slider>();
		UpdateHealthValue();
		character.onFlipped += FlipUi;
		characterStats.OnCurrentHealthChanged += UpdateHealthValue;
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void FlipUi()
	{
		transform.Rotate(0, 180, 0);
	}

	private void UpdateHealthValue()
	{

		slider.maxValue = characterStats.maxHealth.GetValue();
		slider.value = characterStats.currentHealth;
	}

	private void OnDisable()
	{
		character.onFlipped -= FlipUi;

	}

}
