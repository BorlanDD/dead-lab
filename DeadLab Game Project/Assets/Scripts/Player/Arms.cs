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

    public void ReloadedWeapon()
    {
        player.ReloadedWeapon();
    }

    public void TakeMag()
    {
        Weapon weapon = player.usingWeapon;
        if (weapon != null)
        {
            if (weapon.itemName.Equals(Glock18.WEAPON_NAME))
            {
                weapon.magazin.transform.position = player.glock18MagPos.position;
                weapon.magazin.transform.SetParent(player.glock18MagPos);
                Debug.Log("Set");
            }
        }
    }
}
