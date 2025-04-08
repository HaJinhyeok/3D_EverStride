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

public class GameManager : Singleton<GameManager>
{
    PlayerController player;
    TrailRenderer weaponTrail;

    bool _isNPCInteractive = false;
    bool _isConversating = false;

    public Action OnWeaponChanged;
    public Action<bool> OnTrailActivate;

    // 씬 넘어갈 때 플레이어 스폰 위치 확인용?
    public Define.GameState GameState = Define.GameState.Default;

    //public List<Quest> Quests = new List<Quest>();
    public Dictionary<Define.QuestName, Quest> QuestDictionary = new Dictionary<Define.QuestName, Quest>();

    public PlayerController Player
    {
        get { return player; }
    }

    #region Boolean
    public bool IsNPCInteracive
    {
        get { return _isNPCInteractive; }
        set { _isNPCInteractive = value; }
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
        get { return IsCraftPanelOn; }
        set { IsCraftPanelOn = value; }
    }
    #endregion

    public GameObject[] Weapons;
    public GameObject[] Ingredients;
    // 웨폰 데이터 넣으면 Weapons의 인덱스를 알려줄 수 있도록 딕셔너리...?
    public Dictionary<ItemData, int> WeaponsMap;

    public void LoadResources()
    {
        Weapons = Resources.LoadAll<GameObject>(Define.WeaponPath);
        Ingredients = Resources.LoadAll<GameObject>(Define.IngredientPath);

        player = GameObject.Find("Player").GetComponent<PlayerController>();
        OnWeaponChanged += GetWeaponTrail;
        OnTrailActivate += ActivateWeaponTrail;
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
                // 혹시 인벤토리에 이미 나무가 있으면 개수 더해줌
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
        // 삭제되었으면 업데이트
        if (len != QuestDictionary.Count)
        {
            QuestPanel.OnQuestPanelUpdate?.Invoke();
        }
    }

    public void CompleteQuest(Define.QuestName name)
    {
        // 보상 지급 후
        if (name == Define.QuestName.Golem)
        {
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[0].GetComponent<Item>(), 5);
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[1].GetComponent<Item>(), 5);
        }
        else if (name == Define.QuestName.Wood)
        {
            // 나무 10개 차감 후
            player.Inventory.FindItemInInventory(Ingredients[1].GetComponent<Item>()).AddAmount(-10);
            // 돌멩이 10개로 바꿔주자
            GameManager.Instance.Player.Inventory.AddItem(Ingredients[0].GetComponent<Item>(), 10);
        }
        // 퀘스트 삭제
        DeleteQuest(name);
    }
    #endregion
}
