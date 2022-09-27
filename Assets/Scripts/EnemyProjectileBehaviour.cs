using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    public ParticleSystem emitter;
    private AudioSource sound;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //grab our attached components
        emitter = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();

        //if the player exists, move towards them on spawn
        Vector3 direction;
        if (PlayerController.PlayerInstance != null)
        {
            direction = Vector3.Normalize(PlayerController.PlayerInstance.transform.position - GetComponent<Transform>().position);
            rb.AddForce(direction * speed, ForceMode.VelocityChange);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if we hit the player, kill them
        if (collision.gameObject.tag == "Player")
        {
            PlayerController.PlayerInstance.alive = false;
            CameraController.CameraInstance.PlayerDeath();
        }

        //play particle effects on impact, detach the particles from the projectile
        emitter.Play();
        var main = emitter.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        emitter.transform.parent = null;

        //destroy projectile
        Destroy(gameObject);
    }
}
