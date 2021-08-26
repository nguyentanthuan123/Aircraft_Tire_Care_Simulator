using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class actionXR : MonoBehaviour
{
    public GameObject sword;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void spawnSword()
    {
        Instantiate(sword, transform.position, Quaternion.identity);
    }
}
