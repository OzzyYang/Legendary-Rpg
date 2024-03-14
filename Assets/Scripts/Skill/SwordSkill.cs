using UnityEngine;
public class SwordSkill : Skill
{
	[SerializeField] private GameObject swordObject;
	[SerializeField] private float throwForce = 600f;

	[Header("Aim Info")]
	[SerializeField] private GameObject dotObject;
	private int dotsNum = 20;
	private GameObject[] dots;
	public override bool CanUseSkill()
	{
		return base.CanUseSkill();
	}

	public override void UseSkill()
	{
		base.UseSkill();
		GameObject sword = Instantiate(swordObject);
		sword.GetComponent<SwordController>().SetupSword(CaculateDirection(), player.attackCheck.transform.position, throwForce);
	}

	private Vector2 CaculateDirection()
	{
		Vector2 launchPosition = player.attackCheck.transform.position;
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		return mousePosition - launchPosition;
	}

	protected override void Awake()
	{
		base.Awake();
	}

	protected override void Start()
	{
		base.Start();
		GenerateDot();
	}

	protected override void Update()
	{
		base.Update();
	}


	private Vector2 CaculateAimPosition(Vector2 _throwDirection, Vector2 _throwPosition, float _time)
	{
		float theta = Mathf.Atan2(_throwDirection.normalized.y, _throwDirection.normalized.x);
		return new Vector2(_throwPosition.x + throwForce * Mathf.Cos(theta) * _time, _throwPosition.y + throwForce * Mathf.Sin(theta) * _time + 0.5f * Physics2D.gravity.y * _time * _time);
	}

	private void GenerateDot()
	{
		dots = new GameObject[dotsNum];
		for (int i = 0; i < dotsNum; i++)
		{
			dots[i] = GameObject.Instantiate(dotObject, player.attackCheck.position, Quaternion.identity);
			dots[i].SetActive(false);
		}
	}

	public void NeedAimLine(bool _needFlag)
	{
		for (int i = 0; i < dotsNum; i++)
		{
			if (_needFlag) dots[i].transform.position = CaculateAimPosition(CaculateDirection(), player.attackCheck.position, i * 0.1f);
			dots[i].SetActive(_needFlag);
		}
	}

}
