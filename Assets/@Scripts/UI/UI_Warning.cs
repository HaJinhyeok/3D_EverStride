using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Warning : Singleton<UI_Warning>
{
    Text WarningText;

    void Start()
    {
        WarningText = gameObject.GetComponentInChildren<Text>();
        WarningText.text = "";
        WarningText.gameObject.SetActive(false);
    }

    public void WarningEffect(string msg)
    {
        WarningText.text = msg;
        StartCoroutine(CoWarningEffect());
    }

    IEnumerator CoWarningEffect()
    {
        WarningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        WarningText.gameObject.SetActive(false);
    }
}
