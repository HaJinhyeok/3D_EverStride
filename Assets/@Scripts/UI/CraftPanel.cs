using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CraftPanel : MonoBehaviour
{
    public GameObject CraftItemSlot;
    public GameObject CraftItemListPanel;
    public Inventory Inventory;
    public GameObject PreviewSpace;
    // ItemData 리스트를 가져와서 차례로 리스트 채워넣기
    public List<ItemData> CraftItemData = new List<ItemData>();
    public Text CraftIngredientText;
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
        // trigger에 액션을 담고
        eventTrigger.callback.AddListener(action);
        // trigger 추가해주기
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

            // 필요한 이벤트
            // 클릭 시 - currentItem 변경 / ItemPreviewImage 변경
            // 마우스 커서 올릴 시 - CraftItemSlot UI 크기 키워서 가시성 확장?
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
        // 왜 y축으로 20올라간 위치에서 생성되는가 -> previewSpace 자체가 (0,-20,0)에 있기 때문이었던듯
        previewObject = Instantiate(currentItem.ItemData.Prefab, PreviewSpace.transform);
        previewObject.transform.localPosition = new Vector3(0, 0.2f, 0);
        previewObject.AddComponent<PreviewObject>();
        if (previewObject.GetComponent<Pickaxe>())
            previewObject.transform.localEulerAngles = new Vector3(0, 120, 0);
        previewObject.layer = LayerMask.NameToLayer("PreviewObject");

        CraftIngredientText.text = "";
        for (int i = 0; i < currentItem.ItemData.Ingredients.Length; i++)
        {
            Ingredient ingredient = currentItem.ItemData.Ingredients[i];
            CraftIngredientText.text += $"{Define.IngredientData[ingredient.type].name}: {ingredient.count}\t";
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

        // 필요한 재료가 충분히 있는지 확인
        // 현재 제작 대상 아이템 + 인벤토리 정보

        if (IsCraftPossible(currentItem.ItemData.Ingredients))
        {
            // 재료 차감해주고
            UseCraftIngredients(currentItem.ItemData.Ingredients);
            // 아이템 생성 후 inventory 빈 칸에 넣기
            Inventory.AddItem(currentItem.ItemData.Prefab.GetComponent<WeaponItem>(), 1);
        }
        // 충분하지 않으면
        else
        {
            // 재료가 부족하다는 문구 출력
            Debug.Log("Not enough mineral!!!");
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
                // 해당 재료가 없거나 재료의 개수가 필요 개수보다 적을 경우
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
    //    // previewCamera 활성화
    //    PreviewSpace.transform.GetChild(0).gameObject.SetActive(true);
    //}

    //private void OnDisable()
    //{
    //    // previewObject 있으면 파괴
    //    if (PreviewSpace?.transform.childCount >= 4)
    //    {
    //        GameObject previewObject = PreviewSpace.transform.GetChild(3).gameObject;
    //        Destroy(previewObject);
    //    }
    //    // previewCamera 비활성화
    //    PreviewSpace.transform.GetChild(0).gameObject.SetActive(false);
    //}
}
