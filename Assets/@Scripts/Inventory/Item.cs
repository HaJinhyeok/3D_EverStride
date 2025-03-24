using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData ItemData;
    public int Amount = 1;

    public Item() { }
    public Item(ItemData itemData)
    {
        ItemData = itemData;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc == null)
            return;
        pc?.PickUpItem(this, Amount);
    }
}
