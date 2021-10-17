using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxCastTest : MonoBehaviour
{
    private BoxCollider boxCollider;
    public float maxDistance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        boxCollider = this.GetComponent<BoxCollider>();

        
    }

    private void OnDrawGizmos()
    {
        
        RaycastHit hitInfo;
        if (Physics.BoxCast(boxCollider.bounds.center, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down, out hitInfo, Quaternion.identity, maxDistance))
        {
            Gizmos.DrawWireCube(boxCollider.bounds.center - transform.up * hitInfo.distance, boxCollider.bounds.size);
            Debug.Log(hitInfo.collider);

        }
        else
        {
            Debug.Log("off ground");
        }

    }

}
