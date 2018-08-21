using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour {

	protected bool oneShot;

	public TaskTrigger(){}
	private void OnTriggerEnter(Collider other) {
		oneShot = false;
	}

	protected virtual void Triggered(){
		if(oneShot){return;}
	}
}
