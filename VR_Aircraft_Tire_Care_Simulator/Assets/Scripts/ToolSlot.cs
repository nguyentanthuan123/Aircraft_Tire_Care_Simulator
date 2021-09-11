using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolSlot : MonoBehaviour
{
    public string toolType;
    public void getTool()
    {
        ToolsBox.instance.getToolBtn(toolType);
    }
}
