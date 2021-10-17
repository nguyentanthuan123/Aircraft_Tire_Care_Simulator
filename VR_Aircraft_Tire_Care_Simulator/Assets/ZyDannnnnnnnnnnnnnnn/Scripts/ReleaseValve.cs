using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReleaseValve : MonoBehaviour
{
    public Transform valve;
    public Transform pulledPosition;
    public Transform releasedPosition;
    public float force;

    public UnityEvent OnPulled, OnReleased;

    public bool isPressed;


    // Start is called before the first frame update
    void Start()
    {
        isPressed = true;
    }

    // Update is called once per frame
    void Update()
    {
        //make sure that vavle only moves along y axis
        valve.localPosition = new Vector3(0f, valve.localPosition.y, 0f);
        valve.localRotation = Quaternion.identity;

        //limit valve movement
        if (valve.localPosition.y >= pulledPosition.localPosition.y )
        {
            valve.position = new Vector3(pulledPosition.position.x, pulledPosition.position.y, pulledPosition.position.z);
        }
        if(valve.localPosition.y <= releasedPosition.localPosition.y)
        {
            valve.position = new Vector3(releasedPosition.position.x, releasedPosition.position.y, releasedPosition.position.z);
        }
        else
        {
            //make vavle moves back to start position smoothly
            valve.gameObject.GetComponent<Rigidbody>().AddForce(-valve.up * force * Time.deltaTime);
        }

        //activate and deactivate valve
        if (valve.position.y >= (releasedPosition.position.y + pulledPosition.position.y) * 0.5)
        {
            pulled();
        }
        if (valve.position.y < (releasedPosition.position.y + pulledPosition.position.y) * 0.5 && isPressed)
        {
            released();
        }

    }

    private void pulled()
    {
        isPressed = true;
        OnPulled.Invoke();
    }

    private void released()
    {
        isPressed = false;
        OnReleased.Invoke();
    }


}