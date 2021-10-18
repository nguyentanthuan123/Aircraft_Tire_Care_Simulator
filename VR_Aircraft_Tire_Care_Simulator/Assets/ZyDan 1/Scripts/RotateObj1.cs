using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj1 : MonoBehaviour
{
    private Transform obj;
    private Vector3 currentPos;
    // Start is called before the first frame update
    void Start()
    {
        obj=null;
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(currentPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("CanBeRotate"))
        {
            Debug.Log("1");
            obj = other.transform.parent;
            obj.parent = this.transform;
            obj.transform.localPosition = new Vector3(0, obj.transform.localPosition.y, 0);
            currentPos = obj.TransformPoint(obj.transform.localPosition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag.Equals("CanBeRotate"))
        {
            Debug.Log('2');
            //currentPos = obj.TransformPoint(obj.transform.position);
            //obj.transform.position = currentPos;
            //obj.parent = null;
            
        }
    }
}
