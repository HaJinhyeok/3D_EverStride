using UnityEngine;

public class Axe : MonoBehaviour, IAttackable
{
    private void OnTriggerEnter(Collider other)
    {
        // 추후에 여유가 된다면, 히트포인트에 이펙트 넣어도 괜찮을듯
        DoAttack(other.gameObject, 10);
    }

    public bool DoAttack(GameObject target, float damage, Vector3 hitPos = default)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable == null) return false;

        damageable.GetDamage(gameObject, damage);
        // 나무에게는 추가 대미지
        if (target.GetComponent<TreeObject>())
            damageable.GetDamage(gameObject, damage);
        return true;
    }
}
