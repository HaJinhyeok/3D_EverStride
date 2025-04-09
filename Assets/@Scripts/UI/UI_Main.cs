using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    public GameObject MenuPanel;
    public Button StartButton;
    public Button QuitButton;
    public Text AnyKeyText;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClick);
        QuitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            AnyKeyText.gameObject.SetActive(false);
            MenuPanel.SetActive(true);
        }
    }

    void OnStartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(Define.GameScene);
    }

    void OnQuitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
