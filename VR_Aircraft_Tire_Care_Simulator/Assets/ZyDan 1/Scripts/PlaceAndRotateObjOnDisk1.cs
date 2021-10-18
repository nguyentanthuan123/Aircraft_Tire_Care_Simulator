using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceAndRotateObjOnDisk1 : MonoBehaviour
{
    public Transform target;

    private GameObject obj;
    private Rigidbody rb;
    private Vector3 direction;
    [SerializeField] private float moveObjSpeed;
    [SerializeField] private float disCheck;
    private float rotateSpeed;
    private bool objHasMoved;


    // Start is called before the first frame update
    void Start()
    {
        target = transform.Find("Center");
    }

    // Update is called once per frame
    void Update()
    {
        obj = null;
        RaycastHit hitInfo;
        Ray ray = new Ray(this.transform.position, this.transform.up);
        Debug.DrawRay(this.transform.position, this.transform.up, Color.black);

        if (Physics.Raycast(ray, out hitInfo,disCheck))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject != null)
            {
                //Debug.Log(hitInfo.collider.name);
                obj = hitInfo.collider.transform.parent.gameObject;
                rb = obj.GetComponent<Rigidbody>();
                direction = target.transform.position - obj.transform.position;               
            } 
        }

        rotateSpeed = this.GetComponentInParent<AutoRotateDisk1>().speed;
    }

    private void FixedUpdate()
    {
        MoveObj(direction);
        if (direction.magnitude < 0.076f && obj != null)
        {
            RotateObj();
            FreezeObj();
        }
    }

    private void MoveObj(Vector3 direction)
    {
        if (obj != null && obj.transform.up.y == 1f && rotateSpeed == 0f)
        {
            direction = new Vector3(direction.x, 0, direction.z);
            rb.velocity = direction * moveObjSpeed * Time.fixedDeltaTime;
        }
    }

    private void RotateObj()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, rotateSpeed, 0));
        rb.MoveRotation(rotation * rb.rotation);
    }

    public void FreezeObj()
    {
        if (rotateSpeed > 0f && obj.transform.up.y == 1f && obj !=null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
