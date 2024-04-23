using TMPro;
using UnityEngine;

public class UIToolTipController : MonoBehaviour
{
	[SerializeField] protected TextMeshProUGUI title;
	[SerializeField] protected TextMeshProUGUI type;
	[SerializeField] protected TextMeshProUGUI content;

	protected virtual void Update()
	{

		UpdatePositionByMouse();
	}

	protected virtual void UpdatePositionByMouse()
	{
		float xOffset, yOffset;
		xOffset = 20;
		yOffset = -20;
		Vector3 positionInCanvas = Input.mousePosition + new Vector3(xOffset, yOffset);
		Vector2 sizeInCanvas = GetComponent<RectTransform>().sizeDelta;
		Vector2 parentSizeInCanvas = transform.parent.GetComponent<RectTransform>().sizeDelta;
		if ((positionInCanvas.x + sizeInCanvas.x) > parentSizeInCanvas.x) xOffset = -xOffset - sizeInCanvas.x;
		//The mouse coordinate is (x, 0) when it is located at the bottom of the screen.
		if ((positionInCanvas.y - sizeInCanvas.y) < 0) yOffset = -yOffset + sizeInCanvas.y;
		transform.position = Input.mousePosition + new Vector3(xOffset, yOffset);
	}

	public virtual void Show() => this.Show("", "", "");
	public virtual void Show(string title, string content) => this.Show(title, "", content);

	public virtual void Show(string title, string type, string content)
	{
		if (this.title != null) this.title.text = title;
		if (this.content != null) this.content.text = content;
		if (this.type != null) this.type.text = type;

		//Fix bug: ensure the tool tip will not blink
		UpdatePositionByMouse();
		gameObject.SetActive(true);
	}

	public virtual void Hide()
	{
		gameObject.SetActive(false);
	}
}
