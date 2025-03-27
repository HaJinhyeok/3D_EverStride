using System;
using UnityEngine;
[Serializable]
public class Ingredient
{
    public Define.IngredientType type;
    public int count;
}
[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    // script�� ������Ʈȭ �����ִ� Ŭ����
    public string ItemName;
    public Define.ItemType ItemType;
    public Define.WeaponType WeaponType;
    public Define.IngredientType IngredientType;
    public int Price;
    public string Description;
    public bool isStack;
    public Sprite Icon;
    public GameObject Prefab;

    [Header("Ingredient")]
    public Ingredient[] Ingredients;
}
