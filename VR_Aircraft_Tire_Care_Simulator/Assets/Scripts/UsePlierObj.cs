using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePlierObj : MonoBehaviour
{
    private void OnJointBreak(float breakForce)
    {
        GetComponent<Task>().FinishedTask();
    }
}
