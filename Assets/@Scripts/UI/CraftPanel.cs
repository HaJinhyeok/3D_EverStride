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
    public List<ItemData> CraftItemList = new List<ItemData>();
    public Dictionary<string, List<ItemData>> CraftItemDictionary = new Dictionary<string, List<ItemData>>();
    public Text CraftIngredientText;
    public Button CraftButton;
    public Button[] CategoryButton;
    public CraftTable CraftTable;

    Vector2 _start = new Vector2(-65, 120);
    Vector2 _size = new Vector2(120, 50);
    Vector2 _space = new Vector2(10, 10);
    int _numberOfColumn = 2;

    List<GameObject> craftItemSlots = new List<GameObject>();
    CraftItemSlot currentItem;
    Button currentButton;

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

    void CreateCraftList(string category)
    {
        for (int i = 0; i < CraftItemDictionary[category].Count; i++)
        {
            GameObject craftSlot = Instantiate(CraftItemSlot, CraftItemListPanel.transform);

            craftSlot.GetComponent<CraftItemSlot>().ItemData = CraftItemDictionary[category][i];
            craftSlot.GetComponent<RectTransform>().localPosition = CalculatePosition(i);
            craftSlot.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;
            craftSlot.AddComponent<EventTrigger>();
            craftItemSlots.Add(craftSlot);

            // 클릭 시 - currentItem 변경 / ItemPreviewImage 변경
            AddEvent(craftSlot, EventTriggerType.PointerClick, (data) => OnClick(craftSlot, (PointerEventData)data));
        }

    }

    void CreateDictionary()
    {
        for (int i = 0; i < CraftItemList.Count; i++)
        {
            if (CraftItemList[i].ItemType == Define.ItemType.Weapon)
            {
                if (!CraftItemDictionary.ContainsKey(Define.FightItem))
                {
                    List<ItemData> list = new List<ItemData>();
                    list.Add(CraftItemList[i]);
                    CraftItemDictionary.Add(Define.FightItem, list);
                }
                else
                {
                    CraftItemDictionary[Define.FightItem].Add(CraftItemList[i]);
                }
            }
            else if (CraftItemList[i].ItemType == Define.ItemType.Equipment)
            {
                if (!CraftItemDictionary.ContainsKey(Define.EquipItem))
                {
                    List<ItemData> list = new List<ItemData>();
                    list.Add(CraftItemList[i]);
                    CraftItemDictionary.Add(Define.EquipItem, list);
                }
                else
                {
                    CraftItemDictionary[Define.EquipItem].Add(CraftItemList[i]);
                }
            }
            else if (CraftItemList[i].ItemType == Define.ItemType.Countable)
            {
                if (!CraftItemDictionary.ContainsKey(Define.BagItem))
                {
                    List<ItemData> list = new List<ItemData>();
                    list.Add(CraftItemList[i]);
                    CraftItemDictionary.Add(Define.BagItem, list);
                }
                else
                {
                    CraftItemDictionary[Define.BagItem].Add(CraftItemList[i]);
                }
            }
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
        // 장비 아이템일 경우, 프리뷰캐릭터를 생성하고 해당 부위의 레이어값만 렌더링 레이어에 넣어준다
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
            // 왜 y축으로 20올라간 위치에서 생성되는가 -> previewSpace 자체가 (0,-20,0)에 있기 때문이었던듯
            previewObject = Instantiate(currentItem.ItemData.Prefab, PreviewSpace.transform);
            CraftTable.PreviewCameraSetting(1 << LayerMask.NameToLayer("PreviewObject"));
            previewObject.transform.localPosition = new Vector3(0, 0.2f, 0);
        }
        previewObject.AddComponent<PreviewObject>();
        previewObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (previewObject.GetComponent<Pickaxe>())
            previewObject.transform.localEulerAngles = new Vector3(0, 120, 0);
        previewObject.layer = LayerMask.NameToLayer("PreviewObject");
        AudioSource audioSource = previewObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.clip = null;
        }

        CraftIngredientText.text = "";
        for (int i = 0; i < currentItem.ItemData.Ingredients.Length; i++)
        {
            Ingredient ingredient = currentItem.ItemData.Ingredients[i];
            CraftIngredientText.text += $"{Define.IngredientData[ingredient.type].ItemName}: {ingredient.count}\t";
        }
    }

    private void Awake()
    {
        CreateDictionary();
        CraftButton.onClick.AddListener(OnCraftButtonClick);
        for (int i = 0; i < CategoryButton.Length; i++)
        {
            int idx = i;
            CategoryButton[i].onClick.AddListener(() => OnCategoryButtonClick(idx));
        }
    }

    private void OnEnable()
    {
        currentButton = CategoryButton[0];
        OnCategoryButtonClick(0);
    }

    private void OnDisable()
    {
        currentButton.GetComponent<RectTransform>().localScale = Vector3.one;
        if (PreviewSpace.transform.childCount >= 4)
        {
            GameObject previewObject = PreviewSpace.transform.GetChild(3).gameObject;
            Destroy(previewObject);
        }
        CraftIngredientText.text = "";
    }

    void OnCraftButtonClick()
    {
        if (currentItem == null)
            return;
        if (GameManager.Instance.Player.transform.GetChild(0).GetComponent<Animator>().GetBool(Define.IsCrafting))
            return;
        // 필요한 재료가 충분히 있는지 확인
        if (IsCraftPossible(currentItem.ItemData.Ingredients))
        {
            // 재료 차감해주고
            UseCraftIngredients(currentItem.ItemData.Ingredients);
            // CraftTime만큼 망치질 coroutine
            CraftTable.OnCraftAction?.Invoke(currentItem.ItemData, 5f);
            // 아이템 생성 후 inventory 빈 칸에 넣기
            Inventory.AddItem(currentItem.ItemData.Prefab.GetComponent<Item>(), 1);
        }
        // 충분하지 않으면
        else
        {
            // 재료가 부족하다는 문구 출력
            UI_Warning.OnWarningEffect?.Invoke(Define.NotEnoughMineral);
        }
    }

    void OnCategoryButtonClick(int idx)
    {
        currentButton.GetComponent<RectTransform>().localScale = Vector3.one;
        for (int i = 0; i < craftItemSlots.Count; i++)
        {
            Destroy(craftItemSlots[i]);
        }
        craftItemSlots.Clear();
        CreateCraftList(Define.ItemCategory[idx]);
        CategoryButton[idx].GetComponent<RectTransform>().localScale = Vector3.one * 1.2f;
        currentButton = CategoryButton[idx];
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

}
