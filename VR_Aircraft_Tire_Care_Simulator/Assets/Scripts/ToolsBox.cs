using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsBox : MonoBehaviour
{
    #region singleton
    public static ToolsBox instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [System.Serializable]
    public class Tool
    {
        public string toolType;
        public GameObject toolPrefab;
        public Sprite toolIcon;
    }
    public List<Tool> tools;
    public GameObject toolSlot;
    public GameObject toolsGrid;
    public GameObject toolsListPanel;
    public GameObject boxhandle1;
    public GameObject boxhandle2;
    public Transform showToolPoint;
    private Dictionary<string, GameObject> poolDictionary;
    private Animator boxAnim;
    private Animator toolListPanelAnim;
    // Start is called before the first frame update
    void Start()
    {
        boxAnim = GetComponent<Animator>();
        toolListPanelAnim = toolsListPanel.GetComponent<Animator>();
        poolDictionary = new Dictionary<string, GameObject>();
        foreach( Tool tool in tools)
        {
            GameObject toolObj = Instantiate(tool.toolPrefab, transform.position, Quaternion.identity);
            GameObject toolSlotObj = Instantiate(toolSlot, toolsGrid.transform);
            toolSlotObj.GetComponent<Image>().sprite = tool.toolIcon;
            ToolSlot toolSlotScript = toolSlotObj.GetComponent<ToolSlot>();
            toolSlotScript.toolType = tool.toolType;
            toolSlotScript.setName();
            toolObj.SetActive(false);
            poolDictionary.Add(tool.toolType, toolObj);
        }
    }
    public void getToolBtn(string type)
    {
        foreach(Tool tool in tools)
        {
            if (type.Contains(tool.toolType))
            {
                GameObject obj = poolDictionary[type];
                obj.SetActive(true);
                obj.transform.position = showToolPoint.position;
                obj.transform.rotation = showToolPoint.rotation;
            }
        }
    }
    public void OpenToolsBoxBtn()
    {
        boxAnim.SetBool("IsOpen",true);
        toolListPanelAnim.SetBool("isActive", true);
        boxhandle1.SetActive(false);
        boxhandle2.SetActive(false);
    }
    public void CloseToolsBoxBtn()
    {
        boxAnim.SetBool("IsOpen", false);
        toolListPanelAnim.SetBool("isActive", false);
        //toolsListPanel.SetActive(false);
    }
    public void OpenToolsListPanel()
    {
        toolsListPanel.SetActive(true);
    }
    public void ActiveHandle()
    {
        boxhandle1.SetActive(true);
        boxhandle2.SetActive(true);
    }
}
