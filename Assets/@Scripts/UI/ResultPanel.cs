using System;
using UnityEngine;
using UnityEngine.UI;

public class ResultPanel : MonoBehaviour
{
    public Button ResultButton;
    public Text ResultButtonText;
    public Text ResultText;
    public static Action<bool> ResultPanelAction;

    void Start()
    {
        ResultPanelAction += ResultPanelOn;
        ResultButton.onClick.AddListener(OnResultButtonClick);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.IsUIOn = true;
    }

    void OnResultButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Define.GameScene);
    }

    void ResultPanelOn(bool victory)
    {
        if(victory)
        {
            ResultButtonText.text = "Back to Village";
            ResultText.text = "Victory!";
        }
        else
        {
            ResultButtonText.text = "Respawn";
            ResultText.text = "You Died...";
        }
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        GameManager.Instance.IsUIOn = false;
    }

    private void OnDestroy()
    {
        ResultPanelAction -= ResultPanelOn;
    }
}
