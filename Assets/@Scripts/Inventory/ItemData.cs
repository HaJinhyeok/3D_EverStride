using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    // script를 오브젝트화 시켜주는 클래스
    public string ItemName;
    public Define.ItemType ItemType;
    public int Price;
    public string Description;
    public bool isStack;
    public Sprite Icon;
    public GameObject Prefab;
}
