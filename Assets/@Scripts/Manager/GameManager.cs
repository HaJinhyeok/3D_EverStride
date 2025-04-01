using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    PlayerController player;

    public PlayerController Player
    {
        get { return player; }
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

    public GameObject[] Weapons;
    // ���� ������ ������ Weapons�� �ε����� �˷��� �� �ֵ��� ��ųʸ�...?
    public Dictionary<ItemData,int> WeaponsMap;

    public void LoadResources()
    {
        Weapons = Resources.LoadAll<GameObject>("Prefabs/Weapon");

        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
}
