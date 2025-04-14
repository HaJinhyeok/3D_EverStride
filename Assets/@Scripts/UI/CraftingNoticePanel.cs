using System;
using UnityEngine;
using UnityEngine.UI;

public class CraftingNoticePanel : MonoBehaviour
{
    public Text CraftingNoticeText;
    public Image CraftingNoticeImage;

    public static Action<ItemData,float> OnCraftingNoticeAction;

    ItemData _currentItem;
    float _currentTime;
    float _completeTime;

    private void Awake()
    {
        OnCraftingNoticeAction += CraftingNotice;
    }

    void CraftingNotice(ItemData data, float completeTime)
    {
        gameObject.SetActive(true);
        _currentTime = 0f;
        _currentItem = data;
        CraftingNoticeText.text = $"{data.ItemName} Á¦ÀÛ Áß...";
        _completeTime = completeTime;
    }

    void Update()
    {
        _currentTime += Time.deltaTime;
        CraftingNoticeImage.fillAmount = _currentTime / _completeTime;
        if(_currentTime>=_completeTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        OnCraftingNoticeAction -= CraftingNotice;
    }
}
