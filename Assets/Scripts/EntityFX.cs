using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
	[SerializeField] private Material hitMaterial;
	[SerializeField] private float hitFlashDuration;
	private SpriteRenderer sR;
	private Material originMaterial;
	// Start is called before the first frame update
	void Start()
	{
		sR = GetComponent<SpriteRenderer>();
		originMaterial = sR.material;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public IEnumerator FlashFX()
	{
		sR.material = hitMaterial;
		yield return new WaitForSeconds(hitFlashDuration);
		sR.material = originMaterial;
	}

	public void RedColorBlink()
	{
		if (sR.color == Color.white)
		{
			sR.color = Color.red;
		}
		else
		{
			sR.color = Color.white;
		}
	}
	public void CancelRedColorBlink()
	{
		CancelInvoke();
		sR.color = Color.white;
	}
}
