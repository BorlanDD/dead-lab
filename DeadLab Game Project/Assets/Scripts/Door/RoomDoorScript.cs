using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoorScript : NoiseDoorScript {


	protected override void OnOpen(){
		base.OnOpen();
		_source.PlayOneShot(openDoor);
	}

	protected override void OnClose(){
		base.OnClose();
		_source.PlayOneShot(closeDoor);
	}
}
