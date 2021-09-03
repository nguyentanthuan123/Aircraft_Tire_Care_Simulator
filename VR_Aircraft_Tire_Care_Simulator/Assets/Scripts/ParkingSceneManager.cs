using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkingSceneManager : MonoBehaviour
{
    public GameObject handManagerPrefab;
    private static ParkingSceneManager instance;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            GameObject handManagerKeeper = Instantiate(handManagerPrefab, gameObject.transform.position, Quaternion.identity);
            DontDestroyOnLoad(handManagerKeeper);
            DontDestroyOnLoad(gameObject);
        }

    }
}
