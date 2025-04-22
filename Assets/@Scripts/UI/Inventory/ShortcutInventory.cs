using UnityEngine;
using UnityEngine.EventSystems;

public class ShortcutInventory : MonoBehaviour
{
    public GameObject Slot;

    Slot[] ShortcutSlot = new Slot[5];
    Vector2 _start = new Vector2(-50, 2);    // 인벤토리 생성 시작 위치
    Vector2 _size = new Vector2(50, 50);        // 슬롯 크기
    Vector2 _space = new Vector2(5, 5);         // 슬롯 간의 여백

    private void Start()
    {
        CreateSlot();
    }

    Vector2 CalculatePosition(int i)
    {
        float x = _start.x + (_space.x + _size.x) * i;

        return new Vector3(x, _start.y, 0);
    }

    void CreateSlot()
    {
        for (int i = 0; i < ShortcutSlot.Length; i++)
        {
            GameObject go = Instantiate(Slot, transform);

            go.GetComponent<RectTransform>().localPosition = CalculatePosition(i);

            ShortcutSlot[i] = go.GetComponent<Slot>();
            ShortcutSlot[i].OnPostUpdate += OnPostUpdate;
            //_slotUIs.Add(go, ShortcutSlot[i]);
            go.name = "ShortcutSlot : " + i;
        }
    }

    public void OnPostUpdate(Slot slot)
    {
        bool isExist = slot.Amount > 0;
        slot.IconImage.sprite = isExist ? slot.ItemData.Icon : null;
        slot.IconImage.color = isExist ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0); // 불투명 - 투명
        slot.AmountText.text = isExist ? slot.Amount.ToString() : string.Empty;
    }

    public void UpdateShortcutInventory(Slot[] shortcutSlots)
    {
        for (int i = 0; i < ShortcutSlot.Length; i++)
        {
            ShortcutSlot[i].UpdateSlot(shortcutSlots[i]?.ItemData, shortcutSlots[i]?.Amount ?? 0);
        }
    }
}
