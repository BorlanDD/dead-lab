using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{

    public bool started { get; protected set; }
    public bool completed;

    public Type type;

    public enum Type
    {
        Task,
        Subtask
    }

    protected Task task;

    public IList<Task> subtasks { get; protected set; }

    void Awake()
    {
        started = false;
        completed = false;
        subtasks = new List<Task>();

        type = Type.Task;
    }

    public void addSubTask(Task substask)
    {
        subtasks.Add(substask);
        TasksManager.GetInstance().OnTaskUpdated();
    }

    public string description { get; protected set; }

    public virtual void OnStart()
    {
        if (type == Type.Task)
        {
            TasksManager.GetInstance().SetTask(this);
        }
        started = true;
    }
    public virtual void OnFinish()
    {

    }


}
