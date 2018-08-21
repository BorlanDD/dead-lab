using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    public Task currentTask { get; private set; }

    public bool taskHintShowing { get; private set; }

    //Showing task hint for taskHintShowingTime
    public bool taskHintShowingWhile;
    public float taskHintShowingTime;
    public float taskHintShowTime;

    private static TasksManager tasksManager;
    public static TasksManager GetInstance()
    {
        return tasksManager;
    }

    void Awake()
    {
        tasksManager = this;

        taskHintShowing = false;
        taskHintShowingWhile = false;
        taskHintShowingTime = 0;
        taskHintShowTime = 5;
    }

    void Update()
    {
        if (taskHintShowingWhile)
        {
            taskHintShowingTime += Time.deltaTime;
            if (taskHintShowingTime >= taskHintShowTime)
            {
                taskHintShowingWhile = false;
                taskHintShowingTime = 0;
                UserInterface.GetInstance().HideTaskHint();
            }
        }
    }

    void Start()
    {
        SetTask(new AwakeTask());
    }

    public void SetTask(Task task)
    {
        currentTask = task;

        taskHintShowingWhile = true;
        ShowTaskHint();
    }

    public void ShowTaskHint(bool state)
    {
        if (state)
        {
            if (taskHintShowingWhile)
            {
                taskHintShowingWhile = false;
                taskHintShowingTime = 0;
            }
            taskHintShowing = true;
            ShowTaskHint();
        }
        else
        {
            taskHintShowing = false;
            UserInterface.GetInstance().HideTaskHint();
        }
    }

    private void ShowTaskHint()
    {
        string description;
        if (currentTask != null)
        {
            description = currentTask.description;
        }
        else
        {
            description = "No active task";
        }

        UserInterface.GetInstance().ShowTaskHint(description);
    }
}
