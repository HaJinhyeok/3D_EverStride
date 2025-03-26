using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftPanel : MonoBehaviour
{
    public GameObject CraftItemSlot;
    public GameObject CraftItemListPanel;
    // ItemData ����Ʈ�� �����ͼ� ���ʷ� ����Ʈ ä���ֱ�
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

            // �ʿ��� �̺�Ʈ: Ŭ�� �� - currentItem ���� / ItemPreviewImage ����
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
        // �ʿ��� ��ᰡ ����� ������
        if (true)
        {
            // ��� �������ְ�
            // ������ ���� �� inventory �� ĭ�� �ֱ�
        }
        // ������� ������
        else
        {
            // ��ᰡ �����ϴٴ� ���� ���
            Debug.Log("Not enough mineral");
        }
    }
}
