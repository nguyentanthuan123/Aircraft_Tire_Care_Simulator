using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jack : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float dis;

    public Transform body;
    public Transform handle;
    public Transform cylinder1;
    public Transform cylinder2;
    public Transform dome;

    private float initialAngle;
    private float currentAngle;
    private float distanceTravelled = 0;
  
    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = body.position - handle.position;
        initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = body.position - handle.position;
        currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        distanceTravelled = Vector3.Distance(new Vector3(0, body.position.y, 0), new Vector3(0, dome.position.y, 0));

        if (placeJack())
        {
            if(Mathf.Round(body.transform.up.y * 1000.0f) * 0.001f >= 1f && IsGrounded())
            {
                //Debug.Log("hit");
                body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            moveCylinder();
            ResetInitialAngel();
        }
    }

    void ResetInitialAngel()
    {
        if (currentAngle > initialAngle)
        {
            initialAngle = currentAngle;
        }
    }

    bool placeJack()
    {
        float distanceCheck = Vector3.Distance(dome.position, cylinder1.position);
        Vector3 direction = dome.position - cylinder1.position;
        //Debug.Log(distanceCheck);
        if(distanceCheck <= 0.2f)
        {
            cylinder1.parent.Translate(direction.x * Time.deltaTime, 0, direction.z * Time.deltaTime);

            if(distanceCheck < 0.16f)
            {
                return true;
            }
        }
        return false;
    }

    void moveCylinder()
    {
        
        if (initialAngle - currentAngle > 1)
        {
            if (distanceTravelled < 0.2f)
            {
                cylinder1.position = new Vector3(cylinder1.position.x, cylinder1.position.y + 0.03f * Time.deltaTime, cylinder1.position.z);
                initialAngle = currentAngle;
            }
            if (distanceTravelled >= 0.2f && distanceTravelled < 0.35f)
            {
                cylinder2.position = new Vector3(cylinder2.position.x, cylinder2.position.y + 0.03f * Time.deltaTime, cylinder2.position.z);
                cylinder1.position = new Vector3(cylinder1.position.x, cylinder1.position.y + 0.03f * Time.deltaTime, cylinder1.position.z);
                initialAngle = currentAngle;
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(body.transform.position, -body.transform.up);
        //Debug.DrawRay(body.transform.position, -body.transform.up, Color.green);
        if(Physics.Raycast(ray, out hitInfo, dis))
        {
            //Debug.Log("grounded");
            return true;
        }
        return false;
    }
}

