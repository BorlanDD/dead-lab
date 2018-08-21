using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDoorScript : NoiseDoorScript {

	protected override void OnOpen(){
		base.OnOpen();
		_source.clip = openDoor;
		_source.Play();
	}

	protected override void OnClose(){
		base.OnClose();
		_source.clip = closeDoor;
		_source.Play();
	}
}
