using UnityEngine;
using System.Collections;

public class TiltleMenu : MonoBehaviour {

    //List of Canvas Objects
    public GameObject TitleCanvas;

    public GameObject OptionsCanvas;
    public GameObject ControlsCanvas;
    public GameObject VideoCanvas;
    public GameObject AudioCanvas;
    public GameObject ScoreCanvas;
    public GameObject CreditCanvas;


	// Use this for initialization
	void Start () {

        TitleCanvas.SetActive(true);

        OptionsCanvas.SetActive(false);
        ControlsCanvas.SetActive(false);
        VideoCanvas.SetActive(false);
        AudioCanvas.SetActive(false);
        ScoreCanvas.SetActive(false);
        CreditCanvas.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {


	}

    //Makes Title Menu visible when called
    public void TitleMenu() {

        OptionsCanvas.SetActive(false);

        TitleCanvas.SetActive(true);
    }

    //Makes Option Menu visible when called
    public void OptionsMenu() {

        TitleCanvas.SetActive(false);

        ControlsCanvas.SetActive(false);
        VideoCanvas.SetActive(false);
        AudioCanvas.SetActive(false);

        OptionsCanvas.SetActive(true);
    }

    //Makes Control Menu visible when called
    public void ControlMenu()
    {
        OptionsCanvas.SetActive(false);

        ControlsCanvas.SetActive(true);
    }

    //Makes Video Menu visible when called
    public void VideoMenu() {
        OptionsCanvas.SetActive(false);

        VideoCanvas.SetActive(true);
    }

    //Makes Audio Menu visible when called
    public void AudioMenu() {
        OptionsCanvas.SetActive(false);

        AudioCanvas.SetActive(true);
    }

    public void ScoreMenu() {
        TitleCanvas.SetActive(false);

        ScoreCanvas.SetActive(true);
    }

    public void CreditMenu() {
        TitleCanvas.SetActive(false);

        CreditCanvas.SetActive(true);
    }
}
