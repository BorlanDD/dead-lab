using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : InteractionObject
{

    public GameObject[] lights;
    public DoorScript[] doors;
    
    public void switchOnLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(true);
        }
    }

    public void switchOffLights()
    {
        Debug.Log("OFFING: " + lights.Length);
        for (int i = 0; i < lights.Length; i++)
        {
            Debug.Log("OFF");
            lights[i].SetActive(false);
        }
    }

    public void lockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].locked = true;
        }
    }

    public void unlockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].locked = false;
        }
    }

    public override void interract()
    {
        Debug.Log("interact");
    }
}
