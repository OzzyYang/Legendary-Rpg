using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBarController : HealthBarController
{
	[SerializeField] private PlayerManager playerManager;
	private TextMeshProUGUI healthText;

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void FlipUi()
	{
		// the ingame ui of health bar don't need to flip 
	}

	protected override void Start()
	{
		this.character = playerManager.player;
		this.characterStats = this.character.GetComponent<CharacterStats>();
		this.slider = GetComponentInChildren<Slider>();
		this.healthText = GetComponentInChildren<TextMeshProUGUI>();
		base.Start();

	}

	protected override void Update()
	{
		base.Update();
	}

	protected override void UpdateHealthValue()
	{
		base.UpdateHealthValue();
		this.healthText.text = this.characterStats.currentHealth.ToString() + "/" + this.characterStats.maxHealth.GetValue().ToString();
	}
}
