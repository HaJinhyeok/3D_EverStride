using UnityEngine;

public class WeaponItem : Item
{
    public WeaponItem() { }
    public WeaponItem(ItemData itemData)
    {
        ItemData = itemData;
    }

    public Define.WeaponType GetWeaponType()
    {
        return ItemData.WeaponType;
    }
}
