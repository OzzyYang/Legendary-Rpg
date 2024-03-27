using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	public static CameraManager instance;
	public Camera mainCamera;
	public CinemachineVirtualCamera virtualCam1;


	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;
	}
}
