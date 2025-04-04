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
        //PlayerHpImage = GetComponentInChildren<Image>();
        //PlayerHpText = GetComponentInChildren<Text>();
        PlayerHpAction += OnPlayerHpChanged;
        PlayerHpImage.fillAmount = GameManager.Instance.Player.PlayerHp / Define.PlayerMaxHp;
        PlayerHpText.text = $"{GameManager.Instance.Player.PlayerHp} / {Define.PlayerMaxHp}";
    }

    private void Update()
    {
        if(PlayerHpImage==null)
        {
            Debug.Log("개버그네 시발 왜사라짐");
        }
    }

    void OnPlayerHpChanged()
    {
        PlayerHpImage.fillAmount = GameManager.Instance.Player.PlayerHp / Define.PlayerMaxHp;
        PlayerHpText.text = $"{GameManager.Instance.Player.PlayerHp} / {Define.PlayerMaxHp}";
    }
}
