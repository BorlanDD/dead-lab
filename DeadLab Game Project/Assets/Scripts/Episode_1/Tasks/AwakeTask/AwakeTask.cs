using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeTask : Task
{

    public override void OnStart()
    {
        description = "Understand what happen.";
        base.OnStart();

    }

    public override void OnFinish(){
        
    }
}
