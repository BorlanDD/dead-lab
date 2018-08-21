using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGeneratorTask : Task {

	public override void OnStart(){
		base.OnStart();
		description = "Find and switch on energy generator.";
	}
}
