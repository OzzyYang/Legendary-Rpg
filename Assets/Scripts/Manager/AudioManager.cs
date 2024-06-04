using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager insance;
	[SerializeField] private double minSoundDistance = 10;
	[SerializeField] private AudioSource[] sfx;
	[SerializeField] private AudioSource[] bgm;

	private int currentBGMIndex = -1;
	private bool isPlaying = false;

	public int CurrentBGMIndex { get => currentBGMIndex; private set => currentBGMIndex = value; }
	public bool IsPlaying { get => isPlaying; private set => isPlaying = value; }

	private void Awake()
	{
		if (insance == null)
		{
			insance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	private void Start()
	{
		PlayBGMByIndex(0);
	}

	public void PlaySFXByIndex(int index) => this.PlaySFXByIndex(index, PlayerManager.Instance.transform);

	public void PlaySFXByIndex(int index, Transform source)
	{
		if (sfx[index] == null) return;
		if (sfx[index].isPlaying) return;
		if (Vector2.Distance(source.position, PlayerManager.Instance.transform.position) > minSoundDistance) return;

		sfx[index].Play();
	}

	public void StopSFXByIndex(int index)
	{
		if (sfx[index] == null) return;
		sfx[index].Stop();
	}

	public void PlayBGMByIndex(int index)
	{
		if (index >= bgm.Length || currentBGMIndex == index) return;
		if (currentBGMIndex == -1) StopBGMByIndex(currentBGMIndex);
		bgm[index].Play();
		currentBGMIndex = index;
	}

	public void StopBGMByIndex(int index)
	{
		if (index >= bgm.Length || index <= 0) return;
		bgm[index].Stop();
		currentBGMIndex = -1;
	}

	public void StopAllSFX()
	{
		foreach (var item in sfx)
		{
			item.Stop();
		}
	}

	public void StopAllBGM()
	{
		foreach (var item in bgm)
		{
			item.Stop();
		}
		currentBGMIndex = -1;
	}
}
