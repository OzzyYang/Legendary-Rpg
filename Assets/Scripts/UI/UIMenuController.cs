using UnityEngine;

public class UIMenuController : MonoBehaviour
{
	[SerializeField] private GameObject menuPages;
	[SerializeField] private GameObject uiParent;
	public static GameObject instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this.gameObject;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	public void SwitchToPage(GameObject pageToSwitch)
	{
		//foreach (var page in menuPages.GetComponentsInChildren<UIMenuPageController>())
		//{
		//	page.gameObject.SetActive(false);
		//}
		for (int i = 0; i < menuPages.transform.childCount; i++)
		{
			menuPages.transform.GetChild(i).gameObject.SetActive(false);
		}
		pageToSwitch?.SetActive(true);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			uiParent.gameObject.SetActive(!uiParent.activeSelf);
		}
	}

}
