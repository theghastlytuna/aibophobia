using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    public static PlayerController PlayerInstance { get; private set; } //singleton
    public UnityEvent pauseEvent;
    public float speed = 1.0f;
    public float maxSpeed = 1.0f;
    public float dragFactor = 1.0f; //when over the max speed, drag is increased by a proportional amount
                                    //(speed - maxSpeed) * dragFactor
    [Range(0.0f, 10.0f)]
    public float jumpForce = 1.0f;
    public GameObject preFab; //bullet prefab
    public Camera playerCamera;

    private Rigidbody rb;
    private float movementX, movementY, movementZ;
    private bool controlLocked = false;
    bool onFloor;
    bool startCast;
    public bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Awake()
    {
        //singleton check
        if (PlayerInstance != null && PlayerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            PlayerInstance = this;
        }
    }
    void OnMove(InputValue movementValue)
    {
        //grab and store current inputs
        Vector3 movementVector = movementValue.Get<Vector3>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        movementZ = movementVector.z;
    }

    void OnLook(InputValue mouse)
    {
        //grab and store mouse inputs
        if (!controlLocked)
        {
            Vector2 mouseMovement = mouse.Get<Vector2>();
            Vector3 rotation = new Vector3(0.0f, mouseMovement.x * 0.5f, 0.0f);
            transform.Rotate(rotation);
        }
    }

    void OnFire()
    {
        //when click and controls enabled, start raycast
        if (!controlLocked)
        {
            startCast = true;
        }
    }

    void OnPause()
    {
        //when escape is pressed, trigger event to the canvas UI
        pauseEvent.Invoke();
    }

    void OnCollisionExit(Collision collision)
    {
        //if we stop touching the floor
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //as long as we're on the floor
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = true;
        }
        else
        {
            onFloor = false;
        }
    }

    void FixedUpdate()
    {
        //these 2 lines are for horizontal movement, movement on the x axis is weird and needs to be reversed
        rb.AddForce(transform.forward * movementZ * speed);
        rb.AddForce(Vector3.Cross(transform.forward, Vector3.up) * -movementX * speed);

        //only jump if we're on the floor
        if (onFloor)
        {
            //get movement amount from input
            rb.AddForce(Vector3.up * movementY * jumpForce, ForceMode.Impulse);
        }

        int layerMask = 1 << 6; //only return a hit on layer 6 (walls)

        //since raycasting is a physics operation, it goes into fixed update
        if (startCast)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.Log("I'm looking at " + hit.transform.name); //print the object that we hit
                //Debug.Log(hit.point); //print coordinates of hit
                Instantiate(preFab, hit.point, transform.rotation); //create a prefab at point of impact
            }
            else
            {
                //Debug.Log("I'm looking at your mom"); //we didn't hit anything
            }
            startCast = false;
        }

        LimitSpeed();
    }

    public void ToggleControl()
    {
        //used to remove control when paused
        controlLocked = !controlLocked;
    }

    public void LimitSpeed()
    {
        //dynamically reduces the player speed when they get above a certain speed
        if (rb.velocity.sqrMagnitude > maxSpeed)
        {
            rb.drag = 0.05f + (rb.velocity.sqrMagnitude - maxSpeed) / 10 * dragFactor;
        }
        else
        {
            rb.drag = 0.05f;
        }
    }
}
