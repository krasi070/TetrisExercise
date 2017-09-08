using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    public AudioSource audioSource;

	void Update()
    {
        transform.GetChild(0).GetComponent<Text>().text = Mathf.Round(GetComponent<Slider>().value * 100).ToString();
        audioSource.volume = GetComponent<Slider>().value;
    }
}