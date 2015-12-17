using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Audio : MonoBehaviour {

    private int volumePercentage;
    public Text volumePercentageText;

    public GameObject MasterVolumeSlider;

	// Use this for initialization
	void Start () {

        AudioListener.volume = 0.5F;
        setVolume();
        MasterVolumeSlider.GetComponent<Slider>().value = AudioListener.volume;
        setPercentage();
	}
	
	// Update is called once per frame
	void Update () {

        setVolume();
        setPercentage();
        GameVolume();
    }

    void setPercentage() {

        volumePercentageText.text = volumePercentage.ToString() + "%";
    }

    //Needs to be called inside method
    void setVolume() {
        volumePercentage = (int)(AudioListener.volume * 100);
    }

    void GameVolume() {

        AudioListener.volume = MasterVolumeSlider.GetComponent<Slider>().value;
    }
}
