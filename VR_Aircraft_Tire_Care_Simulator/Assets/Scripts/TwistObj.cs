using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistObj : MonoBehaviour
{
    public enum GrownDirection { UpX, UpY, UpZ } // setting direction to twist out and in - transform of parent must be same with obj
    public GrownDirection grownDirection;
    public enum RotateDirection { rX,rY,rZ} // setting rotate direction 
    public RotateDirection rotateDirection;
    public enum TwistDirection { Right, Left} // setting rotate dirction
    public TwistDirection twistDirection;
    public Transform placement;
    public Transform standalTransform;
    public int twistTime; // rotate lap
    public int layerNum, grabableLayerNum;
    public float lapPerSecond; // if lapPerSecond is negative number obj will twist reverse
    public float delayTwistTime; // time delay per lap
    public float gettingTwistValue; // grown value - gettingTwistValue must be same negative or positive as lapPerSecond
    public float speedGrown; // speed when growning
    public float placeDistance; // distance to placement

    [HideInInspector]
    public int twistRealtime;
    [HideInInspector]
    public bool isFinishedTwistOut, isFinishedTwistIn;
    private Rigidbody rigi;
    private float xPlus = 0, yPlus = 0, zPlus = 0;
    private float speedRotate = 54f; // speed when rotating -> 54 * 0.1(rotate) == 1 lap / 1s
    private float xRotate = 0, yRotate = 0, zRotate = 0;
    private bool isTwistingIn = false, isTwistingOut = false;
    
    // Start is called before the first frame update
    void Start()
    {
        speedRotate = speedRotate * lapPerSecond;
        isFinishedTwistOut = true;
        isFinishedTwistIn = false;

        rigi = GetComponent<Rigidbody>();

        if (grownDirection == GrownDirection.UpX) xPlus = gettingTwistValue;

        if (grownDirection == GrownDirection.UpY) yPlus = gettingTwistValue;

        if (grownDirection == GrownDirection.UpZ) zPlus = gettingTwistValue;

        if (rotateDirection == RotateDirection.rX) xRotate = 0.1f; 
                                                            
        if (rotateDirection == RotateDirection.rY) yRotate = 0.1f;
                                                             
        if (rotateDirection == RotateDirection.rZ) zRotate = 0.1f;

        if (placement!= null)
        {
            if (Vector3.Distance(transform.position, placement.position) <= placeDistance)
            {
                //MoveToPlacement();
                ResetTwist();
                StandalState();
            }
            else
            {
                FinishedTwistOut();
            }
        }
    }
    private void Update()
    {
        if (isTwistingIn)
        {
            GoIn();
        }
        if (isTwistingOut)
        {
            GoOut();
        }
    }
    public void MoveToPlacement()
    {
        if (placement == null) return;

        if(Vector3.Distance(transform.position,placement.position)<= placeDistance)
        {
            transform.position = placement.position;
            transform.rotation = placement.rotation;
            ResetTwist();
        }
    }
    public void GettingTwistIn()
    {
        if (isFinishedTwistIn) return;

        if (isTwistingIn) return;

        if (twistRealtime >= twistTime)
        {
            isFinishedTwistOut = false;
            ResetTwist();
            StandalState();
            return;
        }
        twistRealtime++;
        isTwistingIn = true;
        StartCoroutine(TwistDelay());
    }
    public void GettingTwistOut()
    {
        if (isFinishedTwistOut) return;

        if (isTwistingOut) return;

        if (twistRealtime <= 0)
        {
            FinishedTwistOut();
            return;
        }
        twistRealtime--;
        isTwistingOut = true;
        StartCoroutine(TwistDelay());
    }
    IEnumerator TwistDelay()
    {
        yield return new WaitForSeconds(delayTwistTime);
        isTwistingIn = false;
        isTwistingOut = false;
    }
    public void FinishedTwistOut()
    {
        if (isFinishedTwistOut) return;

        this.gameObject.layer = grabableLayerNum; 
        rigi.isKinematic = false;
        isFinishedTwistOut = true;
    }
    public void ResetTwist()
    {
        this.gameObject.layer = layerNum;
        rigi.isKinematic = true;
        if (isFinishedTwistOut)
        {
            twistRealtime = 0;
        }
        else
        {
            twistRealtime = twistTime;
        }
        isFinishedTwistIn = false;
    }
    public void StandalState()
    {
        isFinishedTwistOut = false;
        isFinishedTwistIn = true;
        twistRealtime = twistTime;
        transform.position = standalTransform.position;
        transform.rotation = standalTransform.rotation;
    }
    public void GoOut()
    {
        this.transform.Rotate(-xRotate * speedRotate, -yRotate * speedRotate, -zRotate * speedRotate,Space.Self);
        Vector3 startPos = transform.localPosition;
        this.transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x + xPlus, startPos.y + yPlus, startPos.z + zPlus),speedGrown* Time.deltaTime);
    }
    public void GoIn()
    {
        this.transform.Rotate(xRotate * speedRotate, yRotate * speedRotate, zRotate * speedRotate,Space.Self);
        Vector3 startPos = transform.localPosition;
        this.transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x - xPlus, startPos.y - yPlus, startPos.z - zPlus), speedGrown * Time.deltaTime);
    }
}
