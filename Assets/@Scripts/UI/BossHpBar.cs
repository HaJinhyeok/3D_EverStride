using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public Image BossHpImage;
    public Text BossName;
    public static Action BossHpAction;

    GameObject _boss;

    void Start()
    {
        _boss = GameObject.FindGameObjectWithTag(Define.EnemyTag);
        BossHpImage.fillAmount = Define.GolemMaxHp / Define.GolemMaxHp;
        BossName.text = _boss.name;
        BossHpAction += OnBossHpChanged;
    }

    void OnBossHpChanged()
    {
        BossHpImage.fillAmount = _boss.GetComponent<GolemController>().Hp / Define.GolemMaxHp;
    }

    private void OnDestroy()
    {
        BossHpAction -= OnBossHpChanged;
    }
}
