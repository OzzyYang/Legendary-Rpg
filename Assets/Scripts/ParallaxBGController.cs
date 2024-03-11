using UnityEngine;

public class ParallaxBGController : MonoBehaviour
{
	private GameObject cam;
	private float xPosStart;

	private float length;

	[SerializeField] private float parallaxEffect;
	// Start is called before the first frame update
	void Start()
	{
		cam = GameObject.Find("Main Camera");
		xPosStart = transform.position.x;

		length = GetComponent<SpriteRenderer>().bounds.size.x;
		//Debug.Log(xPosStart + " " + length);
	}

	// Update is called once per frame
	void Update()
	{

		float distanceToMove = cam.transform.position.x * parallaxEffect;

		transform.position = new Vector2(xPosStart + distanceToMove, transform.position.y);

		//���������ڴ˱���ͼ���ƶ��ľ���
		float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);

		if (distanceMoved > length + xPosStart)
		{
			Debug.Log(1);
			xPosStart += 2 * length;

		}
		else if (distanceMoved < xPosStart - length)
		{
			Debug.Log(2);
			xPosStart -= 2 * length;
		}
		//Debug.Log("Moved " + distanceMoved + " ||" + " Initial X " + xPosStart);
	}
}
