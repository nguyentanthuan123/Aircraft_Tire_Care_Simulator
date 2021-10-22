using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BoardChangeScene : MonoBehaviour
{
    public GameObject boardScenePanel;
    // Start is called before the first frame update
    void Start()
    {
        //boardScenePanel.SetActive(false);
    }

    public void OpenBoardScene()
    {
        if(boardScenePanel != null)
        {
            bool isActive = boardScenePanel.activeSelf;
            boardScenePanel.SetActive(!isActive);
        }
    }
}
