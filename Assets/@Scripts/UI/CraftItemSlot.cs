using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class CraftItemSlot : MonoBehaviour
{
    public ItemData ItemData;
    public Image CraftItemImage;
    public Text CraftItemText;

    void Start()
    {
        CraftItemImage.sprite = ItemData.Icon;
        CraftItemText.text = ItemData.ItemName;
    }

}
