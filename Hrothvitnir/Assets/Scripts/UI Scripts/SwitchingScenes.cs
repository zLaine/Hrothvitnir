using UnityEngine;
using System.Collections;

public class SwitchingScenes : MonoBehaviour {


    //Moves to scene with actual gameplay
    public void StartGame() {

        Application.LoadLevel("mainScene");
    }

    public void ToTitle() {

        Application.LoadLevel("Title");
    }
}
