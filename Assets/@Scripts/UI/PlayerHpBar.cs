using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    public Image PlayerHpImage;
    public Text PlayerHpText;
    public static Action PlayerHpAction;

    void Start()
    {
        PlayerHpAction += OnPlayerHpChanged;
        PlayerHpImage.fillAmount = GameManager.Instance.Player.PlayerHp / Define.PlayerMaxHp;
        PlayerHpText.text = $"{GameManager.Instance.Player.PlayerHp} / {Define.PlayerMaxHp}";
    }

    void OnPlayerHpChanged()
    {
        PlayerHpImage.fillAmount = GameManager.Instance.Player.PlayerHp / Define.PlayerMaxHp;
        PlayerHpText.text = $"{GameManager.Instance.Player.PlayerHp} / {Define.PlayerMaxHp}";
    }
}
