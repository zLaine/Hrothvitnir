using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButtonGoTo : MonoBehaviour {

    public void LoadScene() {
        SceneManager.LoadScene("mainScene");
    }

}
