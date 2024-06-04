using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIVolumeController : MonoBehaviour
{
	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private Slider slider;
	[SerializeField] private string mixerParam;
	[SerializeField] private float multiplier = 1.0f;

	public string MixerParam { get => mixerParam; private set => mixerParam = value; }
	public Slider Slider { get => slider; private set => slider = value; }

	public void AjustVolumeBySlider(float value)
	{
		Debug.Log(value);
		audioMixer.SetFloat(MixerParam, value * 100 - 80);
	}
}
