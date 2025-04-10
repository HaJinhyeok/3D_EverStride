using System;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    public Image BossHpImage;
    public Text BossName;
    public static Action BossHpAction;

    GameObject _boss;
    BossController _bossController;
    float _maxHp;

    void Start()
    {
        _boss = GameObject.FindGameObjectWithTag(Define.EnemyTag);
        if (_bossController = _boss.GetComponent<GolemController>())
        {
            _maxHp = Define.GolemMaxHp;
        }
        else if (_bossController = _boss.GetComponent<OrcController>())
        {
            _maxHp = Define.OrcMaxHp;
        }
        BossHpImage.fillAmount = _bossController.Hp / _maxHp;
        BossName.text = _boss.name;
        BossHpAction += OnBossHpChanged;
    }

    void OnBossHpChanged()
    {
        BossHpImage.fillAmount = _bossController.Hp / _maxHp;
    }

    private void OnDestroy()
    {
        BossHpAction -= OnBossHpChanged;
    }
}
