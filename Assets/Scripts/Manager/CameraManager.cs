using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public static CameraManager instance;
	public Camera mainCamera;
	public CinemachineVirtualCamera virtualCam1;
	public GameObject cameraCollider;
	public GameObject player;
	private Vector2 originalPosition;

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;
	}

	private void Start()
	{
		originalPosition = cameraCollider.transform.position;
	}

	//private void LastUpdate()
	//{
	//	cameraCollider.transform.position = new Vector2(player.transform.position.x, originalPosition.y);
	//}

	private void LateUpdate()
	{
		cameraCollider.transform.position = new Vector2(player.transform.position.x, originalPosition.y);
	}
}
