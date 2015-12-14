using System.Collections;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class GoToScenes : MonoBehaviour {

    public void LoadGameScene() {
        Application.LoadLevel("mainScene");
    }

    public void LoadCreditScene() {
        Application.LoadLevel("Credit Screen");
    }

    public void LoadOptionsScene() {
        Application.LoadLevel("Options Menu");
    }

    public void LoadScoreScene() {
        Application.LoadLevel("Score Screen");
    }

    public void LoadTitleScene() {
        Application.LoadLevel("Title");
    }

    /*
    //This code requires a dependancy outside of standard unity, which is expensive.
    //We need to use the above code instead.

    public void LoadGameScene() {
        SceneManager.LoadScene("mainScene");
    }

    public void LoadCreditScene() {
        SceneManager.LoadScene("Credit Screen");
    }

    public void LoadOptionsScene() {
        SceneManager.LoadScene("Options Menu");
    }

    public void LoadScoreScene() {
        SceneManager.LoadScene("Score Screen");
    }

    public void LoadTitleScene() {
        SceneManager.LoadScene("Title");
    }
    */
}
