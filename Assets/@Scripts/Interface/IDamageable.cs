using UnityEngine;

public interface IDamageable
{
    public void GetDamage(GameObject attacker, float damage, int bonusDamage = 1, Vector3 hitPos = default);
}
