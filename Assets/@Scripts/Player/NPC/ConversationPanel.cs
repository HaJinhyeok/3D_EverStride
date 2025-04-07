using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour
{
    public GameObject NPC;
    public Button YesButton;
    public Button NoButton;
    public Camera ConversationCamera;
    public Text ConversationText;
    public static Action<string> OnConversationStart;

    Text _npcName;
    GameObject _gameUI;
    string _currentComment = Define.NPCHello;

    void Start()
    {
        _npcName = GetComponentInChildren<Text>();
        _npcName.text = NPC.name;
        _gameUI = GameObject.Find(Define.GameUI);
        ConversationText.text = "";
        OnConversationStart += ConversationStart;

        YesButton.onClick.AddListener(OnYesButtonClick);
        NoButton.onClick.AddListener(OnNoButtonClick);
    }

    void ConversationStart(string comment)
    {
        StartCoroutine(CoWriteComment(comment));
    }

    void OnYesButtonClick()
    {
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCPointing);
        // 퀘스트 수락 후 카메라 종료
        ConversationCamera.gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    void OnNoButtonClick()
    {
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCWhy);
        // 그냥 카메라 종료
        ConversationCamera.gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    IEnumerator CoWriteComment(string comment)
    {
        foreach (char ch in comment.ToCharArray())
        {
            ConversationText.text += ch;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void OnDestroy()
    {
        OnConversationStart-= ConversationStart;
    }
}
