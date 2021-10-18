using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTag : MonoBehaviour
{
    public GameObject Zone;
    public GameObject Tag;
    public float distanceCheck = 0.05f;

    private float distance = 0;
    private GameObject Note;
    private Rigidbody rigid;
    private GameObject attachTransform;
    private bool isAttached;


    // Start is called before the first frame update
    void Start()
    {
        isAttached = false;
        attachTransform = Zone.transform.Find("Attach Transform").gameObject;
        Note = Zone.transform.Find("Note Warning").gameObject;
        Note.SetActive(false);
        rigid = GetComponent<Rigidbody>();
        Zone.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Zone.transform.position, Tag.transform.position);
        if (distance < distanceCheck && !isAttached)
        {
            //Debug.Log("1");
            Tag.transform.position = attachTransform.transform.position;
            Tag.transform.rotation = attachTransform.transform.rotation;
            rigid.constraints = RigidbodyConstraints.FreezeAll;

            isAttached = true;
        }
        if(distance > distanceCheck)
        {
            rigid.constraints = RigidbodyConstraints.None;
            isAttached = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit");
        if (other.gameObject.tag.Equals("LeftHandController"))
        {
            if (!isAttached)
            {
                Note.SetActive(true);
                Zone.GetComponent<MeshRenderer>().enabled = true;
            }
            
        }

        if (other.gameObject.tag.Equals("RightHandController"))
        {
            if(!isAttached)
            {
                Note.SetActive(true);
                Zone.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Zone.GetComponent<MeshRenderer>().enabled = false;
        Note.SetActive(false);
    }
}
