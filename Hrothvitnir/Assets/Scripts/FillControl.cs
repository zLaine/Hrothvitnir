using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FillControl : MonoBehaviour {

    public float threatLevel;
                              
    // Use this for initialization
	void Start () {
        threatLevel = 0;
    }
	
	// Update is called once per frame
	void Update () {

        threatLevel = MousePlayerControl.threat;

        if (threatLevel < 1) {
            GetComponent<Image>().enabled = false;
        }

        else {
            GetComponent<Image>().enabled = true;
        }

        GameObject.Find("Threat Level").GetComponent<Slider>().value = threatLevel;
    }
}
