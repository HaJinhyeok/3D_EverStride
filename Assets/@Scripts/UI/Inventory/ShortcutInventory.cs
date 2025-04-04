using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShortcutInventory : Inventory
{
    Slot[] ShortcutSlot = new Slot[5];

    Vector2 CalculatePosition(int i)
    {
        float x = _start.x + ((_space.x + _size.x) * (i % _numberOfColumn));
        float y = _start.y + (-(_space.y + _size.y) * (i / _numberOfColumn));

        return new Vector3(x, y, 0);
    }

    public override void OnEnterInterface(GameObject go)
    {
        MouseData.MouseOverInventory = go.GetComponent<ShortcutInventory>();
    }

    protected override void CreateSlot()
    {
        for (int i = 0; i < ShortcutSlot.Length; i++)
        {
            GameObject go = Instantiate(Slot, transform);

            go.GetComponent<RectTransform>().localPosition = CalculatePosition(i);
            // 이벤트 트리거는 일단 보류
            //go.AddComponent<EventTrigger>();

            //AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            //AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });
            //AddEvent(go, EventTriggerType.EndDrag, delegate { OnEndDrag(go); });

            ShortcutSlot[i] = go.GetComponent<Slot>();
            ShortcutSlot[i].OnPostUpdate += OnPostUpdate;
            _slotUIs.Add(go, ShortcutSlot[i]);
            go.name = "ShortcutSlot : " + i;
        }
    }
    public void UpdateShortcutInventory(Slot[] shortcutSlots)
    {
        for (int i = 0; i < ShortcutSlot.Length; i++)
        {
            ShortcutSlot[i].UpdateSlot(shortcutSlots[i].ItemData, shortcutSlots[i].Amount);
        }
    }

    public Slot UseShortcutItem(int idx)
    {
        return ShortcutSlot[idx];
    }

    private void Awake()
    {
        _start = new Vector2(-50, 2);
        CreateSlot();

        AddEvent(gameObject, EventTriggerType.PointerEnter, (baseEvent) => { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, (baseEvent) => { OnExitInterface(gameObject); });
    }
}
