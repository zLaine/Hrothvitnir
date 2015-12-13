using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScenes : MonoBehaviour {


    /*
    This line of code is considered obsolete by the compiler,
    replaced with SceneManager instead

    public void LoadScene() {
        Application.LoadLevel("mainScene");
    } 
    */

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
}
