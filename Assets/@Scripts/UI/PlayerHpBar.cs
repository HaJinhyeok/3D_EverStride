using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Image PlayerHpImage;
    public static Action PlayerHpAction;

    void Start()
    {
        PlayerHpAction += OnPlayerHpChanged;
        PlayerHpImage.fillAmount = GameManager.Instance.Player.PlayerHp / Define.PlayerMaxHp;
    }

    void OnPlayerHpChanged()
    {
        PlayerHpImage.fillAmount = GameManager.Instance.Player.PlayerHp / Define.PlayerMaxHp;
    }

    private void OnDisable()
    {
        PlayerHpAction -= OnPlayerHpChanged;
    }
}
