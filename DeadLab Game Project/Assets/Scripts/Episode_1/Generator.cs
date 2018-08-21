using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : InteractionObject
{

    public GameObject[] lights;
    public DoorScript[] doors;

    private int generatorKeyId;
    private InterfactionObjectEnum generatorKeyType;


    public override void OnStart()
    {
        base.OnStart();
        locked = true;

        generatorKeyId = 1;
        generatorKeyType = InterfactionObjectEnum.Key;

    }

    public void SwitchOnLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(true);
        }
    }

    public void SwitchOffLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].SetActive(false);
        }
    }

    public void LockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].locked = true;
        }
    }

    public void UnlockDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].locked = false;
        }
    }

    public override void Interract()
    {
        base.Interract();
        Inventory inventory = Player.GetInstance().inventory;
        Item key = inventory.GetItem(generatorKeyId, generatorKeyType);
        if (key != null)
        {
            SwitchOnLights();
            UnlockDoors();
            inventory.RemoveItem(key);
        }
        else
        {
            FindGeneratorTask fgt = (FindGeneratorTask)FindGeneratorTask.GetInstance();
            if (fgt != null)
            {
                FindGeneratorKeyTask fgkt = GetComponent<FindGeneratorKeyTask>();
                if(!fgkt.started){
                    fgkt.OnStart();
                    fgt.addSubTask(fgkt);
                    locked = true;
                }
            }
        }
    }
}
