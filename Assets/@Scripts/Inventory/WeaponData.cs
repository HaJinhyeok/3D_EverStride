using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Inventory/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string WeaponName;
    public Define.WeaponType WeaponType;
    public int Price;
    public string Description;
    public Sprite Icon;
    public GameObject Prefab;
}
