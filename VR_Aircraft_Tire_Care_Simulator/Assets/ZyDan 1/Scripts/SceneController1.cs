using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController1 : MonoBehaviour
{
    public GameObject toolListPanel;
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

    public void GoToWorkShop()
    {
        SceneManager.LoadScene("Workshop 2");
    }
    public void OpenToolList()
    {
        toolListPanel.SetActive(true);
    }
    public void CloseToolList()
    {
        toolListPanel.SetActive(false);
    }
}
