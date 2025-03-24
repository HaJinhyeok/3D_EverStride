using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    // script�� ������Ʈȭ �����ִ� Ŭ����
    public string ItemName;
    public Define.ItemType ItemType;
    public int Price;
    public string Description;
    public bool isStack;
    public Sprite Icon;
    public GameObject Prefab;
}
