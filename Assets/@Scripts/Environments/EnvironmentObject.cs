using UnityEngine;

public abstract class EnvironmentObject : MonoBehaviour, IDroppable, IDamageable
{
    protected float _durability;
    protected AudioSource _audioSource;

    public abstract void DropItem();

    public abstract void DropItemsOnDestroy();

    public abstract void GetDamage(GameObject attacker, float damage, int bonusDamage = 1, Vector3 hitPos = default);
    
}
