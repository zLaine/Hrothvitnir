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
        //Testing out displaying size instead of score
        //SetScoreText();

        SetSizeText();
	}
	
	// Update is called once per frame
	void Update () {

        score = MousePlayerControl.size - 8;
        //Testing out displaying size instead of score
        //SetScoreText();

        SetSizeText();
    }

    void SetScoreText() {
        scoreText.text = "Score: " + score.ToString();
    }

    void SetSizeText()
    {
        int wolfSize = MousePlayerControl.size * 5;
        int feet = wolfSize / 12;
        int inches = wolfSize % 12;

        scoreText.text = "Size: " + feet.ToString() + " ft " + inches.ToString() + " in";
    }
}
