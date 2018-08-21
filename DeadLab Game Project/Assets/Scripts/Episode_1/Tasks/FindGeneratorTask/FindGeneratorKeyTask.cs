using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGeneratorKeyTask : Task {

	private static FindGeneratorKeyTask findGeneratorKeyTask;

	public static FindGeneratorKeyTask GetInstance(){
		return findGeneratorKeyTask;
	}
	public override void OnStart(){
		findGeneratorKeyTask = this;
		description = "Find generator battery.";
		type = Type.Subtask;

		base.OnStart();
	}
	
}
