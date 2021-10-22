using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plier : MonoBehaviour
{
    public LayerMask usePlierLayer;
    public float distanceAction;

    private Animator plierAnim;
    private GameObject usePlierObj;
    private Joint connectJoint;

    // Start is called before the first frame update
    void Start()
    {
        plierAnim = GetComponent<Animator>();
    }
 
    public void usingPlier()
    {
        if (usePlierObj != null) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, distanceAction, usePlierLayer);
        if (colliders.Length < 1) return;

        usePlierObj = colliders[0].gameObject;
        if (!usePlierObj.transform.tag.Equals("isActiveTask")) return;

        connectJoint = usePlierObj.AddComponent<FixedJoint>();
        var Rigi = GetComponent<Rigidbody>();
        connectJoint.connectedBody = Rigi;
        connectJoint.breakForce = float.PositiveInfinity;
        connectJoint.breakTorque = float.PositiveInfinity;

        plierAnim.SetBool("usingPlier", true);
    }
    public void releasePlier()
    {
        if(usePlierObj != null)
        {
            usePlierObj = null;
        }
        if(connectJoint != null)
        {
            Destroy(connectJoint);
        }

        plierAnim.SetBool("usingPlier", false);
    }
}
