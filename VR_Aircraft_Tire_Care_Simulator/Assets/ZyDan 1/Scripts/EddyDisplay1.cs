using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EddyDisplay1 : MonoBehaviour
{
    public GameObject display;

    [SerializeField] private Transform[] allDisplay;
    private bool toggle = false;
    //private bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        display = this.gameObject;
        allDisplay = display.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < allDisplay.Length; i++)
        {
            allDisplay[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StarAndShutDown()
    {
        toggle = !toggle;
        if (toggle)
        {
            display.SetActive(true);
            allDisplay[1].gameObject.SetActive(true);
            allDisplay[2].gameObject.SetActive(false);
            allDisplay[3].gameObject.SetActive(false);
        }
        else
        {
            display.SetActive(false);
        }
    }
}
