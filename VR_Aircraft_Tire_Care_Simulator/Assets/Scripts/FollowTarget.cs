using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    public GameObject target;
    public float speed;
    public bool freezeX, freezeY, freezeZ;
    private int x, y, z;
    private float xThis, yThis, zThis;
    // Start is called before the first frame update
    void Start()
    {
        if (freezeX)
        {
            x = 0;
            xThis = transform.position.x;
        }
        else
        {
            x = 1;
            xThis = 0;
        }
        if (freezeY)
        {
            y = 0;
            yThis = transform.position.y;
        }
        else
        {
            y = 1;
            yThis = 0;
        }
        if (freezeZ)
        {
            z = 0;
            zThis = transform.position.z;
        }
        else
        {
            z = 1;
            zThis = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 followPoint = target.transform.position;
        transform.position = Vector3.Lerp(transform.position,new Vector3(followPoint.x * x + xThis, followPoint.y * y + yThis,followPoint.z * z +zThis) , speed * Time.deltaTime);
    }
}
