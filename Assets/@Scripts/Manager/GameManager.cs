using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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

    public GameObject[] Weapons;
    // 웨폰 데이터 넣으면 Weapons의 인덱스를 알려줄 수 있도록 딕셔너리...?
    public Dictionary<ItemData,int> WeaponsMap;

    public void LoadResources()
    {
        Weapons = Resources.LoadAll<GameObject>("Prefabs/Weapon");
    }
}
