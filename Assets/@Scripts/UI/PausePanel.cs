using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    public Button BackButton;
    public Button ExitButton;

    void Start()
    {
        BackButton.onClick.AddListener(OnBackButtonClick);
        ExitButton.onClick.AddListener(OnExitButtonClick);
        gameObject.SetActive(false);
    }

    void OnBackButtonClick()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsPaused = false;
        gameObject.SetActive(false);
    }

    void OnExitButtonClick()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.IsPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(Define.MainScene);
    }
}
