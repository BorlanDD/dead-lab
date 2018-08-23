using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arms : MonoBehaviour
{

    public Player player;

    public void TakeOffWeapon()
    {
        player.TakeOffWeapon();
    }

    public void SetWeapon()
    {
        player.SetWeapon();
    }

    public void SettedWeapon()
    {
        if (player.usingWeapon == null)
        {
            return;
        }
        player.usingWeapon.Setted();
    }
}
