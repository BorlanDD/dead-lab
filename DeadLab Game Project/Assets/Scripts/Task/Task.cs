using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task {

	public Task nextTask;
	public bool started {get; protected set;}

	public Task (){
		started = false;
	}

    public string description { get; protected set; }

    public virtual void OnStart() { 
		TasksManager.GetInstance().SetTask(this);
		started = true;
	}
    public virtual void OnFinish() { 
		
	}


}
