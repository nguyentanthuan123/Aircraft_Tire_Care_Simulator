using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateACable : MonoBehaviour
{
    public GameObject prefab;
    public int number;

    private float distance;
    private GameObject clone ;
    // Start is called before the first frame update
    void Start()
    {
        distance = prefab.transform.localScale.y * 2;
        
        //float[] prefabPosition = new float[number - 1];
        float PositionX = prefab.transform.position.x;
        float PositionY = prefab.transform.position.y;
        float PositionZ = prefab.transform.position.z;

        clone = prefab;
        //for (int i = number; i > 0; i--)
        //{
        //    GameObject clone = Instantiate(prefab, new Vector3(PositionX, PositionY, PositionZ - distance), prefab.transform.rotation);
        //    PositionZ = clone.transform.position.z;
        //    clone.GetComponent<CharacterJoint>().connectedBody = prefab.GetComponent<Rigidbody>();
        //    prefab = clone;
        //}

        for (int i = number; i > 0; i--)
        {
            GameObject clone1 = Instantiate(prefab, new Vector3(PositionX, PositionY, PositionZ - distance), prefab.transform.rotation);
            PositionZ = clone1.transform.position.z;
            clone1.GetComponent<ConfigurableJoint>().connectedBody = prefab.GetComponent<Rigidbody>();
            prefab = clone1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
