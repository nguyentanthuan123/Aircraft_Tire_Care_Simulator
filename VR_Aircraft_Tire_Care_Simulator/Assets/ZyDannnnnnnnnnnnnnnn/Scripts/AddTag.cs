using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTag : MonoBehaviour
{
    public GameObject Zone;
    public GameObject Tag;
    public float distanceCheck = 0.2f;

    private float distance = 0;
    private GameObject Note;
    private Rigidbody rigid;
    private GameObject attachTransform;


    // Start is called before the first frame update
    void Start()
    {
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
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rigid.constraints == RigidbodyConstraints.FreezeAll)
        {
            rigid.constraints = RigidbodyConstraints.None;
            new WaitForSeconds(2);
        }

        //Debug.Log("Hit");
        if (other.gameObject.tag.Equals("LeftHandController"))
        {
            Note.SetActive(true);
            Zone.GetComponent<MeshRenderer>().enabled = true;
        }

        if (other.gameObject.tag.Equals("RightHandController"))
        {
            Note.SetActive(true);
            Zone.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (distance < distanceCheck && rigid.constraints == RigidbodyConstraints.None)
        {
            Tag.transform.position = attachTransform.transform.position;
            Tag.transform.rotation = attachTransform.transform.rotation;
            rigid.constraints = RigidbodyConstraints.FreezeAll;     
        }
        Zone.GetComponent<MeshRenderer>().enabled = false;
        Note.SetActive(false);
    }
}
