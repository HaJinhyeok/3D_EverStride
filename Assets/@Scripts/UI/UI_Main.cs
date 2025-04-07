using UnityEngine;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    public Button StartButton;
    public Button QuitButton;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClick);
        QuitButton.onClick.AddListener(OnQuitButtonClick);
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
