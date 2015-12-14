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

    AudioSource source;
    public AudioClip chomp;
    public AudioClip wolfWalking;
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

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if the game has ended
        FailState();

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

        //makes sure the gowth scales correctly and doesn't warp
        if(transform.localScale.x != transform.localScale.y)
        {
            if(Mathf.Abs(transform.localScale.x) > transform.localScale.y)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.x), 1);
            }
            else
            {
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(transform.localScale.y, transform.localScale.y, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-transform.localScale.y, transform.localScale.y, 1);
                }
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
            flipping.x *= -1;
            transform.localScale = flipping;
            flipped = false;
        }
        else if (mousePosition.x < GetComponent<Renderer>().bounds.center.x && flipped == false)
        {
            Vector3 flipping = transform.localScale;
            flipping.x *= -1;
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

            //plays sound
            source.Play();

            //prevents the z from changing, fixes bug where character moved in front of camera
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        else
        {
            //stops walk animation
            wolfWalk.SetFloat("wolfSpeed", 0);

            //stops sound
            source.Stop();
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
                //plays sound
                source.PlayOneShot(chomp, .2f);

                //respawns the food in another location
                foodRespawn(collider);
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
            threatTickTimer = 0;
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
            transform.localScale = new Vector3(transform.localScale.x + .003f, transform.localScale.y + .003f, 1);

            //makes camera zoom out
            mainCam.orthographicSize += .004f;
            GetComponent<Rigidbody2D>().angularVelocity = 0;

            size += 1;
        }
    }

    void foodRespawn(Collider2D collider)
    {
        int increment = collider.gameObject.GetComponent<foodStats>().growthAmount;
        Vector3 wolfSize = GetComponent<Renderer>().bounds.size;
        Vector3 colliderSize = collider.GetComponent<Renderer>().bounds.size;
        Vector3 modWolfSize = new Vector3((Mathf.Abs(wolfSize.x) - Mathf.Abs(wolfSize.x) * .1f), (wolfSize.y - wolfSize.y * .1f), 1);

        collider.transform.position = new Vector3(Random.Range(-2F, 2.6F), Random.Range(-1F, 2.4F), 0);
        collider.transform.localScale = new Vector3(collider.transform.localScale.x + .001f , collider.transform.localScale.y + .001f, 1);

        //everything else ends up eatable right away when respawned
        if(collider.name != "Big Food" && collider.name != "Very Big Food" && (colliderSize.x <= modWolfSize.x) && (colliderSize.y <= modWolfSize.y))
        {
            collider.transform.localScale = new Vector3(collider.transform.localScale.x - .0005f, collider.transform.localScale.y - .0005f, 1);
        }
        //1.8 size when eaten
        if(collider.name == "Big Food")
        {
            collider.transform.localScale = new Vector3(collider.transform.localScale.x + (collider.transform.localScale.x * .8f), collider.transform.localScale.y + (collider.transform.localScale.y * .8f), 1);
        }
        //triples in size when eaten
        if (collider.name == "Very Big Food")
        {
            collider.transform.localScale = new Vector3(collider.transform.localScale.x + (collider.transform.localScale.x * 2f), collider.transform.localScale.y + (collider.transform.localScale.y * 2f), 1);
        }
    }

    //will be checked for either fail states of threat reaching 100 or losing all health
    void FailState() {

        //The reason for greater than as well is because some threats increase at different incrememnts meaning it may not always be exactly 100
        if (threat >= 100) {
            Application.LoadLevel("Game Over");
        }
    }
}
