using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUtils
{

    public static Weapon GetWeaponBySlot(int slot)
    {
        Weapon weapon;
        IList<Item> weapons = Player.GetInstance().inventory.GetItemByType(ItemType.Weapon);
        for (int i = 0; i < weapons.Count; i++)
        {
			weapon = (Weapon)weapons[i];
            if (weapon.slot == slot)
            {
				return weapon;
            }
        }

        return null;
    }
}
