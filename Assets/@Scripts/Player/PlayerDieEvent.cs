using UnityEngine;

public class PlayerDieEvent : MonoBehaviour
{
    // *** Animation Event activated when Player Die *** //
    public void OnDieAnimation()
    {
        // ���� ���� UI �� ������ ��ư
        ResultPanel.ResultPanelAction?.Invoke(false);
    }
}
