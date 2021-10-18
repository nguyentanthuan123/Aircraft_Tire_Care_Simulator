using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Probe1 : MonoBehaviour
{
    public GameObject ProbeTip;
    public GameObject Display;

    [SerializeField] private Transform[] allDisplay;

    // Start is called before the first frame update
    void Start()
    {
        allDisplay = Display.transform.GetComponentsInChildren<Transform>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {   
        if (collision.collider.tag.Equals("Smooth"))
        {
            allDisplay[1].gameObject.SetActive(false);
            allDisplay[2].gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("crack"))
        {
            allDisplay[1].gameObject.SetActive(false);
            allDisplay[2].gameObject.SetActive(false);
            allDisplay[3].gameObject.SetActive(true);
        }
    }
}
