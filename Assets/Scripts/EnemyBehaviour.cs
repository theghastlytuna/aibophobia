using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject preFab; //bullet prefab
    public float fireRate;
    public float speed;
    public float holdDistance; //how far away the enemy should stop from the player
    private float coolDown;
    //private GameObject player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        coolDown += Time.deltaTime;

        //if we're off cooldown and the player is alive, shoot
        if (coolDown >= fireRate)
        {
            coolDown = 0;
            if (PlayerController.PlayerInstance.alive)
            {
                Instantiate(preFab, transform.position, transform.rotation);
            }
        }
    }

     void FixedUpdate()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        float distance = 0;

        //if player is alive, move towards them
        if (PlayerController.PlayerInstance.alive)
        {
            direction = Vector3.Normalize(PlayerController.PlayerInstance.transform.position - transform.position);
            distance = Vector3.Magnitude(PlayerController.PlayerInstance.transform.position - transform.position);

            if (distance > holdDistance)
            {
                rb.AddForce(direction * speed, ForceMode.Acceleration);
            }
        }
    }
}
