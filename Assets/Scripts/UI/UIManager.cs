using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject menuPages;
	[SerializeField] private GameObject uiParent;
	public static UIManager instance;
	private void Start()
	{
		if (instance == null)
		{
			instance = this.GetComponent<UIManager>();
		}
		else
		{
			Destroy(this.gameObject);
		}
		this.menuPages.SetActive(false);
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
		}
	}

}
