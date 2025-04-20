using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class CraftPanel : MonoBehaviour
{
    public GameObject CraftItemSlot;
    public GameObject CraftItemListPanel;
    public Inventory Inventory;
    public GameObject PreviewSpace;
    // ItemData ����Ʈ�� �����ͼ� ���ʷ� ����Ʈ ä���ֱ�
    public List<ItemData> CraftItemData = new List<ItemData>();
    public Text CraftIngredientText;
    public Button CraftButton;
    public CraftTable CraftTable;

    Vector2 _start = new Vector2(-65, 120);
    Vector2 _size = new Vector2(120, 50);
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
            GameObject craftSlot = Instantiate(CraftItemSlot, CraftItemListPanel.transform);

            craftSlot.GetComponent<CraftItemSlot>().ItemData = CraftItemData[i];
            //craftSlot.GetComponent<RectTransform>().anchoredPosition = CalculatePosition(i);
            craftSlot.GetComponent<RectTransform>().localPosition = CalculatePosition(i);
            craftSlot.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
            craftSlot.AddComponent<EventTrigger>();

            // �ʿ��� �̺�Ʈ
            // Ŭ�� �� - currentItem ���� / ItemPreviewImage ����
            // ���콺 Ŀ�� �ø� �� - CraftItemSlot UI ũ�� Ű���� ���ü� Ȯ��?
            AddEvent(craftSlot, EventTriggerType.PointerClick, (data) => OnClick(craftSlot, (PointerEventData)data));
        }
    }

    public void OnClick(GameObject go, PointerEventData eventData)
    {
        GameObject previewObject;
        currentItem = go.GetComponent<CraftItemSlot>();
        if (PreviewSpace.transform.childCount >= 4)
        {
            previewObject = PreviewSpace.transform.GetChild(3).gameObject;
            Destroy(previewObject);
        }
        // ��� �������� ���, ������ĳ���͸� �����ϰ� �ش� ������ ���̾�� ������ ���̾ �־��ش�
        if (currentItem.ItemData.ItemType == Define.ItemType.Equipment)
        {
            previewObject = Instantiate(GameManager.Instance.PreviewCharacter, PreviewSpace.transform);
            CraftTable.PreviewCameraSetting(1 << LayerMask.NameToLayer("PreviewObject") | 1 << currentItem.ItemData.Prefab.layer);
            switch (currentItem.ItemData.Prefab.layer)
            {
                case (int)Define.EquipmentType.Helmet:
                    previewObject.transform.localPosition = new Vector3(0f, -1.5f, 0f);
                    break;
                case (int)Define.EquipmentType.Chest:
                    previewObject.transform.localPosition = new Vector3(0f, -1f, 1f);
                    break;
                case (int)Define.EquipmentType.Shoulders:
                    previewObject.transform.localPosition = new Vector3(0f, -1.3f, 0.5f);
                    break;
                case (int)Define.EquipmentType.Gloves:
                    previewObject.transform.localPosition = new Vector3(0f, -1.3f, 1.5f);
                    break;
                case (int)Define.EquipmentType.Pants:
                    previewObject.transform.localPosition = new Vector3(0f, 0f, 1f);
                    break;
                case (int)Define.EquipmentType.Boots:
                    previewObject.transform.localPosition = new Vector3(0f, 0.05f, 0f);
                    break;
                default:
                    break;
            }
        }
        else
        {
            // �� y������ 20�ö� ��ġ���� �����Ǵ°� -> previewSpace ��ü�� (0,-20,0)�� �ֱ� �����̾�����
            previewObject = Instantiate(currentItem.ItemData.Prefab, PreviewSpace.transform);
            CraftTable.PreviewCameraSetting(1 << LayerMask.NameToLayer("PreviewObject"));
            previewObject.transform.localPosition = new Vector3(0, 0.2f, 0);
        }
        previewObject.AddComponent<PreviewObject>();
        previewObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //if (previewObject.GetComponent<Item>()?.ItemData.ItemName == "�ֱ�")
        //    previewObject.transform.localPosition = Vector3.zero;
        if (previewObject.GetComponent<Pickaxe>())
            previewObject.transform.localEulerAngles = new Vector3(0, 120, 0);
        previewObject.layer = LayerMask.NameToLayer("PreviewObject");

        CraftIngredientText.text = "";
        for (int i = 0; i < currentItem.ItemData.Ingredients.Length; i++)
        {
            Ingredient ingredient = currentItem.ItemData.Ingredients[i];
            CraftIngredientText.text += $"{Define.IngredientData[ingredient.type].ItemName}: {ingredient.count}\t";
        }
    }

    private void Awake()
    {
        CreateCraftList();
        CraftButton.onClick.AddListener(OnCraftButtonClick);
    }

    void OnCraftButtonClick()
    {
        if (currentItem == null)
            return;
        if (GameManager.Instance.Player.transform.GetChild(0).GetComponent<Animator>().GetBool(Define.IsCrafting))
            return;
        // �ʿ��� ��ᰡ ����� �ִ��� Ȯ��
        // ���� ���� ��� ������ + �κ��丮 ����

        if (IsCraftPossible(currentItem.ItemData.Ingredients))
        {
            // ��� �������ְ�
            UseCraftIngredients(currentItem.ItemData.Ingredients);
            // CraftTime��ŭ ��ġ�� coroutine
            CraftTable.OnCraftAction?.Invoke(currentItem.ItemData, 5f);
            // ������ ���� �� inventory �� ĭ�� �ֱ�
            Inventory.AddItem(currentItem.ItemData.Prefab.GetComponent<Item>(), 1);
        }
        // ������� ������
        else
        {
            // ��ᰡ �����ϴٴ� ���� ���
            Debug.Log("Not enough mineral!!!");
            UI_Warning.Instance.WarningEffect(Define.NotEnoughMineral);
        }
    }

    bool IsCraftPossible(Ingredient[] ingredients)
    {
        GameObject tmpObject = GameObject.Find("TMP");
        if (tmpObject == null)
        {
            tmpObject = new GameObject("TMP");
        }
        Item tmpItem = tmpObject.GetComponent<Item>();
        if (tmpItem == null)
        {
            tmpItem = tmpObject.AddComponent<Item>();
        }
        for (int i = 0; i < ingredients.Length; i++)
        {
            tmpItem.ItemData = Define.IngredientData[ingredients[i].type];
            Slot slot = Inventory.FindItemInInventory(tmpItem);
            if (slot == null || slot.Amount < ingredients[i].count)
            {
                // �ش� ��ᰡ ���ų� ����� ������ �ʿ� �������� ���� ���
                return false;
            }
        }
        return true;
    }

    void UseCraftIngredients(Ingredient[] ingredients)
    {
        GameObject tmpObject = GameObject.Find("TMP");
        if (tmpObject == null)
        {
            tmpObject = new GameObject("TMP");
        }
        Item tmpItem = tmpObject.GetComponent<Item>();
        if (tmpItem == null)
        {
            tmpItem = tmpObject.AddComponent<Item>();
        }
        for (int i = 0; i < ingredients.Length; i++)
        {
            tmpItem.ItemData = Define.IngredientData[ingredients[i].type];
            Slot slot = Inventory.FindItemInInventory(tmpItem);
            slot.AddAmount(-ingredients[i].count);
        }
    }

    //private void OnEnable()
    //{
    //    // previewCamera Ȱ��ȭ
    //    PreviewSpace.transform.GetChild(0).gameObject.SetActive(true);
    //}

    //private void OnDisable()
    //{
    //    // previewObject ������ �ı�
    //    if (PreviewSpace?.transform.childCount >= 4)
    //    {
    //        GameObject previewObject = PreviewSpace.transform.GetChild(3).gameObject;
    //        Destroy(previewObject);
    //    }
    //    // previewCamera ��Ȱ��ȭ
    //    PreviewSpace.transform.GetChild(0).gameObject.SetActive(false);
    //}
}
