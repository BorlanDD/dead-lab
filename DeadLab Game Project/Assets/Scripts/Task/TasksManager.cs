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
    }

    public void SetTask(Task task)
    {
        currentTask = task;

        OnTaskUpdated();
    }

    public void OnTaskUpdated()
    {
        taskHintShowingWhile = true;
        taskHintShowingTime = 0;
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
            IList<Task> subtasks = currentTask.subtasks;
            for (int i = 0; i < subtasks.Count; i++)
            {
                description = description + "\n\t-" + subtasks[i].description;
                if (subtasks[i].completed)
                {
                    description = description + "(Completed)";
                }
                else
                {
                    description = description + "(In process)";
                }
            }
        }
        else
        {
            description = "No active task";
        }

        UserInterface.GetInstance().ShowTaskHint(description);
    }
}
