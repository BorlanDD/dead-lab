using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glock18 : Weapon
{

    public static string WEAPON_NAME = "Glock-18";
    public override void OnAwake()
    {
        base.OnAwake();
        itemName = "Glock-18";
        weaponType = Type.Pistol;

        availableShootingModes.Add(ShootingMode.Single);
        availableShootingModes.Add(ShootingMode.Burst);

        afterSingleDelay = 0.3f;

        burstBulletCount = 3;
        bulletSpeed = 45;
        burstModeDelay = 0.08f;
        afterBurstDelay = 0.75f;

        slot = 1;
        maxbulletCounts = 18;
        bulletCounts = 18;
        damage = 15;
    }
}
