using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCastTest : MonoBehaviour
{
    private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider = this.GetComponent<BoxCollider>();
        IsGrounded();
    }

    private bool IsGrounded()
    {
        RaycastHit hitInfo;
        if (Physics.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, Vector3.down, out hitInfo))
        {
            Debug.Log(hitInfo.collider.gameObject);
            if (hitInfo.collider.gameObject.layer.Equals("Ground"))
            {
                return true;
            }
        }
        return false;
    }
}
