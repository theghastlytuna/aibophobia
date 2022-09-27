using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileBehaviour : MonoBehaviour
{
    public float speed = 1.0f;
    public ParticleSystem emitter;
    private AudioSource sound;
    private GameObject player;
    private Rigidbody rb;
    private bool justSpawned = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        emitter = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
        Vector3 direction = Vector3.Normalize(player.transform.position - GetComponent<Transform>().position);
        rb.AddForce(direction * speed, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!justSpawned)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.BroadcastMessage("PlayerDeath");
                collision.gameObject.SetActive(false);
                emitter.Play();
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                //collision.gameObject.BroadcastMessage("EnemyDeath");
                collision.gameObject.SetActive(false);
                emitter.Play();
            }
            else
            {
                emitter.Play();
                var emitterMain = emitter.main;
                emitterMain.stopAction = ParticleSystemStopAction.Destroy;
                emitter.transform.parent = null;
                Destroy(gameObject);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (justSpawned)
        {
            justSpawned = false;
        }
    }
}
