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
    private Dictionary<string, GameObject> poolDictionary;
    public Transform showToolPoint;
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, GameObject>();
        foreach( Tool tool in tools)
        {
            GameObject toolObj = Instantiate(tool.toolPrefab, transform.position, Quaternion.identity);
            GameObject toolSlotObj = Instantiate(toolSlot, toolsGrid.transform);
            toolSlotObj.GetComponent<Image>().sprite = tool.toolIcon;
            toolSlotObj.GetComponent<ToolSlot>().toolType = tool.toolType;
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
}
