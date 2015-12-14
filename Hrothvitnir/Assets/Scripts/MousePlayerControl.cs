using UnityEngine;
using System.Collections;

public class MousePlayerControl : MonoBehaviour
{
    //Changes this to be static so I may access easily in other scripts
    public static float threat;
    public static int size;

    public float playerSpeed;
    public Camera mainCam;
    public bool hideable;
    public bool hiding;
    public int threatTimerValue;
    public int threatDecreaseTimerValue;
    public int damageTimerValue;
    public int health;


    Animator wolfWalk;

    bool flipped = false;
    int hideTimer = 10;
    int threatTickTimer;  //Timer for increasing threat
    int threatDecreaseTickTimer; //timer for decreasing threat
    int damageTimer;

    Collider2D hideableCollider; //Stores the most recent hideable object (that is actually hideable)
    Vector3 tempPosition; //used to determine if wolf is moving or not for animation purposes (since velocity isn't used)



    // Use this for initialization
    void Start()
    {
        threat = 0;
        threatTickTimer = 0;
        threatDecreaseTickTimer = 0;
        damageTimer = 0;
        size = 8;
        wolfWalk = GetComponent<Animator>();
        tempPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //prevents negative threat
        if (threat < 0)
        {
            threat = 0;
        }

        //hides behind object
        if (hiding == false && hideable == true)
        {
            hideTimer -= 1;
            float hide = Input.GetAxis("Vertical");
            if (hideTimer <= 0 && hide != 0 && hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder != 7)
            {
                hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 7;
                hiding = true;
                hideTimer = 10;
            }
        }
        //comes out from hiding
        else if (hiding == true)
        {
            hideTimer -= 1;
            float hide = Input.GetAxis("Vertical");
            if (hideTimer <= 0 && hide != 0 && hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder == 7)
            {
                hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                hiding = false;
                hideTimer = 10;
            }
        }
    }

    //Fixed Update resolves regularly, before physics finish
    void FixedUpdate()
    {
        //flips the sprite based on mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x > GetComponent<Renderer>().bounds.center.x && flipped == true)
        {
            Vector3 flipping = transform.localScale;
            flipping.x *= -1;// * flipping.y;
            transform.localScale = flipping;
            flipped = false;
        }
        else if (mousePosition.x < GetComponent<Renderer>().bounds.center.x && flipped == false)
        {
            Vector3 flipping = transform.localScale;
            flipping.x *= -1; //* flipping.y;
            transform.localScale = flipping;
            flipped = true;
        }

        //moves toward mouse position
        float input = Input.GetAxis("Horizontal");
        if (input > 0)
        {
            //moves towards the mouse at specified speed
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, playerSpeed);

            //controls walk animation
            wolfWalk.SetFloat("wolfSpeed", 1);

            //prevents the z from changing, fixes bug where character moved in front of camera
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        else
        {
            //stops walk animation
            wolfWalk.SetFloat("wolfSpeed", 0);
        }
    }


    void OnTriggerStay2D(Collider2D collider)
    {
        //determines whether an object is big enough to hide behind
        if (collider.transform.tag == "Hideable")
        {
            Vector3 wolfSize = GetComponent<Renderer>().bounds.size;
            Vector3 colliderSize = collider.GetComponent<Renderer>().bounds.size;

            Vector3 modWolfSize = new Vector3((Mathf.Abs(wolfSize.x) - Mathf.Abs(wolfSize.x) * .1f), (wolfSize.y - wolfSize.y * .1f), 1);

            if ((colliderSize.x >= modWolfSize.x) && (colliderSize.y >= modWolfSize.y))
            {
                hideable = true;
                hideableCollider = collider;
            }
            else
            {
                hideable = false;
            }
        }
        //handles threat increase for gods
        if (collider.transform.tag == "ThreatIncrease")
        {
            if (hiding == false)
            {
                threatTickTimer -= 1;
                if (threatTickTimer <= 0)
                {
                    threat += collider.gameObject.GetComponent<godVariables>().threatIncrease;
                    threatTickTimer = threatTimerValue;
                }
            }
        }

        //handles threat decrease for gods
        if (collider.transform.tag == "ThreatDecrease")
        {
            if (hiding == false)
            {
                threatDecreaseTickTimer -= 1;
                if (threatDecreaseTickTimer <= 0)
                {
                    if (threat > 0)
                    {
                        threat -= collider.gameObject.GetComponent<godVariables>().threatDecrease;
                    }
                    threatDecreaseTickTimer = threatDecreaseTimerValue;
                }
            }
        }

        //handles collision with food objects
        if (collider.transform.tag == "Food")
        {
            Vector3 wolfSize = GetComponent<Renderer>().bounds.size;
            Vector3 colliderSize = collider.GetComponent<Renderer>().bounds.size;

            Vector3 modWolfSize = new Vector3((Mathf.Abs(wolfSize.x) - Mathf.Abs(wolfSize.x) * .1f), (wolfSize.y - wolfSize.y * .1f), 1);

            if ((colliderSize.x <= modWolfSize.x) && (colliderSize.y <= modWolfSize.y))
            {
                Destroy(collider.gameObject);
                WolfGrow(collider.gameObject.GetComponent<foodStats>().growthAmount);
            }
            else
            {
                damageTimer -= 1;
                if (damageTimer <= 0)
                {
                    health -= collider.gameObject.GetComponent<foodStats>().damage;
                    damageTimer = damageTimerValue;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //comes out from hiding if you leave the collider
        if (collider.transform.tag == "Hideable")
        {
            hideable = false;
            hiding = false;
            hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
            hideableCollider = null;
            hideTimer = 10;
        }

        if (collider.transform.tag == "ThreatIncrease")
        {
            threatTickTimer = threatTimerValue;
        }

        if (collider.transform.tag == "ThreatDecrease")
        {
            threatDecreaseTickTimer = threatDecreaseTimerValue;
        }
    }


    void WolfGrow(int increment)
    {
        for (int i = 0; i < increment; i++)
        {
            //makes wolf grow
            transform.localScale = new Vector3(transform.localScale.x + .001f, transform.localScale.y + .001f, 1);

            //makes camera zoom out
            mainCam.orthographicSize += .001f;
            GetComponent<Rigidbody2D>().angularVelocity = 0;

            size += 1;
        }
    }
}
