using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTask : Task
{

    private static AwakeTask awakeTask;
    public override void OnStart()
    {
        awakeTask = this;
        description = "Understand what happen.";
        base.OnStart();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started && other.gameObject.tag == "Player")
        {
            OnStart();
        }
    }

    public static AwakeTask GetInstance()
    {
        return awakeTask;
    }
}
