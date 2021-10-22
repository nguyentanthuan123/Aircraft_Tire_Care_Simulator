using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    #region singleton
    public static TaskManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion
    [System.Serializable]
    public class taskObj
    {
        public string taskName;
        public GameObject objHasTask;
        public int taskOrder;
        public GameObject taskUI;
    }
    public List<taskObj> taskObjsList;
    public GameObject taskListUI;
    public GameObject taskUIPrefab;
    public int taskActiveNum;
    // Start is called before the first frame update
    void Start()
    {
        SortTask();
        ResetTask();
        ShowTask();
    }
    public void ResetTask()
    {
        foreach(taskObj taskObjNode in taskObjsList)
        {
            taskObjNode.taskUI = Instantiate(taskUIPrefab, taskListUI.transform);
            taskObjNode.taskUI.GetComponent<Text>().text = taskObjNode.taskName;
            taskObjNode.taskUI.SetActive(false);
        }
    }
    private void SortTask()
    {
        if (taskObjsList.Count < 1) return;

        taskObj temp;
        for(int i = 0; i < taskObjsList.Count-1; i++)
        {
            if(taskObjsList[i].taskOrder > taskObjsList[i+1].taskOrder)
            {
                temp = taskObjsList[i];
                taskObjsList[i] = taskObjsList[i + 1];
                taskObjsList[i + 1] = temp;
            }
            taskObjsList[i].objHasTask.GetComponent<Task>().taskID = i;
        }
        taskActiveNum = taskObjsList[0].taskOrder;
    }
    public void ShowTask()
    {
        foreach(taskObj taskObjNode in taskObjsList)
        {
            if(taskObjNode.taskOrder == taskActiveNum)
            {
                taskObjNode.taskUI.SetActive(true);
                taskObjNode.objHasTask.transform.tag = "isActiveTask";
            }
        }
    }
    public void UpdateTask(int taskID)
    {
        Destroy(taskObjsList[taskID].taskUI);
        taskObjsList.Remove(taskObjsList[taskID]);
        SortTask();
        ShowTask();
    }
}
