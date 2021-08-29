using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNLG()
    {
        SceneManager.LoadScene("Parking 1");
    }

    public void GoToCockpit()
    {
        SceneManager.LoadScene("Cockpit 1");
    }
}
