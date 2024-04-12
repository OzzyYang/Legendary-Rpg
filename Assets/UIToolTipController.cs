using UnityEngine;

public class UIToolTipController : MonoBehaviour
{

	private void Update()
	{

		UpdatePositionByMouse();
	}

	private void UpdatePositionByMouse()
	{
		float xOffset, yOffset;
		xOffset = 20;
		yOffset = -20;
		Vector3 positionInCanvas = Input.mousePosition + new Vector3(xOffset, yOffset);
		Vector2 sizeInCanvas = this.GetComponent<RectTransform>().sizeDelta;
		Vector2 parentSizeInCanvas = this.transform.parent.GetComponent<RectTransform>().sizeDelta;
		if ((positionInCanvas.x + sizeInCanvas.x) > parentSizeInCanvas.x) xOffset = -xOffset - sizeInCanvas.x;
		//The mouse coordinate is (x, 0) when it is located at the bottom of the screen.
		if ((positionInCanvas.y - sizeInCanvas.y) < 0) yOffset = -yOffset + sizeInCanvas.y;
		this.transform.position = Input.mousePosition + new Vector3(xOffset, yOffset);
	}

	public void Show()
	{
		//Fix bug: ensure the tool tip will not blink
		this.UpdatePositionByMouse();
		this.gameObject.SetActive(true);
	}

	public void Hide()
	{
		this.gameObject.SetActive(false);
	}
}
