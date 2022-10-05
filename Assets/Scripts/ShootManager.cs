using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
    public static ShootManager ManagerInstance { get; private set; } //singleton
    public bool activePowerup = false;
    bool startCast;
    public GameObject preFab; //bullet prefab
    public Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Awake()
    {
        //singleton check
        if (ManagerInstance != null && ManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            ManagerInstance = this;
        }
    }

    void FixedUpdate()
    {
        int layerMask = 1 << 6; //only return a hit on layer 6 (walls)

        //since raycasting is a physics operation, it goes into fixed update
        if (startCast)
        {
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (activePowerup)
                {
                    Instantiate(preFab, hit.point, transform.rotation); //create a prefab at point of impact
                    Instantiate(preFab, hit.point + new Vector3(1,0,0), transform.rotation); //create a prefab at point of impact
                    Instantiate(preFab, hit.point + new Vector3(-1, 0, 0), transform.rotation); //create a prefab at point of impact

                }
                else
                {
                    Instantiate(preFab, hit.point, transform.rotation); //create a prefab at point of impact

                }
            }
            startCast = false;
        }
    }
    void OnFire()
    {
        //when click and controls enabled, start raycast
        if (!PlayerController.PlayerInstance.controlLocked)
        {
            startCast = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
