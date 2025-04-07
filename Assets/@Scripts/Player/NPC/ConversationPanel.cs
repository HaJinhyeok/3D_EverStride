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
    public static Action<string[]> OnConversationStart;

    Text _npcName;
    GameObject _gameUI;

    void Start()
    {
        _npcName = GetComponentInChildren<Text>();
        _npcName.text = NPC.name;
        _gameUI = GameObject.Find(Define.GameUI);
        ConversationText.text = "";
        OnConversationStart += ConversationStart;

        YesButton.onClick.AddListener(() => StartCoroutine(OnYesButtonClick()));
        NoButton.onClick.AddListener(() => StartCoroutine(OnNoButtonClick()));
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
    }

    void ConversationStart(string[] comment)
    {
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCSuggest);
        StartCoroutine(CoWriteComment(comment, true));
    }

    IEnumerator OnYesButtonClick()
    {
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCPointing);
        yield return StartCoroutine(CoWriteComment(Define.NPC_GOOD, false));
        // 퀘스트 수락 후 카메라 종료
        ConversationCamera.gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    IEnumerator OnNoButtonClick()
    {
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCWhy);
        yield return StartCoroutine(CoWriteComment(Define.NPC_TOO_BAD, false));
        // 그냥 카메라 종료
        ConversationCamera.gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    IEnumerator CoWriteComment(string[] comment, bool buttonOn)
    {
        ConversationText.text = "";
        for (int i = 0; i < comment.Length; i++)
        {
            foreach (char ch in comment[i].ToCharArray())
            {
                ConversationText.text += ch;
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.G)));
        }

        YesButton.gameObject.SetActive(buttonOn);
        NoButton.gameObject.SetActive(buttonOn);
    }

    private void OnDestroy()
    {
        OnConversationStart -= ConversationStart;
    }
}
