using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    public GameObject preFab;
    public float spawnRate; //Seconds per spawn
    public int maxEnemyAlive; //Maximum enemies that are in the scene
    public int maxEnemySpawned; //Maximum enemies that can come from this spawner over one game
    private float cooldown = 0; //Time since last spawn
    public int enemyAlive = 0; //Current enemies in the scene
    public int enemySpawned = 0; //Current number of enemies that have been spawned by this spawner

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        enemyAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (cooldown >= spawnRate && enemyAlive < maxEnemyAlive && enemySpawned < maxEnemySpawned)
        {
            cooldown = 0;
            enemySpawned++;
            Instantiate(preFab, transform.position, transform.rotation);
        }
    }
}
