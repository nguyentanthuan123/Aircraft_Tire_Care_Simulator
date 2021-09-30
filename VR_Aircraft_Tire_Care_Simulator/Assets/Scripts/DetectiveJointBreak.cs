using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectiveJointBreak : MonoBehaviour
{
    public bool isBreak = false;
    private void OnJointBreak(float breakForce)
    {
        isBreak = true;
    }
}
