using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceJack1 : MonoBehaviour
{
    [SerializeField] private float dis; //y axis distance from body positon to ground by 
    [SerializeField] private float radius; // y axis distance ground check point - ground; 0.088 is size of body box collider by y axis

    public Transform body;
    public Transform handle;
    public Transform cylinder1;
    public Transform dome;
    public Transform groundCheck;
    public GameObject sideViewDisplay;
    public GameObject ruler;
    public bool isPlaced;
    public LayerMask groundLayerMask;

  
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (placeJack())
        {
            // make sure the jack in right position and on ground
            if (IsGrounded())
            {
                isPlaced = true;
            }
            else
            {
                isPlaced = false;
            }
            ruler.SetActive(true);
            sideViewDisplay.SetActive(true);
        }
        else
        {
            sideViewDisplay.SetActive(false);
            ruler.SetActive(false);
            isPlaced = false;
        }
    }

    private bool placeJack()
    {
        //measure distance between the jack and the contact dome
        float distanceCheck = Vector3.Distance(dome.position, cylinder1.position);
        Vector3 direction = dome.position - cylinder1.position;

        //help to move the jack when it closes to the contact dome
        if (distanceCheck <= 0.25f)
        {
            direction = new Vector3(direction.x, 0f, direction.z);
            body.GetComponent<Rigidbody>().MovePosition(body.position + direction *Time.deltaTime);
            return true;
        }
        return false;
    }

    //dectect if the jack is on ground or not
    private bool IsGrounded()
    {
        RaycastHit hitInfo;
        
        Ray ray = new Ray(body.transform.position, -body.transform.up);
        if(Physics.Raycast(ray, out hitInfo, dis, groundLayerMask) && Physics.CheckSphere(groundCheck.position, radius, groundLayerMask))
        {
            return true;
        }
        return false;

        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }
}

