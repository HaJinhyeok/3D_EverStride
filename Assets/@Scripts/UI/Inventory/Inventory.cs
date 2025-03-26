using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public static class MouseData
{
    public static Inventory MouseOverInventory; // ���콺�� �ö� �κ��丮
    public static GameObject SlotHoveredOver;   // ���콺 Ŀ���� ��ġ�� ����
    public static GameObject DragImage;         // �巡�� ���� ������ �̹���
}

[RequireComponent(typeof(EventTrigger))]
public class Inventory : MonoBehaviour
{
    public GameObject Slot;

    Vector2 _start = new Vector2(-110, 120);    // �κ��丮 ���� ���� ��ġ
    Vector2 _size = new Vector2(50, 50);        // ���� ũ��
    Vector2 _space = new Vector2(5, 5);         // ���� ���� ����
    int _numberOfColumn = 5;

    Slot[] _slots = new Slot[30];
    Dictionary<GameObject, Slot> _slotUIs = new Dictionary<GameObject, Slot>();

    public Action<ItemData> OnUseItem;

    // �� ������ ���� ��ȯ
    public int EmptySlotCount
    {
        get
        {
            int count = 0;
            foreach (Slot slot in _slots)
            {
                if (slot.Amount == 0)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public void OnPostUpdate(Slot slot)
    {
        bool isExist = slot.Amount > 0;
        slot.IconImage.sprite = isExist ? slot.ItemData.Icon : null;
        slot.IconImage.color = isExist ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0); // ������ - ����
        slot.AmountText.text = isExist ? slot.Amount.ToString() : string.Empty;
    }

    Vector2 CalculatePosition(int i)
    {
        float x = _start.x + ((_space.x + _size.x) * (i % _numberOfColumn));
        float y = _start.y + (-(_space.y + _size.y) * (i / _numberOfColumn));

        return new Vector3(x, y, 0);
    }

    GameObject CreateDragImage(GameObject go)
    {
        if (_slotUIs[go].ItemData == null)
        {
            return null;
        }
        GameObject dragImage = new GameObject("DragImage");
        RectTransform rectTr = dragImage.AddComponent<RectTransform>();
        rectTr.sizeDelta = new Vector2(40, 40);
        dragImage.transform.SetParent(transform.parent);
        Image image = dragImage.AddComponent<Image>();

        image.sprite = _slotUIs[go].ItemData.Icon;
        image.raycastTarget = false; // ���콺 ������ ���� �ʰ� ����
        return dragImage;
    }

    void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        var trigger = go.GetComponent<EventTrigger>();
        if (!trigger)
            return;
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        // trigger�� �׼��� ���
        eventTrigger.callback.AddListener(action);
        // trigger �߰����ֱ�
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnterInterface(GameObject go)
    {
        MouseData.MouseOverInventory = go.GetComponent<Inventory>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.MouseOverInventory = null;
    }

    public void OnEnterSlot(GameObject go)
    {
        MouseData.SlotHoveredOver = go;
    }

    public void OnStartDrag(GameObject go)
    {
        if (_slotUIs[go].ItemData == null)
            return;
        MouseData.DragImage = CreateDragImage(go);
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.DragImage == null)
            return;
        MouseData.DragImage.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(GameObject go)
    {
        Destroy(MouseData.DragImage);
        if (_slotUIs[go].ItemData == null)
            return;
        if (MouseData.MouseOverInventory == null)
        {
            _slotUIs[go].SpawnItem();
        }
        else if (MouseData.SlotHoveredOver)
        {
            Slot mouseHoverSlot = MouseData.MouseOverInventory._slotUIs[MouseData.SlotHoveredOver];
            SwapItems(_slotUIs[go], mouseHoverSlot);
        }
    }

    public void SwapItems(Slot slotA, Slot slotB)
    {
        if (slotA == slotB)
            return;
        ItemData tempData = slotB.ItemData;
        int tempAmount = slotB.Amount;
        slotB.UpdateSlot(slotA.ItemData, slotA.Amount);
        slotA.UpdateSlot(tempData, tempAmount);
    }

    public void UseItem(Slot slot)
    {
        if (slot.ItemData == null || slot.Amount <= 0)
        {
            ItemData itemData = slot.ItemData;
            slot.UpdateSlot(slot.ItemData, slot.Amount - 1);
            OnUseItem?.Invoke(itemData);
        }
    }

    void OnRightClick(Slot slot)
    {
        UseItem(slot);
    }

    void OnLeftClick(Slot slot)
    {
        // ���� ���� ��� ����. �ְ� ���� ��� �ֱ�
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        Slot slot = _slotUIs[go];
        if (slot == null)
            return;
        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
        if (data.button == PointerEventData.InputButton.Right)
        {
            OnRightClick(slot);
        }
    }

    // �κ��丮 ���� ���� ������ �ִ��� �˻�
    public Slot FindItemInInventory(Item item)
    {
        return _slots?.FirstOrDefault(slot => slot.ItemData?.name == item.ItemData?.name);
    }

    public Slot GetEmptySlot()
    {
        return _slots?.FirstOrDefault(slot => slot.ItemData == null);
    }

    // ������ �߰��ϴ� �Լ�
    public bool AddItem(Item item, int amount)
    {
        Slot slot = FindItemInInventory(item);
        if (!item.ItemData.isStack || slot == null)
        {
            if (EmptySlotCount <= 0)
            {
                return false;
            }
            GetEmptySlot().UpdateSlot(item.ItemData, amount);
        }
        else
        {
            slot.AddAmount(amount);
        }
        return true;
    }

    void CreateSlot()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            GameObject go = Instantiate(Slot, Vector3.zero, Quaternion.identity, transform);

            go.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            go.AddComponent<EventTrigger>();

            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnEnterSlot(go); });

            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });

            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });

            _slots[i] = go.GetComponent<Slot>();
            _slots[i].OnPostUpdate += OnPostUpdate;
            _slotUIs.Add(go, _slots[i]);
            go.name = "Slot : " + i;
        }
    }

    private void Awake()
    {
        CreateSlot();
        AddEvent(gameObject, EventTriggerType.PointerEnter, (baseEvent) => { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, (baseEvent) => { OnExitInterface(gameObject); });
    }
}
