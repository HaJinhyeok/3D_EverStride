using UnityEngine;

public class GolemPortal : MonoBehaviour
{
    public GameObject NoticePanelObject;

    string _golemTetxt = "Golem";

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Define.PlayerTag))
        {
            NoticePanelObject.SetActive(true);
            NoticePanel.NoticePanelAction?.Invoke(_golemTetxt, Define.GolemScene);
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
