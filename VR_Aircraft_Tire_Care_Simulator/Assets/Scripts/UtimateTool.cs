using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtimateTool : MonoBehaviour
{
    // obj use script must be obj have collider for grab
    public enum moveDirectionEnum { moveX,moveY,moveZ} // State of direction to move tool
    public moveDirectionEnum moveDirection;
    public LayerMask targetUseToolLayer; // Layer of obj use tool
    public Transform connectPoint; // position to connect obj use tool - connectPoint must be zero rotation relative to use script obj
    public float connectDistance; // distance to connect - use tool
    public float jointLimitMin, jointLimitMax; // limit of hinge joint - move hand tool
    public int connectMass; // mass of connect joint when connection active
    public int toolMass; // mass of tool when connection active
    public int GrabToolMass; // mass of tool when deactive tool

    private GameObject connectJoint;
    private GameObject targetUseTool;
    private Transform useToolObjPoint;
    private Vector3 localConnectPointPos;
    private Quaternion localConnectPointRot;
    private Rigidbody rigiTool;
    private HingeJoint joint;
    private FixedJoint jointToConnectPoint;
    private DetectiveJointBreak jointBreakScript;
    private TwistObj twistObjScript;
    private float angleRotateValue;
    private int xAxis = 0, yAxis = 0, zAxis = 0;
    private int sideValue; // 1 = Right, 0 = Left;
    private bool isActive = false, isTwistTarget = true, isSwistSideFirst = true;
    // Start is called before the first frame update
    void Start()
    {
        // save default local position and rotation of connectPoint
        localConnectPointPos = connectPoint.localPosition;
        localConnectPointRot = connectPoint.localRotation;

        // set mass when tool deactivate
        rigiTool = this.GetComponent<Rigidbody>();
        rigiTool.mass = GrabToolMass;

        // set direction move tool sate
        if (moveDirection == moveDirectionEnum.moveX)
        {
            xAxis = 1;
        }
        if (moveDirection == moveDirectionEnum.moveY)
        {
            yAxis = 1;
        }
        if (moveDirection == moveDirectionEnum.moveZ)
        {
            zAxis = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (joint.angle  > jointLimitMax - 1) //check angle go to hinge joint max limit or min limit
            {
                if (sideValue == 1 || isSwistSideFirst) //check sideValue for 1 = Max or 0 = min or is the first time swist side to ignore sideValue
                {
                    // must be in order
                    sideValue = 1; // set sideValue for the first time swist
                    TwistingByTool();
                    isSwistSideFirst = false;
                    sideValue = 0; // set sideValue to ignore statements if next swist time is the same side
                }
            }
            if (joint.angle < jointLimitMin + 1)
            {
                if (sideValue == 0 || isSwistSideFirst)
                {
                    sideValue = 0;
                    TwistingByTool();
                    isSwistSideFirst = false;
                    sideValue = 1;
                }
            }
        }

    }
    public void ActivateTool()
    {
        if (isActive) return;

        // find target use tool layer in connect distance
        Collider[] targetCollider = Physics.OverlapSphere(connectPoint.position, connectDistance, targetUseToolLayer);
        if (targetCollider.Length < 1) return;

        // set target and check if it has TwistObj Script
        targetUseTool = targetCollider[0].gameObject;
        if (!IsActiveTask(targetUseTool)) return;

        twistObjScript = targetUseTool.GetComponent<TwistObj>();
        if (!twistObjScript) return;

        // find obj use tool connection point - use tool objs have to have a child tranform with tag "UseToolPoint" and have vector X Point Backward
        Transform[] useToolObjParts = targetUseTool.GetComponentsInChildren<Transform>();
        foreach(Transform part in useToolObjParts)
        {
            if (part.gameObject.transform.tag.Equals("UseToolPoint"))
            {
                useToolObjPoint = part;
            }
        }
        if (useToolObjPoint == null) return;

        // move to obj use tool position have connection point
        connectPoint.SetParent(null);
        connectPoint.localScale = Vector3.one;
        transform.SetParent(connectPoint);
        connectPoint.position = useToolObjPoint.position;
        connectPoint.rotation = useToolObjPoint.rotation; // connectPoint must has same rotation as tool to make sure direction are right

        // Init empty gameobject for connect joint
        connectJoint = new GameObject();
        connectJoint.transform.SetParent(this.transform);
        connectJoint.transform.position = connectPoint.position;
        connectJoint.transform.rotation = connectPoint.rotation; // vector z of connectPoint has to look backward to tool tail and x has to point up

        // Add joint for tool
        var rigiJoint = connectJoint.AddComponent<Rigidbody>();
        joint = connectJoint.AddComponent<HingeJoint>();
        joint.connectedBody = this.GetComponent<Rigidbody>();
        rigiJoint.mass = connectMass;

        // Add joint for obj use tool
        jointToConnectPoint = connectJoint.AddComponent<FixedJoint>();
        jointToConnectPoint.connectedBody = targetUseTool.GetComponent<Rigidbody>();

        // Setting hinge joint limit
        joint.useLimits = true;
        var limit = joint.limits;
        limit.min = jointLimitMin;
        limit.max = jointLimitMax;

        joint.limits = limit;

        var axis = joint.axis;
        axis.x = xAxis;
        axis.y = yAxis;
        axis.z = zAxis;
        joint.axis = axis;

        rigiTool.mass = toolMass; // set heavy mass to stable hand

        targetUseTool.GetComponent<Collider>().enabled = false;

        isActive = true;
    }
    public void DeactivateTool()
    {
        if (!isActive) return;

        if (connectJoint != null) Destroy(connectJoint);

        if (targetUseTool != null)
        {
            targetUseTool.GetComponent<Collider>().enabled = true;
            twistObjScript = null;
            targetUseTool = null;
        }
        if(useToolObjPoint != null)
        {
            useToolObjPoint = null;
        }
        // set tool obj back to normal
        this.transform.SetParent(null);
        connectPoint.SetParent(transform);
        connectPoint.localPosition = localConnectPointPos;
        connectPoint.localRotation = localConnectPointRot;
        rigiTool.mass = GrabToolMass;

        isSwistSideFirst = true;
        isTwistTarget = true;
        isActive = false;
    }
    private void TwistingByTool()
    {
        if (!isTwistTarget) // allow twist when both side are touched
        {
            isTwistTarget = true;
        }
        else
        {
            if (twistObjScript != null)
            {
                if(twistObjScript.twistDirection == TwistObj.TwistDirection.Right) // check direction in twist obj - Max = right, min = left
                {
                    if (sideValue == 1) 
                    { 
                        twistObjScript.GettingTwistIn();
                        if (twistObjScript.isFinishedTwistIn) return;
                    }
                    else
                    {
                        twistObjScript.GettingTwistOut();
                        if (twistObjScript.isFinishedTwistOut) return;
                    }
                }
                else
                {
                    if (sideValue == 0)
                    {
                        twistObjScript.GettingTwistIn();
                        if (twistObjScript.isFinishedTwistIn) return;
                    }
                    else
                    {
                        twistObjScript.GettingTwistOut();
                        if (twistObjScript.isFinishedTwistOut) return;
                    }
                }
                isTwistTarget = false;
            }
        }
    }
    private bool IsActiveTask(GameObject obj)
    {
        if (obj.transform.tag.Equals("isActiveTask")) return true;

        return false;
    }
}
