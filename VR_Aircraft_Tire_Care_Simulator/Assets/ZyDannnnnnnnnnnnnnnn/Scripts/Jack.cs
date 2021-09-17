using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jack : MonoBehaviour
{
    public Transform body;
    public Transform handle;
    public Transform cylinder1;
    public Transform cylinder2;

    private float  initialAngle;
    private float currentAngle;
    private float distanceTravelled = 0;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = body.position - handle.position;
        initialAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lastPosition = cylinder1.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = body.position - handle.position;
        currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        distanceTravelled += Vector3.Distance(new Vector3(0, cylinder1.position.y, 0), new Vector3(0, lastPosition.y, 0));
        lastPosition = cylinder1.position;
        //Debug.Log(distanceTravelled);

        if (initialAngle - currentAngle > 1 && currentAngle != initialAngle)
        {
            if(distanceTravelled < 0.2f)
            {
                cylinder1.position = new Vector3(cylinder1.position.x, cylinder1.position.y + 0.01f * Time.deltaTime, cylinder1.position.z);
                initialAngle = currentAngle;
            }
            if (distanceTravelled >= 0.2f && distanceTravelled < 0.4f)
            { 
                cylinder2.position = new Vector3(cylinder2.position.x, cylinder2.position.y + 0.01f * Time.deltaTime, cylinder2.position.z);
                cylinder1.position = new Vector3(cylinder1.position.x, cylinder1.position.y + 0.01f * Time.deltaTime, cylinder1.position.z);
                initialAngle = currentAngle;
            }
        }
        ResetInitialAngel();
    }

    void ResetInitialAngel()
    {
        if (currentAngle > initialAngle)
        {
            initialAngle = currentAngle;
        }
    }
}

