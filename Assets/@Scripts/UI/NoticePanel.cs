using System;
using UnityEngine;
using UnityEngine.UI;

public class NoticePanel : MonoBehaviour
{
    public Button RaidStartButton;
    public Text RaidText;

    string _raidSceneName;

    public static Action<string, string> NoticePanelAction;

    void Start()
    {
        NoticePanelAction += OnNoticePanel;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.IsUIOn = true;
    }

    void OnRaidStartButtonClick()
    {
        if(string.IsNullOrEmpty(_raidSceneName))
        {
            UI_Warning.Instance.WarningEffect(Define.NotReadyBoss);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_raidSceneName);
        }
    }

    void OnNoticePanel(string raidText, string raidScene)
    {
        RaidText.text = raidText;

        _raidSceneName = raidScene;
        RaidStartButton.onClick.RemoveAllListeners();
        RaidStartButton.onClick.AddListener(OnRaidStartButtonClick);
    }

    private void OnDisable()
    {
        GameManager.Instance.IsUIOn = false;
    }

    private void OnDestroy()
    {
        NoticePanelAction -= OnNoticePanel;
    }
}
