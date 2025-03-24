using UnityEngine;

public abstract class EnvironmentObject : MonoBehaviour, IDroppable
{
    protected float _durability;

    public abstract void DropItems();

    public abstract void DropItemsOnDestroy();
}
