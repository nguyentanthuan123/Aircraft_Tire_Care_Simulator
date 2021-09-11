using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCam;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        LookAtRotation();
    }
    private void LookAtRotation()
    {
        if (mainCam)
        {
            Vector3 cameraDirection = (transform.position - mainCam.transform.position).normalized;
            Quaternion rotateDirection = Quaternion.LookRotation(new Vector3(cameraDirection.x, 0, cameraDirection.z));
            transform.rotation = Quaternion.Slerp( transform.rotation,rotateDirection , Time.deltaTime * 6f);
        }
    } 
}
