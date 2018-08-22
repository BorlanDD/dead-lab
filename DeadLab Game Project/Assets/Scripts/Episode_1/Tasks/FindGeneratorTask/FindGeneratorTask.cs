using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGeneratorTask : Task
{

    public static FindGeneratorTask findGeneratorTask;

    public static FindGeneratorTask GetInstance()
    {
        return findGeneratorTask;
    }

    public Generator generator;

    public override void OnStart()
    {
		findGeneratorTask = this;
        description = "Find and switch on energy generator.";
        base.OnStart();

        AwakeTask awakeTask = AwakeTask.GetInstance();

        if (awakeTask != null)
        {
            awakeTask.OnFinish();
            Destroy(awakeTask.gameObject);
        }
        generator.LockDoors();
        generator.SwitchOffLights();
        generator.locked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!started && other.gameObject.tag == "Player")
        {
            OnStart();
        }
    }
}
