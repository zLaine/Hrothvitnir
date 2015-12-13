using UnityEngine;
using System.Collections;

public class MousePlayerControl : MonoBehaviour
{

    public float playerSpeed;
    public float threat;
    public Camera mainCam;
    public bool hideable;
    public bool hiding;
    public int threatTimerValue = 50;
    public int threatDecreaseTimerValue = 60;

    bool flipped = false;
    int hideTimer = 10;
    int threatTickTimer;  //Timer for increasing threat
    int threatDecreaseTickTimer; //timer for decreasing threat

    Collider2D hideableCollider; //Stores the most recent hideable object (that is actually hideable)
    


    // Use this for initialization
    void Start()
    {
        threat = 0;
        threatTickTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //prevents negative threat
        if (threat < 0)
        {
            threat = 0;
        }

        //flips the sprite based on mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePosition.x < 0 && flipped == true)
        {
            Vector3 flipping = transform.localScale;
            flipping.x *= -1;// * flipping.y;
            transform.localScale = flipping;
            flipped = false;
        }
        else if (mousePosition.x > 0 && flipped == false)
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
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, playerSpeed);
            //prevents the z from changing, fixes bug where character moved in front of camera
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }


        //hides behind object
        if(hiding == false && hideable == true)
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


    void OnTriggerStay2D(Collider2D collider)
    {
        //determines whether an object is big enough to hide behind
        if (collider.transform.tag == "Hideable")
        {
            Vector3 wolfScale = transform.localScale;
            Vector3 colliderScale = collider.transform.localScale;

            Vector3 modWolfScale = new Vector3((Mathf.Abs(wolfScale.x) + Mathf.Abs(wolfScale.x) * .1f), (wolfScale.y + wolfScale.y * .1f), 1);

            if ((colliderScale.x >= modWolfScale.x) && (colliderScale.y >= modWolfScale.y))
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


    void Grow(int increment)
    {
        //makes wolf grow
        transform.localScale = new Vector3(transform.localScale.x + .001f * increment, transform.localScale.y + .001f * increment, 1);

        //makes camera zoom out
        mainCam.orthographicSize += increment * .01f;
        GetComponent<Rigidbody2D>().angularVelocity = 0;
    }
}
