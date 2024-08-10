using UnityEngine;
using UnityEngine.UI;

public class BGvolume_btn : MonoBehaviour
{

    // UI Slider 요소
    public Slider volumeSlider;
    // 볼륨을 조절할 AudioSource
    public AudioSource musicSource;

    void Update()
    {

        if (!musicSource)
        {
            GameObject MusicSource = GameObject.FindGameObjectWithTag("Music");
            musicSource = MusicSource.GetComponent<AudioSource>();
            // Slider의 값이 변경될 때마다 OnVolumeChanged 함수를 호출합니다.
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }
        
    }





    // Slider의 값이 변경될 때 호출되는 함수
    void OnVolumeChanged(float volume)
    {

        // AudioSource의 볼륨을 Slider 값으로 설정합니다.
        musicSource.volume = volume;

    }

}
