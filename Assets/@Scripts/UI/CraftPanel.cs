using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CraftPanel : MonoBehaviour
{
    public GameObject CraftItemSlot;
    public GameObject CraftItemListPanel;
    public GameObject PreviewSpace;
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

    void CreateCraftList()
    {
        for (int i = 0; i < CraftItemData.Count; i++)
        {
            GameObject craftSlot = Instantiate(CraftItemSlot, Vector2.zero, Quaternion.identity, CraftItemListPanel.transform);

            craftSlot.GetComponent<CraftItemSlot>().ItemData = CraftItemData[i];
            craftSlot.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            craftSlot.AddComponent<EventTrigger>();

            // �ʿ��� �̺�Ʈ
            // Ŭ�� �� - currentItem ���� / ItemPreviewImage ����
            // ���콺 Ŀ�� �ø� �� - CraftItemSlot UI ũ�� Ű���� ���ü� Ȯ��?
            AddEvent(craftSlot, EventTriggerType.PointerClick, (data) => OnClick(craftSlot, (PointerEventData)data));
        }
    }

    public void OnClick(GameObject go, PointerEventData eventData)
    {
        currentItem = go.GetComponent<CraftItemSlot>();
        GameObject previewObject = PreviewSpace.GetComponentInChildren<Item>()?.gameObject;
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
        previewObject = Instantiate(currentItem.ItemData.Prefab, Vector3.zero, Quaternion.identity, PreviewSpace.transform);
        previewObject.AddComponent<PreviewObject>();
        previewObject.layer = 1 << LayerMask.NameToLayer("PreviewObject");
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

    private void OnEnable()
    {
        // previewCamera Ȱ��ȭ
    }

    private void OnDisable()
    {
        // previewObject �ı�
        // previewCamera ��Ȱ��ȭ
    }
}
