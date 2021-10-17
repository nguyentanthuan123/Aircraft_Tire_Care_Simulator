using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rbmovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform target;
    public float speed;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = target.position - this.gameObject.transform.position;
    }

    private void FixedUpdate()
    {
        MoveCube(direction);
    }
    private void MoveCube(Vector3 direction)
    {
        direction = new Vector3(direction.x, 0, direction.z);
        //rb.AddForce(direction * Time.deltaTime* speed);
        //rb.velocity = direction * speed * Time.deltaTime;
        rb.MovePosition(transform.position + direction * Time.deltaTime * speed);
    }
}
