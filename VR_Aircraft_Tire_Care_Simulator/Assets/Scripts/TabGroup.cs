using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TabGroup : MonoBehaviour
{
    public GameObject menuCanvas;
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;
    public List<GameObject> objectsToSwap;
    private int sceneNum;

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null || button != selectedTab)
        {
            button.backGround.sprite = tabHover;
        }    
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected (TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.backGround.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for(int i=0; i<objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }    
            else
            {
                objectsToSwap[i].SetActive(false);
            }    
        }


    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if(selectedTab != null && button == selectedTab) { continue; }
            button.backGround.sprite = tabIdle;
        }    
    }

    public void GoBtn()
    {
        if (!selectedTab) return;

        sceneNum = selectedTab.transform.GetSiblingIndex();
        SceneManager.LoadScene(sceneNum);
    }
    public void exitBtn()
    {
        menuCanvas.SetActive(false);
    }
}
