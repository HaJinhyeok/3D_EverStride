using UnityEngine;
using UnityEngine.UI;

public class GolemNoticePanel : MonoBehaviour
{
    public Button RaidStartButton;

    void Start()
    {
        RaidStartButton.onClick.AddListener(OnRaidStartButtonClick);
        gameObject.SetActive(false);
    }

    void OnRaidStartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Define.BossScene);
    }
}
