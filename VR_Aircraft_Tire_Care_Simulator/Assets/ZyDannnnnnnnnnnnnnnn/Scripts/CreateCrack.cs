using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCrack : MonoBehaviour
{
    public GameObject wheel;
    public GameObject[] spawnPosition;
    private GameObject crackPrefab;
    private int random;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = GameObject.FindGameObjectsWithTag("SpawnPosition");
        crackPrefab = GameObject.Find("Crack");
        
        //random create crack
        if(Random.value >= 0.5f)
        {
            spawnCrack();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        crackPrefab.transform.position = spawnPosition[random].transform.position;
    }


    //random spawn crack 
    private void spawnCrack()
    {
        random = Random.Range(0, spawnPosition.Length);
        GameObject crack =  Instantiate(crackPrefab, spawnPosition[random].transform.position, Quaternion.identity);
    }
}
