using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorBattery : Item {

	public Generator generator;
	public override void OnAwake(){
		base.OnAwake();
		id = 1;
		type = ItemType.Battery;
	}

	public override void Interract(){
		base.Interract();
		FindGeneratorKeyTask fgkt = FindGeneratorKeyTask.GetInstance();
		if(fgkt != null){
			generator.locked = false;
			fgkt.completed = true;
			TasksManager.GetInstance().OnTaskUpdated();
		}
	}
}
