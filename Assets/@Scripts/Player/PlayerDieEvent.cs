using UnityEngine;

public class PlayerDieEvent : MonoBehaviour
{
    // *** Animation Event activated when Player Die *** //
    public void OnDieAnimation()
    {
        Debug.Log("You Died...");
        // 게임 오버 UI 및 리스폰 버튼
        ResultPanel.ResultPanelAction?.Invoke(false);
    }
}
