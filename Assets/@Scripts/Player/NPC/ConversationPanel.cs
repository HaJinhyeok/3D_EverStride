using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour
{
    public GameObject NPC;
    public Button YesButton;
    public Button NoButton;
    public Button ExitButton;
    public Camera ConversationCamera;
    public Text ConversationText;
    public Text NpcName;

    public static Action<string[], bool, bool> OnConversationStart;
    public static Action OnConversationExit;

    public Quest CompletedQuest;

    string[] _currentContext;
    GameObject _gameUI;

    void Start()
    {
        NpcName.text = NPC.name;
        _gameUI = GameObject.Find(Define.GameUI);
        CompletedQuest = null;
        ConversationText.text = "";
        OnConversationStart += ConversationStart;
        OnConversationExit += OnExitButtonClick;

        YesButton.onClick.AddListener(() => StartCoroutine(OnYesButtonClick()));
        NoButton.onClick.AddListener(() => StartCoroutine(OnNoButtonClick()));
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
        ExitButton.onClick.AddListener(OnExitButtonClick);
        gameObject.SetActive(false);
    }

    void ConversationStart(string[] comment, bool yesFlag, bool noFlag)
    {
        foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
        {
            // 완료된 퀘스트가 있으면
            if (quest.Value.isComlete)
            {
                CompletedQuest = quest.Value;
                break;
            }
        }
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCSuggest);
        StartCoroutine(CoWriteComment(comment, yesFlag, noFlag));
    }

    IEnumerator OnYesButtonClick()
    {
        // 퀘스트 종류를 인식할 수 있는 정보가 필요함
        // 현재 완료된 퀘스트가 없으면
        if (CompletedQuest == null)
        {
            if (_currentContext == Define.NPC_Quest_Wood)
            {
                GameManager.Instance.MakeQuest(Define.QuestName.Wood, Define.WoodQuest, 10);
            }
            else if (_currentContext == Define.NPC_Quest_Golem)
            {
                GameManager.Instance.MakeQuest(Define.QuestName.Golem, Define.GolemQuest, 1);
            }
            else if (_currentContext == Define.NPC_Quest_Orc)
            {
                GameManager.Instance.MakeQuest(Define.QuestName.Orc, Define.OrcQuest, 1);
            }
            NPC.GetComponent<Animator>().SetTrigger(Define.NPCPointing);
            yield return StartCoroutine(CoWriteComment(Define.NPC_GOOD, false, false));
        }
        else
        {
            NPC.GetComponent<Animator>().SetTrigger(Define.NPCClapping);
            GameManager.Instance.CompleteQuest(CompletedQuest.name);
            CompletedQuest = null;
            yield return StartCoroutine(CoWriteComment(Define.NPC_BYE, false, false));
        }
        // 수락 후 카메라 종료
        ConversationCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    IEnumerator OnNoButtonClick()
    {
        NPC.GetComponent<Animator>().SetTrigger(Define.NPCWhy);
        yield return StartCoroutine(CoWriteComment(Define.NPC_TOO_BAD, false, false));
        // 그냥 카메라 종료
        ConversationCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    void OnExitButtonClick()
    {
        StopAllCoroutines();
        ConversationCamera.gameObject.SetActive(false);
        gameObject.SetActive(false);
        GameManager.Instance.IsConversating = false;
        _gameUI.SetActive(true);
    }

    IEnumerator CoWriteComment(string[] comment, bool yesFlag, bool noFlag)
    {
        ConversationText.text = "";
        _currentContext = comment;
        for (int i = 0; i < comment.Length; i++)
        {
            foreach (char ch in comment[i].ToCharArray())
            {
                ConversationText.text += ch;
                yield return new WaitForSeconds(0.1f);
            }
            if (i != comment.Length - 1)
            {
                yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.G)));
            }
        }

        YesButton.gameObject.SetActive(yesFlag);
        NoButton.gameObject.SetActive(noFlag);
        yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.G)));
    }

    private void OnDestroy()
    {
        OnConversationStart -= ConversationStart;
        OnConversationExit -= OnExitButtonClick;
    }
}
