using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {

    //this will be based on size
    private int score;

    public Text scoreText;

	// Use this for initialization
	void Start () {

        score = MousePlayerControl.size - 8;
        SetScoreText();

	}
	
	// Update is called once per frame
	void Update () {

        score = MousePlayerControl.size - 8;
        SetScoreText();
	}

    void SetScoreText() {
        scoreText.text = "Score: " + score.ToString();
    }
}
