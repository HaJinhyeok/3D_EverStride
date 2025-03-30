using UnityEngine;

public interface IAttackable
{
    public bool DoAttack(GameObject target, float damage, Vector3 hitPos = default);
}
