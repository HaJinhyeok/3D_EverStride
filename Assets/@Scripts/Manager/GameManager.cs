using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    PlayerController player;
    TrailRenderer weaponTrail;

    public Action OnWeaponChanged;
    public Action<bool> OnTrailActivate;

    // �� �Ѿ �� �÷��̾� ���� ��ġ Ȯ�ο�?
    public Define.GameState GameState = Define.GameState.Default;

    public PlayerController Player
    {
        get { return player; }
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

    public GameObject[] Weapons;
    // ���� ������ ������ Weapons�� �ε����� �˷��� �� �ֵ��� ��ųʸ�...?
    public Dictionary<ItemData, int> WeaponsMap;

    public void LoadResources()
    {
        Weapons = Resources.LoadAll<GameObject>("Prefabs/Weapon");

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
}
