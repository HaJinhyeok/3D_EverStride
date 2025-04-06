using UnityEngine;

public class VillainPortal : MonoBehaviour
{
    public GameObject NoticePanelObject;

    string _villainText = "Villain";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            NoticePanelObject.SetActive(true);
            NoticePanel.NoticePanelAction?.Invoke(_villainText, "");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            NoticePanelObject.SetActive(false);
        }
    }
}
