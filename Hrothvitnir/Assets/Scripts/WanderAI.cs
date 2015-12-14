using UnityEngine;
using System.Collections;

public class WanderAI : MonoBehaviour {

    Vector3 targetLocation;
    Vector3 currentLocation;
    Vector3 previousLocation;

    Animator OdinWalk;

    public float walkSpeed;
    public bool hasAnimation;
    public bool walkRight;

	// Use this for initialization
	void Start ()
    {
        targetLocation = new Vector3(Random.Range(-2F, 2.6F), Random.Range(-1F, 2.4F), 0);
        previousLocation = transform.position;

        OdinWalk = GetComponent<Animator>();
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

        //controls the walking animations
        if(hasAnimation == true)
        {
            if(previousLocation.x < transform.position.x)
            {
                OdinWalk.SetBool("walkRight", true);
                walkRight = true;
            }
            else
            {
                OdinWalk.SetBool("walkRight", false);
                walkRight = false;
            }

            previousLocation = currentLocation;
        }
    }
}
