using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public bool isActive;
    public bool isFinished;
    [HideInInspector]
    public int taskID;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void FinishedTask()
    {
        isFinished = true;
        TaskManager.instance.UpdateTask(taskID);
    }
}
