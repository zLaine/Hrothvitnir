using UnityEngine;
using System.Collections;

public class WanderAI : MonoBehaviour {

    public Vector3 targetLocation;
    Vector3 currentLocation;
    Vector3 previousLocation;

    bool isPaused;

    Animator walk;

    public float walkSpeed;
    public bool hasAnimation;
    public bool walkRight;

    // Use this for initialization
    void Start()
    {
        targetLocation = new Vector3(Random.Range(-2F, 2.6F), Random.Range(-1F, 2.4F), 0);
        previousLocation = transform.position;

        walk = GetComponent<Animator>();
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentLocation = transform.position;

        if ((Input.GetKeyDown(KeyCode.Escape)))
        {
            isPaused = !isPaused;
        }

        if (isPaused == false)
        {

            if (currentLocation == targetLocation)
            {
                targetLocation = new Vector3(Random.Range(-2F, 2.6F), Random.Range(-1F, 2.4F), 0);
            }

            transform.position = Vector3.MoveTowards(transform.position, targetLocation, walkSpeed);

            //controls the walking animations
            //this one works for asymmetrical characters
            if (hasAnimation == true && this.gameObject.name == "Odin")
            {
                if (previousLocation.x < transform.position.x)
                {
                    walk.SetBool("walkRight", true);
                    walkRight = true;
                }
                else
                {
                    walk.SetBool("walkRight", false);
                    walkRight = false;
                }

                previousLocation = currentLocation;
            }
            //symmetrical characters
            else if (hasAnimation == true)
            {
                if (previousLocation.x < transform.position.x)
                {
                    //honestly we don't really need this part but I am going to leave it in to maybe make this easier to read.
                    //Sprite walks right automatically.
                    walkRight = true;
                }
                else
                {
                    Vector3 flipping = transform.localScale;
                    flipping.x = -Mathf.Abs(flipping.x);
                    transform.localScale = flipping;
                    walkRight = false;
                }

                previousLocation = currentLocation;
            }
        }
    }
}
