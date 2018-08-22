using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TasksManager : MonoBehaviour
{
    public IList<Task> tasks { get; private set; }

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
        tasks = new List<Task>();

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
        tasks.Add(task);
        OnTaskUpdated();
    }

    public void FinishTask(Task task)
    {
        task.completed = true;
        tasks.Remove(task);
        if (tasks.Count > 0)
        {
            OnTaskUpdated();
        }
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
        if (tasks.Count > 0)
        {
            description = "";
            for (int t = 0; t < tasks.Count; t++)
            {
                description += tasks[t].description;
                IList<Task> subtasks = tasks[t].subtasks;
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
                description += "\n";
            }
        }
        else
        {
            description = "No active task";
        }

        UserInterface.GetInstance().ShowTaskHint(description);
    }
}
