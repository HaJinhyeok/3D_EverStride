using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftPanel : MonoBehaviour
{
    public GameObject CraftItemSlot;
    public GameObject CraftItemListPanel;
    // ItemData 리스트를 가져와서 차례로 리스트 채워넣기
    public List<ItemData> CraftItemData = new List<ItemData>();
    public Button CraftButton;

    Vector2 _start = new Vector2(-75, 120);
    Vector2 _size = new Vector2(140, 50);
    Vector2 _space = new Vector2(10, 10);
    int _numberOfColumn = 2;

    List<CraftItemSlot> craftItemSlots = new List<CraftItemSlot>();
    CraftItemSlot currentItem;

    Vector2 CalculatePosition(int i)
    {
        float x = _start.x + ((_space.x + _size.x) * (i % _numberOfColumn));
        float y = _start.y + (-(_space.y + _size.y) * (i / _numberOfColumn));

        return new Vector2(x, y);
    }

    void CreateCraftList()
    {
        for (int i = 0; i < CraftItemData.Count; i++)
        {
            GameObject craftSlot = Instantiate(CraftItemSlot, Vector2.zero, Quaternion.identity, CraftItemListPanel.transform);

            craftSlot.GetComponent<CraftItemSlot>().ItemData = CraftItemData[i];
            craftSlot.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            craftSlot.AddComponent<EventTrigger>();

            // 필요한 이벤트: 클릭 시 - currentItem 변경 / ItemPreviewImage 변경
        }
    }

    public void OnClick(GameObject go, PointerEventData eventData)
    {

    }

    private void Awake()
    {
        CreateCraftList();
    }

    void OnCraftButtonClick()
    {
        // 필요한 재료가 충분히 있으면
        if (true)
        {
            // 재료 차감해주고
            // 아이템 생성 후 inventory 빈 칸에 넣기
        }
        // 충분하지 않으면
        else
        {
            // 재료가 부족하다는 문구 출력
            Debug.Log("Not enough mineral");
        }
    }
}
