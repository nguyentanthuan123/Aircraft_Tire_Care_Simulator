using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWarningTagZone1 : MonoBehaviour
{
    public Transform Zone;
    public Transform player;
    public float distanceCheck;

    private GameObject note;
   
    // Start is called before the first frame update
    void Start()
    {
        note = GameObject.Find("Note Warning");
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Zone.position, player.position);
        //Debug.Log(distance);
        if (distance < distanceCheck)
        {
            //Debug.Log("true");
            note.SetActive(true);
        }
        else
        {
            //Debug.Log("false");
            note.SetActive(false);
        }
    }
}
