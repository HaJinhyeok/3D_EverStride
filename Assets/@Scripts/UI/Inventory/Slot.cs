using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class Slot : MonoBehaviour
{
    public ItemData ItemData;
    public int Amount;
    public Action<Slot> OnPostUpdate;

    public Image IconImage;
    public Text AmountText;

    public Slot(ItemData itemData, int amount) => UpdateSlot(itemData, amount);
    public void AddAmount(int amount) => UpdateSlot(ItemData, Amount += amount);

    private void Start()
    {
        IconImage.color = new Color(0, 0, 0, 0);
        AmountText.text = string.Empty;
    }

    public void UpdateSlot(ItemData itemData, int amount)
    {
        if (amount <= 0)
        {
            itemData = null;
        }
        ItemData = itemData;
        Amount = amount;
        OnPostUpdate?.Invoke(this);
    }

    // 슬롯이 비어있는지 체크하는 함수
    public bool IsCanPlaceInSlot(Slot slot)
    {
        if (slot.ItemData == null || slot.Amount <= 0)
        {
            return true;
        }
        return false;
    }

    // 인벤토리에서 아이템을 꺼낼 때
    public void SpawnItem()
    {
        PlayerController pc = FindAnyObjectByType<PlayerController>();
        Vector3 spawnPos = pc.transform.position + (pc.transform.GetChild(0).forward * 3); // Player GameObject 안의 Knight 찾아서 앞방향
        Item item = Instantiate(ItemData.Prefab, spawnPos, Quaternion.identity).GetComponent<Item>();

        item.Amount = Amount;
        UpdateSlot(null, 0);
    }
}
