using UnityEngine;
using System.Collections;

public class MousePlayerControl : MonoBehaviour
{

    public float playerSpeed;
    public float threat;
    public Camera mainCam;
    public bool hideable;
    public bool hiding;

    bool flipped = false;
    float timer = 10;

    Collider2D hideableCollider;
    


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            timer -= 1;
            float hide = Input.GetAxis("Vertical");
            if (timer < 0 && hide != 0 && hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder != 7)
            {
                hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 7;
                hiding = true;
                timer = 10;
            }
        }
        //comes out from hiding
        else if (hiding == true)
        {
            timer -= 1;
            float hide = Input.GetAxis("Vertical");
            if (timer < 0 && hide != 0 && hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder == 7)
            {
                hideableCollider.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                hiding = false;
                timer = 10;
            }
        }

    }


    void OnTriggerStay2D(Collider2D collider)
    {
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
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.tag == "Hideable")
        {
            hideable = false;
            hideableCollider = null;
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
