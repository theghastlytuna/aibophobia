using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public class PlayerController : MonoBehaviour
{
    public static PlayerController PlayerInstance { get; private set; }
    public UnityEvent pauseEvent;
    public float speed = 1.0f;
    public float maxSpeed = 1.0f;
    public float dragFactor = 1.0f; //when over the max speed, drag is increased by a proportional amount
                                    //(speed - maxSpeed) * dragFactor
    [Range(0.0f, 10.0f)]
    public float jumpForce = 1.0f;
    public GameObject preFab;
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
        Vector3 movementVector = movementValue.Get<Vector3>();
        movementX = movementVector.x;
        movementY = movementVector.y;
        movementZ = movementVector.z;
    }

    void OnLook(InputValue mouse)
    {
        if (!controlLocked)
        {
            Vector2 mouseMovement = mouse.Get<Vector2>();
            Vector3 rotation = new Vector3(0.0f, mouseMovement.x * 0.5f, 0.0f);
            transform.Rotate(rotation);
        }
    }

    void OnFire()
    {
        if (!controlLocked)
        {
            startCast = true;
        }
    }

    void OnPause()
    {
        pauseEvent.Invoke();
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Floor")
    //    {
    //        onFloor = true;
    //    }
    //}

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onFloor = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
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
        //Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        //Vector3 actualMovement = Vector3.Scale(transform.forward, movement);
        //rb.AddForce(actualMovement * speed);
        rb.AddForce(transform.forward * movementZ * speed);
        rb.AddForce(Vector3.Cross(transform.forward, Vector3.up) * -movementX * speed);

        if (onFloor)
        {
            rb.AddForce(Vector3.up * movementY * jumpForce, ForceMode.Impulse);
        }

        int layerMask = 1 << 6;

        if (startCast)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                //Debug.Log("I'm looking at " + hit.transform.name);
                //Debug.Log(hit.point);
                Instantiate(preFab, hit.point, transform.rotation);
            }
            else
            {
                //Debug.Log("I'm looking at your mom");
            }
            startCast = false;
        }

        LimitSpeed();
    }

    public void ToggleControl()
    {
        controlLocked = !controlLocked;
    }

    public void LimitSpeed()
    {
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
