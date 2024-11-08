using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    public GameObject[] Enemies;
    private EnemySpwaner Spawner;
    // Start is called before the first frame update
    void Start()
    {
        Spawner = GameObject.Find("EnemySpwaner").GetComponent<EnemySpwaner>();
    }

    // Update is called once per frame
    void Update()
    {
        bool allEnemiesDestroyed = true;

        // Check if all enemies are destroyed
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] != null) // If any enemy is still active, set the flag to false
            {
                allEnemiesDestroyed = false;
                break;
            }
        }

        // If all enemies are destroyed, trigger enemy spawning
        if (allEnemiesDestroyed)
        {
            Spawner.DoSpawn();
            Destroy(gameObject);
        }
    }
}
