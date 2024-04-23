using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject uiParent;
	[SerializeField] private GameObject menuPages;
	[SerializeField] private UIInGameUIController inGameUI;
	public static UIManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this.GetComponent<UIManager>();
		}
		else
		{
			Destroy(gameObject);
		}
		menuPages.SetActive(false);
	}

	public void SwitchToPage(GameObject pageToSwitch)
	{
		for (int i = 0; i < menuPages.transform.childCount; i++)
		{
			menuPages.transform.GetChild(i).gameObject.SetActive(false);
		}

		pageToSwitch?.SetActive(true);
	}

	public UIMenuPageController GetMenuPageController() => this.menuPages.GetComponent<UIMenuPageController>();

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			menuPages.SetActive(!menuPages.activeSelf);
			inGameUI.gameObject.SetActive(!menuPages.activeSelf);
		}
	}

}
