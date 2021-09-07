using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
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
        SceneManager.LoadScene("Parking 2");
    }

    public void GoToCockpit()
    {
        SceneManager.LoadScene("Cockpit 2");
    }
}
