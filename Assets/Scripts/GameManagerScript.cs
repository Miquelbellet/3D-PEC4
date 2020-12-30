using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject carsParent;
    public GameObject zombiesSpawnPoints;
    public GameObject zombiePrefab;

    void Start()
    {
        if(PlayerPrefs.GetInt("GenerateZombies", 1) == 1) InvokeRepeating("SpawnZombie", 0f, Random.Range(10f, 15f));

        if (PlayerPrefs.GetInt("EnableCars", 1) == 1) carsParent.SetActive(true);
        else carsParent.SetActive(false);
    }

    void Update()
    {
        
    }

    private void SpawnZombie()
    {
        int randomSpawn = Random.Range(0, zombiesSpawnPoints.transform.childCount);
        GameObject zombie = Instantiate(zombiePrefab, zombiesSpawnPoints.transform.GetChild(randomSpawn).transform.position, Quaternion.Euler(0, 0, 0));
        zombie.SetActive(true);
    }
}
