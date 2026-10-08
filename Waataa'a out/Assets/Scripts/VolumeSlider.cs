using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private AudioMixer _mixer;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private TextMeshProUGUI _valueText;
    [SerializeField]
    private AudioMixMode _mixMode;

    public void OnChangeSlider(float Value)
    {
        _valueText.SetText($"{Value.ToString("N4")}");

        switch (_mixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                _audioSource.volume = Value;
                break;
            case AudioMixMode.LinearMixerVolume:
                _mixer.SetFloat("Volume", (-80 + Value * 80));
                break;
            case AudioMixMode.LogrithmicMixerVolume:
                _mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;
        }

        float a = Mathf.Log10(Value) * 20;

        PlayerPrefs.SetFloat("Volume", Value);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        _mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume",1) * 20));
    }

    public enum AudioMixMode
        { 
            LinearAudioSourceVolume,
            LinearMixerVolume,
            LogrithmicMixerVolume
        }
}
