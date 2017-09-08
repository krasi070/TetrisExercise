using UnityEngine;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    public Slider musicSlider;
    public Text musicValue;
    public Slider soundEffectsSlider;
    public Text soundEffectsValue;
    public AudioSource musicAudioSource;
    public AudioSource soundEffectsAudioSource;

    private void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        float masterVolumeValue = Mathf.Round(GetComponent<Slider>().value * 100);
        transform.GetChild(0).GetComponent<Text>().text = masterVolumeValue.ToString();
        musicSlider.value = masterVolumeValue / 100;
        musicAudioSource.volume = masterVolumeValue / 100;
        musicValue.text = masterVolumeValue.ToString();
        soundEffectsSlider.value = masterVolumeValue / 100;
        soundEffectsAudioSource.volume = masterVolumeValue / 100;
        soundEffectsValue.text = masterVolumeValue.ToString();
        PlayerPrefs.SetString("MasterVolume", masterVolumeValue.ToString());
        PlayerPrefs.SetString("MusicVolume", masterVolumeValue.ToString());
        PlayerPrefs.SetString("SoundEffectsVolume", masterVolumeValue.ToString());
    }
}