using UnityEngine;

public interface IDamageable
{
    public void GetDamage(GameObject attacker, float damage, Vector3 hitPos = default);
}
