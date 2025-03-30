using UnityEngine;

public abstract class EnvironmentObject : MonoBehaviour, IDroppable, IDamageable
{
    protected float _durability;

    public abstract void DropItem();

    public abstract void DropItemsOnDestroy();

    public abstract void GetDamage(GameObject attacker, float damage, Vector3 hitPos = default);
}
