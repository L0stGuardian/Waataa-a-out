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

    public void OnChangeSlider(float Value) // Usar camelCase en el parámetro; Revisar: una sola responsabilidad
    {
        _valueText.SetText($"{Value.ToString("N4")}");

        switch (_mixMode)
        {
            case AudioMixMode.LinearAudioSourceVolume:
                _audioSource.volume = Value;
                break;
            case AudioMixMode.LinearMixerVolume: // Separar los case con una línea en blanco
                _mixer.SetFloat("Volume", (-80 + Value * 80)); // Revisar: usar una constante o configuración con nombre
                break;
            case AudioMixMode.LogrithmicMixerVolume: // Separar los case con una línea en blanco; Corregir la ortografía del nombre
                _mixer.SetFloat("Volume", Mathf.Log10(Value) * 20);
                break;
        }

        float a = Mathf.Log10(Value) * 20; // Usar un nombre descriptivo; Revisar si este código es necesario

        PlayerPrefs.SetFloat("Volume", Value);
        PlayerPrefs.Save();
    }

    private void Start() // Respetar el orden de los miembros
    {
        _mixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("Volume",1) * 20));
    }

    public enum AudioMixMode // Mover el enum público a su propio archivo; Respetar el orden de los miembros
        { 
            LinearAudioSourceVolume,
            LinearMixerVolume,
            LogrithmicMixerVolume
        }
}
