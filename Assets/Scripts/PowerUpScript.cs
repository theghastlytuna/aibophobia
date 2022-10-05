using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            ShootManager.ManagerInstance.activePowerup = true;
            Debug.Log("Triggered");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
