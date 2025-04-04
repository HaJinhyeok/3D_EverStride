using UnityEngine;

public class GolemPortal : MonoBehaviour
{
    public GameObject GolemNoticePanel;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.PlayerTag))
        {
            GolemNoticePanel.SetActive(true);
        }
    }
}
