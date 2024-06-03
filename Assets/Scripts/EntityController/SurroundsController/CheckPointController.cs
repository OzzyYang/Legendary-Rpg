using UnityEngine;

public class CheckPointController : MonoBehaviour
{
	[SerializeField] private string checkpointId;
	[SerializeField] private bool isActivated;
	private Animator animator;
	private Rigidbody2D rb;

	public string CheckpointId { get => checkpointId; private set => checkpointId = value; }
	public bool IsActivated { get => isActivated; private set => isActivated = value; }


	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{

	}

	[ContextMenu("Generate ID")]
	private void GenerateID()
	{
		checkpointId = System.Guid.NewGuid().ToString();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<PlayerController>() != null)
		{
			SetActivated(true);
			if (GameManager.Instance != null)
			{
				GameManager.Instance.LastCheckointPosition = transform.position;
			}
			if (SaveManager.Instance != null)
			{
				SaveManager.Instance.SaveGame();
			}

		}
		if (other.CompareTag("Ground"))
		{
			rb.constraints = RigidbodyConstraints2D.FreezeAll;
		}
	}

	public void SetActivated(bool activated)
	{
		animator.SetBool("isActivated", activated);
		IsActivated = activated;
	}
}
