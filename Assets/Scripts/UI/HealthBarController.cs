using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
	[SerializeField] protected CharacterController character;
	[SerializeField] protected CharacterStats characterStats;

	[SerializeField] protected Slider slider;

	protected virtual void Awake()
	{
	}

	// Start is called before the first frame update
	protected virtual void Start()
	{
		UpdateHealthValue();
		character.onFlipped += FlipUi;
		characterStats.OnCurrentHealthChanged += UpdateHealthValue;
		characterStats.maxHealth.OnStatValueChanged += UpdateHealthValue;
	}

	// Update is called once per frame
	protected virtual void Update()
	{

	}

	protected virtual void FlipUi()
	{
		transform.Rotate(0, 180, 0);
	}

	protected virtual void UpdateHealthValue()
	{

		slider.maxValue = characterStats.maxHealth.GetValue();
		slider.value = characterStats.currentHealth;
	}

	private void OnDisable()
	{
		character.onFlipped -= FlipUi;
	}

}
