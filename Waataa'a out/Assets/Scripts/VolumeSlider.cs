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

    private void Start() 
    {
        _mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume",1) * 20));
    }

    public void OnChangeSlider(float volumeValue) 
    {
        _valueText.SetText($"{volumeValue.ToString("N4")}");

        switch (_mixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                _audioSource.volume = volumeValue;
                break;

            case AudioMixMode.LinearMixerVolume: 
                _mixer.SetFloat("Volume", (-80 + volumeValue * 80)); // Revisar: usar una constante o configuración con nombre
                break;

            case AudioMixMode.LogrithmicMixerVolume:
                _mixer.SetFloat("Volume", Mathf.Log10(volumeValue) * 20);
                break;
        }
    }

}
