using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwistObj : MonoBehaviour
{
    public enum GrownDirection { UpX, UpY, UpZ } // setting direction to twist out and in
    public GrownDirection grownDirection;
    public enum TwistDirection { Right, Left} // setting rotate dirction
    public TwistDirection twistDirection;
    public Transform placement;
    public int twistTime; // rotate lap
    public int layerNum, grabableLayerNum;
    public float delayTwistTime; // time delay per lap
    public float gettingTwistOutValue; // grown value
    public float speedGrown; // speed when growning
    public float speedRotate; // speed when rotating
    public float placeDistance; // distance to placement
    public bool isFinishedTwistOut, isFinishedTwistIn;

    [HideInInspector]
    public int twistRealtime;
    private Rigidbody rigi;
    private float xPlus = 0, yPlus = 0, zPlus = 0;
    private bool isTwistingIn = false, isTwistingOut = false;
    
    // Start is called before the first frame update
    void Start()
    {
        isFinishedTwistOut = true;
        isFinishedTwistIn = false;

        rigi = GetComponent<Rigidbody>();

        if (grownDirection == GrownDirection.UpX) xPlus = gettingTwistOutValue;

        if (grownDirection == GrownDirection.UpY) yPlus = gettingTwistOutValue;

        if (grownDirection == GrownDirection.UpZ) zPlus = gettingTwistOutValue;

        if(placement!= null)
        {
            if (Vector3.Distance(transform.position, placement.position) <= placeDistance)
            {
                MoveToPlacement();
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

        if (twistRealtime > twistTime)
        {
            isFinishedTwistOut = false;
            ResetTwist();
            isFinishedTwistIn = true;
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
    public void GoOut()
    {
        this.transform.Rotate(-xPlus * speedRotate, -yPlus * speedRotate, -zPlus * speedRotate);
        Vector3 startPos = transform.position;
        this.transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x + xPlus, startPos.y + yPlus, startPos.z + zPlus),speedGrown* Time.deltaTime);
    }
    public void GoIn()
    {
        this.transform.Rotate(xPlus * speedRotate, yPlus * speedRotate, zPlus * speedRotate);
        Vector3 startPos = transform.position;
        this.transform.position = Vector3.Lerp(startPos, new Vector3(startPos.x - xPlus, startPos.y - yPlus, startPos.z - zPlus), speedGrown * Time.deltaTime);
    }
}
