using System;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public Define.QuestName name;
    public string context;
    public int requireNum;
    public int currentNum;
    public bool isComlete;
    public Quest(Define.QuestName name, string context, int requireNum)
    {
        this.name = name;
        this.context = context;
        this.requireNum = requireNum;
        this.currentNum = 0;
        this.isComlete = false;
    }

    public void AddNum(int account)
    {
        this.currentNum += account;
        QuestPanel.OnQuestPanelUpdate?.Invoke();
        if (this.currentNum >= this.requireNum)
        {
            this.isComlete = true;
        }
    }
}

public struct PlayerEquipment
{
    public Define.EquipmentType helmet;
    public Define.EquipmentType chest;
    public Define.EquipmentType shoulders;
    public Define.EquipmentType gloves;
    public Define.EquipmentType pants;
    public Define.EquipmentType boots;
}


public class GameManager : Singleton<GameManager>
{
    PlayerController player;
    TrailRenderer weaponTrail;

    bool _isNPCInteractive = false;
    bool _isPossibleCraft = false;
    bool _isConversating = false;
    bool _isPaused = false;

    public Action OnWeaponChanged;
    public Action<bool> OnTrailActivate;

    public Dictionary<Define.QuestName, Quest> QuestDictionary = new Dictionary<Define.QuestName, Quest>();

    public PlayerController Player
    {
        get { return player; }
    }

    #region Boolean

    public bool IsPaused
    {
        get { return _isPaused; }
        set { _isPaused = value; }
    }

    public bool IsNPCInteracive
    {
        get { return _isNPCInteractive; }
        set { _isNPCInteractive = value; }
    }

    public bool IsPossibleCraft
    {
        get { return _isPossibleCraft; }
        set { _isPossibleCraft = value; }
    }

    public bool IsConversating
    {
        get { return _isConversating; }
        set { _isConversating = value; }
    }

    public bool IsUIOn
    {
        get;
        set;
    }

    public bool IsCraftPanelOn
    {
        get;
        set;
    }
    public bool IsInventoryOn
    {
        get;
        set;
    }
    #endregion

    public GameObject[] Weapons;
    public GameObject[] Ingredients;
    public GameObject[] ConsumptionItems;

    // Inventory ������ ���� ����
    public Slot[] InventorySlots;
    public Slot[] ShortcutInventorySlots;
    GameObject Slot;

    // Equipment ���� ����
    public PlayerEquipment PlayerEquipment;

    // ���� ������ ������ Weapons�� �ε����� �˷��� �� �ֵ��� ��ųʸ�...?
    //public Dictionary<ItemData, int> WeaponsMap;

    private void Awake()
    {
        Slot = Resources.Load<GameObject>(Define.SlotPath);
        if (InventorySlots == null)
            InventorySlots = new Slot[25];
        if (ShortcutInventorySlots == null)
            ShortcutInventorySlots = new Slot[5];
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            GameObject go = Instantiate(Slot, transform);


            InventorySlots[i] = go.GetComponent<Slot>();
            //InventorySlots[i].OnPostUpdate += OnPostUpdate;
            go.name = "Slot : " + i;
        }
        for (int i = 0; i < ShortcutInventorySlots.Length; i++)
        {
            GameObject go = Instantiate(Slot, transform);



            ShortcutInventorySlots[i] = go.GetComponent<Slot>();
            //ShortcutInventorySlots[i].OnPostUpdate += OnPostUpdate;
            go.name = "SrhotcutSlot : " + i;
        }
    }

    public void LoadResources()
    {
        Weapons = Resources.LoadAll<GameObject>(Define.WeaponPath);
        Ingredients = Resources.LoadAll<GameObject>(Define.IngredientPath);
        ConsumptionItems = Resources.LoadAll<GameObject>(Define.ConsumptionPath);

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        OnWeaponChanged += GetWeaponTrail;
        OnTrailActivate += ActivateWeaponTrail;

        // starter base equipment
        PlayerEquipment.chest = Define.EquipmentType.None;
        PlayerEquipment.pants = Define.EquipmentType.None;
        PlayerEquipment.boots = Define.EquipmentType.None;
        // no base equipment
        PlayerEquipment.helmet = Define.EquipmentType.None;
        PlayerEquipment.shoulders = Define.EquipmentType.None;
        PlayerEquipment.gloves = Define.EquipmentType.None;
    }

    //public void LoadInventory()
    //{
    //    Inventory inventory=GameObject.Find("Inventory").AddComponent<Inventory>();
    //    inventory = Inventory;
    //    ShortcutInventory shortcut= GameObject.Find("ShortcutInventory").AddComponent<ShortcutInventory>();
    //    shortcut = ShortcutInventory;
    //}

    public void SaveInventory(Slot[] slots, Slot[] shortcutSlots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlots[i].UpdateSlot(slots[i]?.ItemData, slots[i]?.Amount ?? 0);
        }
        for (int i = 0; i < shortcutSlots.Length; i++)
        {
            ShortcutInventorySlots[i].UpdateSlot(shortcutSlots[i]?.ItemData, shortcutSlots[i]?.Amount ?? 0);
        }
    }

    public void GetWeaponTrail()
    {
        if (Player.WeaponTypeHash == 2)
        {
            Debug.Log(Player.WeaponPos.transform.childCount);
            if (Player.WeaponPos.transform.childCount == 1)
            {
                Debug.Log(Player.WeaponPos.transform.GetChild(0).gameObject.name);
                weaponTrail = Player.WeaponPos.transform.GetChild(0).GetChild(0).GetComponent<TrailRenderer>();
            }
            else
            {
                Debug.Log(Player.WeaponPos.transform.GetChild(1).gameObject.name);
                weaponTrail = Player.WeaponPos.transform.GetChild(1).GetChild(0).GetComponent<TrailRenderer>();
            }
        }
        else
        {
            weaponTrail = null;
        }
    }

    public void ActivateWeaponTrail(bool state)
    {
        weaponTrail?.gameObject.SetActive(state);
    }

    #region Quest
    public void MakeQuest(Define.QuestName name, string context, int num)
    {
        Quest quest = new Quest(name, context, num);
        if (!QuestDictionary.ContainsKey(name))
        {
            QuestDictionary.Add(name, quest);
            if (name == Define.QuestName.Wood)
            {
                // Ȥ�� �κ��丮�� �̹� ������ ������ ���� ������
                Slot slot = player.Inventory.FindItemInInventory(Ingredients[1].GetComponent<Item>());
                if (slot != null)
                {
                    QuestDictionary[name].AddNum(slot.Amount);
                }
            }
            QuestPanel.OnQuestPanelUpdate?.Invoke();
        }
        else
        {
            UI_Warning.Instance.WarningEffect(Define.DuplicatedQuest);
        }
    }

    public void DeleteQuest(Define.QuestName name)
    {
        int len = QuestDictionary.Count;
        foreach (KeyValuePair<Define.QuestName, Quest> quest in QuestDictionary)
        {
            if (quest.Value.name == name)
            {
                QuestDictionary.Remove(name);
                break;
            }
        }
        // �����Ǿ����� ������Ʈ
        if (len != QuestDictionary.Count)
        {
            QuestPanel.OnQuestPanelUpdate?.Invoke();
        }
    }

    public void CompleteQuest(Define.QuestName name)
    {
        // ���� ���� ��
        if (name == Define.QuestName.Golem)
        {
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[0].GetComponent<Item>(), 5);
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[1].GetComponent<Item>(), 5);
        }
        else if (name == Define.QuestName.Wood)
        {
            // ���� 10�� ���� ��
            player.Inventory.FindItemInInventory(Ingredients[1].GetComponent<Item>()).AddAmount(-10);
            // ������ 10���� �ٲ�����
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[0].GetComponent<Item>(), 10);
        }
        // ����Ʈ ����
        DeleteQuest(name);
    }
    #endregion
}
