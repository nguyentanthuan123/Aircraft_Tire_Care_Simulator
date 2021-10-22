using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region singleton
    public static SceneController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public GameObject menuPanel;
    public GameObject toolListPanel;
    public GameObject taskPanel;

    public void GoToNLG()
    {
        SceneManager.LoadScene("Parking 1");
    }

    public void GoToCockpit()
    {
        SceneManager.LoadScene("Cockpit 1");
    }

    public void GoToWorkShop()
    {
        SceneManager.LoadScene("Workshop 1");
    }
    public void OpenToolList()
    {
        toolListPanel.SetActive(true);
    }
    public void CloseToolList()
    {
        toolListPanel.SetActive(false);
    }
    public void OpenTaskPanel()
    {
        if(taskPanel != null)
        {
            bool isActive = taskPanel.activeSelf;
            taskPanel.SetActive(!isActive);
        }
    }
    public void OpenMenu()
    {
        if (menuPanel != null)
        {
            bool isActive = menuPanel.activeSelf;
            menuPanel.SetActive(!isActive);
        }
    }
}
