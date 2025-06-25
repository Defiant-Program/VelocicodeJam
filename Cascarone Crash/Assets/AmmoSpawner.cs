using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    [SerializeField] Transform topLeft, topRight, bottomLeft, bottomRight;
    [SerializeField] GameObject eggCartonPrefab;
    [SerializeField] GameObject hatPrefab;
    [SerializeField] Transform eggCartonParent;
    [SerializeField] Transform hatParent;

    [SerializeField] Transform alamo;

    int eggSpawnTimer;
    int hatSpawnTimer;

    int alamoSpawnTimer;

    GameObject alammo;
    GameObject hatalamo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        eggSpawnTimer++;
        if (eggSpawnTimer > 300)
            SpawnAmmo();
        hatSpawnTimer++;
        if (hatSpawnTimer > 600)
            SpawnHat();
        alamoSpawnTimer++;
        if (alamoSpawnTimer > 900)
        {
            if (!alammo)
                SpawnAlammo();
            if (!hatalamo)
                SpawnHatAlamo();
        }
    }

    void SpawnAlammo()
    {
        alamoSpawnTimer = 0;
        alammo = Instantiate(eggCartonPrefab, new Vector3(alamo.position.x - 3, 1.1f, alamo.position.z - 7), Quaternion.identity, eggCartonParent);
    }
    void SpawnHatAlamo()
    {
        alamoSpawnTimer = 0;
        hatalamo = Instantiate(hatPrefab, new Vector3(alamo.position.x + 3, 1.1f, alamo.position.z - 7), Quaternion.identity, hatParent);
    }

    void SpawnAmmo()
    {
        eggSpawnTimer = 0;
        Vector3 cartonSpawnPoint = new Vector3(GameController.GC.player.transform.position.x + Random.insideUnitCircle.x * 20, 1.1f, GameController.GC.player.transform.position.z + Random.insideUnitCircle.y * 20);
        Instantiate(eggCartonPrefab, cartonSpawnPoint, Quaternion.identity, eggCartonParent);
    }

    void SpawnHat()
    {
        hatSpawnTimer = 0;
        Vector3 hatSpawnPoint = new Vector3(Random.Range(Mathf.Max(topLeft.position.x, GameController.GC.player.transform.position.x - 200), Mathf.Min(topRight.position.x, GameController.GC.player.transform.position.x + 200)), 1, Random.Range(Mathf.Max(topLeft.position.z, GameController.GC.player.transform.position.z - 200), Mathf.Min(bottomRight.position.z, GameController.GC.player.transform.position.z + 200)));
        Instantiate(hatPrefab, hatSpawnPoint, Quaternion.identity, hatParent);
    }
}
