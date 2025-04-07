using UnityEngine;

public class PlayerDieEvent : MonoBehaviour
{
    // *** Animation Event activated when Player Die *** //
    public void OnDieAnimation()
    {
        Debug.Log("You Died...");
        // ���� ���� UI �� ������ ��ư
        ResultPanel.ResultPanelAction?.Invoke(false);
    }
}
