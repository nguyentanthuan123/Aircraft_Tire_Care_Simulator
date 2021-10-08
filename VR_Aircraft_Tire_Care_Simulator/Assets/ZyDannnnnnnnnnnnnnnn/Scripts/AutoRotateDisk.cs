using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateDisk : MonoBehaviour
{
    public Transform disk;
    public float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    private bool toggle;
    private Component otherGO;
    
    // Start is called before the first frame update
    void Start()
    {
        toggle = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (toggle)
        {
            SpeedUp();
        }
        else
        {
            SpeedDown();
        }
    }
    
    private void SpeedUp()
    {
        if (speed < maxSpeed)
        {
            speed = speed + acceleration * Time.deltaTime;
        }
        disk.Rotate(0, speed, 0);
    }

    private void SpeedDown()
    {
        if (speed > 0)
        {
            speed = speed - acceleration * Time.deltaTime * 5;
        }
        else if (speed < 0)
        {
            speed = 0;
        }
        disk.Rotate(0, speed, 0);
    }

    public void StartStop()
    {
        toggle = !toggle;
    }
}

