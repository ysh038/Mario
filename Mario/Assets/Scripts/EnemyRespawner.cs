using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject []enemy;

    public float respawnDelay;
    private float lastSpawn;

    public int numOfEnemy;
    private int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        //enemyPrefab = GetComponent<GameObject>();
        lastSpawn = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= lastSpawn + respawnDelay && enemyCount < numOfEnemy )
        {
            lastSpawn = Time.time;
            enemy[enemyCount] = Instantiate(enemyPrefab, new Vector3(25,3,0),Quaternion.identity);
            enemyCount++;
        }
    }
}
