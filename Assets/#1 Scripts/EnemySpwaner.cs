using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
    int SpwanType;
    public GameObject[] EnemyFlight;
    // Start is called before the first frame update
    void Start()
    {
        SpwanType = Random.Range(0,10);
        StartCoroutine(SpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2.5f);
        Instantiate(EnemyFlight[SpwanType], transform.position, Quaternion.identity);
        
        SpwanType = Random.Range(0,10);

        yield return null;
    }

    public void DoSpawn()
    {
        StartCoroutine(SpawnEnemy());
    }
}
