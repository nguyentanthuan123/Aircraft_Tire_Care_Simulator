using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Handle : MonoBehaviour
{
    public UnityEvent OnPressed, OnReleased;
    public Transform handle;
    public Transform body;

    private float initialAngle;
    private float currentAngle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentAngle = transform.eulerAngles.z;

        //activate cylinder when handle is moving down
        if(initialAngle - currentAngle > 1f)
        {
            press();
            initialAngle = currentAngle;
        }

        ResetInitialAngel();
    }

    void ResetInitialAngel() // reset angle when current angle > initial angle
    {
        if (currentAngle > initialAngle)
        {
            initialAngle = currentAngle;
        }
    }

    private void press()
    {

        OnPressed.Invoke();
    }

    private void release()
    {
        OnReleased.Invoke();
    }

}
