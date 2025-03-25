using UnityEngine;

public class Weapon : MonoBehaviour
{
    public WeaponData WeaponData;

    public Weapon() { }
    public Weapon(WeaponData weaponData)
    {
        WeaponData = weaponData;
    }

    public Define.WeaponType GetWeaponType()
    {
        return WeaponData.WeaponType;
    }
}
