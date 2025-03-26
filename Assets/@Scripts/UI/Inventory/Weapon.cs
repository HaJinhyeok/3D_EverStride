using UnityEngine;

public class Weapon : Item
{
    public Weapon() { }
    public Weapon(ItemData itemData)
    {
        ItemData = itemData;
    }

    public Define.WeaponType GetWeaponType()
    {
        return ItemData.WeaponType;
    }
}
