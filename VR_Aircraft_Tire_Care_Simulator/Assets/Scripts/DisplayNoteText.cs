using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisplayNoteText : MonoBehaviour
{
    public Transform Tag;

    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
        //Tag = GetComponent<GameObject>();
        //text = gameObject.transform.FindChild("Note Warning").gameObject;

    }


    // Update is called once per frame
    void Update()
    {
        

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("M36"))
        {
            text.SetActive(false);
        }
    }

    
}
