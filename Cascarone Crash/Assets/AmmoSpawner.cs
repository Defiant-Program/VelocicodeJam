using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] Transform topLeft, topRight, bottomLeft, bottomRight;
    [SerializeField] GameObject eggCartonPrefab;
    [SerializeField] Transform eggCartonParent;

    int spawnTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        spawnTimer++;
        if (spawnTimer > 300)
            SpawnAmmo();
    }

    void SpawnAmmo()
    {
        spawnTimer = 0;
        Vector3 cartonSpawnPoint = new Vector3(Random.Range(topLeft.position.x, topRight.position.x), 1 , Random.Range(topLeft.position.z, bottomRight.position.z));
        Instantiate(eggCartonPrefab, cartonSpawnPoint, Quaternion.identity, eggCartonParent);
    }
}
