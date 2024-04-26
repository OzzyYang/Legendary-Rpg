using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class UIVFX : VFX
{
	[SerializeField] private bool isMask;
	[SerializeField] private bool fadeInOnStart;
	//[SerializeField] private bool fadeOutOnQuit;
	[Range(0f, 1f)]
	[SerializeField] private float fadeSmoothness;
	private Image targetImage;
	private float fadeInterval;

	/// <summary>
	/// Execute <see cref="FadeOut(float)"/> Asynchronously.
	/// </summary>
	/// <param name="fadeDuration"></param>
	/// <returns></returns>
	public async Task FadeOutAsync(float fadeDuration)
	{
		FadeOut(fadeDuration);
		await Task.Delay((int)(Mathf.Clamp((targetImage.color.a - 1), 0, 1) * fadeDuration * 1000));
	}
	/// <summary>
	///	Execute fade-out effect, the entire fade-out from 100% to 0% transparency will last for <paramref name="fadeDuration"/> seconds.
	///	If the effect target's transparency is not 100%, the duration of the effect will be adjusted proportionally to the transparency percentage.
	/// </summary>
	/// <param name="fadeDuration"></param>
	public override void FadeOut(float fadeDuration)
	{
		CancelInvoke();
		targetImage = effectTarget.GetComponent<Image>();
		fadeInterval = 1 / (fadeDuration / fadeSmoothness);
		InvokeRepeating(nameof(DecreaseTransparency), 0, fadeSmoothness);
	}
	/// <summary>
	/// Execute <see cref="FadeIn"/> Asynchronously.
	/// </summary>
	/// <param name="fadeDuration"></param>
	/// <returns></returns>
	public async Task FadeInAsync(float fadeDuration)
	{
		FadeIn(fadeDuration);
		await Task.Delay((int)(Mathf.Clamp((1 - targetImage.color.a), 0, 1) * fadeDuration * 1000));
	}
	/// <summary>
	///	Execute fade-in effect, the entire fade-in from 0% to 100% transparency will last for <paramref name="fadeDuration"/> seconds.
	///	If the effect target's transparency is not 0%, the duration of the effect will be adjusted proportionally to the transparency percentage.
	/// </summary>
	/// <param name="fadeDuration"></param>
	/// <returns></returns>
	public override void FadeIn(float fadeDuration)
	{
		CancelInvoke();
		targetImage = effectTarget.GetComponent<Image>();
		fadeInterval = 1 / (fadeDuration / fadeSmoothness);
		InvokeRepeating(nameof(IncreaseTransparency), 0, fadeSmoothness);
	}

	private void DecreaseTransparency()
	{
		targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, targetImage.color.a - fadeInterval);
		if (targetImage.color.a <= 0) CancelInvoke(nameof(DecreaseTransparency));
	}

	private void IncreaseTransparency()
	{
		targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, targetImage.color.a + fadeInterval);
		if (targetImage.color.a >= 1) CancelInvoke(nameof(IncreaseTransparency));
	}

	protected override void Start()
	{
		base.Start();

		if (fadeInOnStart)
		{
			if (isMask) FadeOut(1.2f);
			else FadeIn(1.2f);
		}


	}

	protected override void Update()
	{
		base.Update();
	}
}
