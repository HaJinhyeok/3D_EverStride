using UnityEngine;

public class PlayerPotionEvent : MonoBehaviour
{
    PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }

    // *** Animation Event activated when Player Drink Potion *** //
    void OnPotionAnimationExit()
    {
        GameManager.Instance.Player.PlayerHp = Mathf.Min(Define.PlayerMaxHp, GameManager.Instance.Player.PlayerHp + 20f);
        // 포션 아이템 제거, 인벤토리에서 포션 -1
        GameObject currentItem = GameManager.Instance.Player.ItemPos.transform.GetChild(0).gameObject;
        GameManager.Instance.Player.Inventory.AddItem(currentItem.GetComponent<Item>(), -1);
        Destroy(currentItem);
    }
}
