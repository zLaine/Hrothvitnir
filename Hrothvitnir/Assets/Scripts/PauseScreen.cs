using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {

    public GameObject PauseUI;
    public bool isPaused = false;

    void Start() {

        PauseUI.SetActive(false);
    }

    void Update()  {

        if((Input.GetKeyDown(KeyCode.Escape))) {

            isPaused = !isPaused;
        }

        //This pauses if hits escape key
        if(isPaused) {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        //This resume if hit escape key
        if(!isPaused) {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }

    }

    //This is for the resume button if person doesn't want to hit resume
    public void Resume() {
        isPaused = false;
        Time.timeScale = 1;
    }
}
