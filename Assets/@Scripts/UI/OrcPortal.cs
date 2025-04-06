using UnityEngine;

public class OrcPortal : MonoBehaviour
{
    public GameObject NoticePanelObject;

    string _orcText = "Orc";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            NoticePanelObject.SetActive(true);
            NoticePanel.NoticePanelAction?.Invoke(_orcText, "");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(Define.PlayerTag))
        {
            NoticePanelObject.SetActive(false);
        }
    }
}
