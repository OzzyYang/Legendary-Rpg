using System.Collections.Generic;
using UnityEngine;

public class UIMenuPageController : MonoBehaviour
{
	[Header("Character page")]
	[SerializeField] List<GameObject> statsSlotParentList;
	[SerializeField] GameObject character;
	// Start is called before the first frame update
	void Start()
	{
		this.character = PlayerManager.instance.player.gameObject;
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
	}

	// Update is called once per frame
	void Update()
	{
		UpdateStatsFrom(character.GetComponent<CharacterStats>());
	}

	public void UpdateStatsFrom(CharacterStats characterStats)
	{
		foreach (var stats in statsSlotParentList)
		{
			foreach (var stat in stats.GetComponentsInChildren<UIStatSlotController>())
			{
				stat.SetupSlotByCharacter(character.GetComponent<CharacterStats>());
			}
		}
	}

	public void SwitchToPage(GameObject pageToSwitch)
	{
		//foreach (var page in menuPages.GetComponentsInChildren<UIMenuPageController>())
		//{
		//	page.gameObject.SetActive(false);
		//}
		for (int i = 0; i < this.transform.childCount; i++)
		{
			this.transform.GetChild(i).gameObject.SetActive(false);
		}
		pageToSwitch?.SetActive(true);
	}
}
