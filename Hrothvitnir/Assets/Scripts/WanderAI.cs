using UnityEngine;
using System.Collections;

public class WanderAI : MonoBehaviour {

    Vector3 targetLocation;
    Vector3 currentLocation;
    public float walkSpeed;

	// Use this for initialization
	void Start ()
    {
        targetLocation = new Vector3(Random.Range(-2F, 2.6F), Random.Range(-1F, 2.4F), 0);
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentLocation = transform.position;

        if(currentLocation == targetLocation)
        {
            targetLocation = new Vector3(Random.Range(-2F, 2.6F), Random.Range(-1F, 2.4F), 0);
        }

        transform.position = Vector3.MoveTowards(transform.position, targetLocation, walkSpeed);

    }
}
