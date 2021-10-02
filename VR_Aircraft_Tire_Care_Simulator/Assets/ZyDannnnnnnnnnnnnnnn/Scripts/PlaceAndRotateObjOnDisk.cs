using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceAndRotateObjOnDisk : MonoBehaviour
{
    public Transform target;

    private GameObject obj;
    private Rigidbody rb;
    private Vector3 direction;
    [SerializeField]
    private float moveObjSpeed;
    private float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        target = this.transform.Find("Center");
    }

    // Update is called once per frame
    void Update()
    {
        obj = null;
        RaycastHit hitInfo;
        Ray ray = new Ray(this.transform.position, this.transform.up);
        Debug.DrawRay(this.transform.position, this.transform.up, Color.black);

        if (Physics.Raycast(ray, out hitInfo,0.1f))
        {
            Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject != null)
            {
                //Debug.Log(hitInfo.collider.name);
                obj = hitInfo.collider.transform.parent.gameObject;
                rb = obj.GetComponent<Rigidbody>();
                direction = target.transform.position - obj.transform.position;               
            } 
        }

        rotateSpeed = this.GetComponentInParent<AutoRotateDisk>().speed;
        FreezeObj();
    }

    private void FixedUpdate()
    {
        if(obj != null)
        {
            MoveObj(direction);
            RotateObj();
        }
    }

    private void MoveObj(Vector3 direction)
    {
        direction = new Vector3(direction.x, 0, direction.z);
        rb.velocity = direction * moveObjSpeed * Time.deltaTime;
    }

    private void RotateObj()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, rotateSpeed, 0));
        rb.MoveRotation(rotation * rb.rotation);
    }

    public void FreezeObj()
    {
        if (rotateSpeed > 0f)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
