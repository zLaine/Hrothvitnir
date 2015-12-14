using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

    public int hearts;
	// Use this for initialization
	void Start () {

        hearts = MousePlayerControl.health;
	}
	
	// Update is called once per frame
	void Update () {

        hearts = MousePlayerControl.health;

        if(hearts == 2) {
            GameObject.Find("Heart (2)").GetComponent<Image>().enabled = false;
        }

        if (hearts == 1) {
            GameObject.Find("Heart (1)").GetComponent<Image>().enabled = false;
        }

        if (hearts == 0) {
            GameObject.Find("Heart").GetComponent<Image>().enabled = false;
        }
    }
}
