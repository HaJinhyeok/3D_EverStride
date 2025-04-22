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
    public Define.EquipmentStatus helmet;
    public Define.EquipmentStatus chest;
    public Define.EquipmentStatus shoulders;
    public Define.EquipmentStatus gloves;
    public Define.EquipmentStatus pants;
    public Define.EquipmentStatus boots;
}


public class GameManager : Singleton<GameManager>
{
    PlayerController player;
    TrailRenderer weaponTrail;

    bool _isExist = false;
    bool _isNPCInteractive = false;
    bool _isPossibleCraft = false;
    bool _isConversating = false;
    bool _isPaused = false;
    bool _isUiOn = false;

    public Action OnWeaponChanged;
    public Action<bool> OnTrailActivate;

    public Dictionary<Define.QuestName, Quest> QuestDictionary = new Dictionary<Define.QuestName, Quest>();

    public PlayerController Player
    {
        get { return player; }
    }

    #region Boolean Flags

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
        get { return _isUiOn; }
        set { _isUiOn = value; }
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
    public GameObject PreviewCharacter;

    // Inventory ������ ���� ����
    public Slot[] InventorySlots;
    public Slot[] ShortcutInventorySlots;
    GameObject Slot;

    // Equipment ���� ����
    public PlayerEquipment PlayerEquipment;

    private void Awake()
    {
        

        // starter base equipment
        PlayerEquipment.chest = Define.EquipmentStatus.Base;
        PlayerEquipment.pants = Define.EquipmentStatus.Base;
        PlayerEquipment.boots = Define.EquipmentStatus.Base;
        // no base equipment
        PlayerEquipment.helmet = Define.EquipmentStatus.None;
        PlayerEquipment.shoulders = Define.EquipmentStatus.None;
        PlayerEquipment.gloves = Define.EquipmentStatus.None;
    }

    public void FindPlayer()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();

    }

    public void LoadResources()
    {
        Slot = Resources.Load<GameObject>(Define.SlotPath);
        Weapons = Resources.LoadAll<GameObject>(Define.WeaponPath);
        Ingredients = Resources.LoadAll<GameObject>(Define.IngredientPath);
        ConsumptionItems = Resources.LoadAll<GameObject>(Define.ConsumptionPath);
        PreviewCharacter = Resources.Load<GameObject>(Define.PreviewCharacter);

        OnWeaponChanged += GetWeaponTrail;
        OnTrailActivate += ActivateWeaponTrail;
    }

    // GameManager Instance ����� �뵵
    public void Initiate() 
    {
        if (_isExist)
            return;
        _isExist = true;
        LoadResources();
        if (InventorySlots == null)
            InventorySlots = new Slot[25];
        if (ShortcutInventorySlots == null)
            ShortcutInventorySlots = new Slot[5];
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            GameObject go = Instantiate(Slot, transform);
            InventorySlots[i] = go.GetComponent<Slot>();
            go.name = "Slot : " + i;
        }
        for (int i = 0; i < ShortcutInventorySlots.Length; i++)
        {
            GameObject go = Instantiate(Slot, transform);
            ShortcutInventorySlots[i] = go.GetComponent<Slot>();
            go.name = "SrhotcutSlot : " + i;
        }

        UpdateTestItems();
    }

    // �κ��丮�� �ִ� ������ ��Ȳ ����
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

    // �κ��丮�� �����ϸ� �� ��ȯ ������ �׽�Ʈ ������ ����Ƿ�
    // ���ӸŴ������� ����
    public void UpdateTestItems()
    {
        // test�� ����
        for (int i = 0; i < 3; i++)
        {
            ShortcutInventorySlots[i].UpdateSlot(GameManager.Instance.Weapons[i + 1].GetComponent<WeaponItem>().ItemData, 1);
        }
        // test�� ���
        for (int i = 0; i < GameManager.Instance.Ingredients.Length; i++)
        {
            InventorySlots[5 + i].UpdateSlot(GameManager.Instance.Ingredients[i].GetComponent<Item>().ItemData, 10);
        }
        // test�� ����
        ShortcutInventorySlots[3].UpdateSlot(GameManager.Instance.ConsumptionItems[0].GetComponent<Item>().ItemData, 2);
        //ShortcutInventory.UpdateShortcutInventory(ShortcutInventorySlots);
    }

    // Ʈ���� ����Ʈ ���� Ȯ��
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
            UI_Warning.OnWarningEffect?.Invoke(Define.DuplicatedQuest);
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
        if (name == Define.QuestName.Wood)
        {
            // ���� 10�� ���� ��
            player.Inventory.FindItemInInventory(Ingredients[1].GetComponent<Item>()).AddAmount(-10);
            // ������ 10���� �ٲ�����
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[0].GetComponent<Item>(), 10);
        }
        else if (name == Define.QuestName.Golem)
        {
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[2].GetComponent<Item>(), 3);
        }
        else if (name == Define.QuestName.Orc)
        {
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[2].GetComponent<Item>(), 5);
        }
        // ����Ʈ ����
        DeleteQuest(name);
    }
    #endregion
}
