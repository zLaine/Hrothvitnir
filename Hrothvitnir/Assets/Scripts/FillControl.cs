using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FillControl : MonoBehaviour {

    public int threatLevel;
                              
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (threatLevel < 1)
        {
            GetComponent<Image>().enabled = false;
        }
        else
        {
            GetComponent<Image>().enabled = true;
        }

    }
}
