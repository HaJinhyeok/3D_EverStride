using UnityEngine;

public class Sword : MonoBehaviour, IAttackable
{
    private void OnTriggerEnter(Collider other)
    {
        // ���Ŀ� ������ �ȴٸ�, ��Ʈ����Ʈ�� ����Ʈ �־ ��������
        DoAttack(other.gameObject, 10);
    }

    public bool DoAttack(GameObject target, float damage, Vector3 hitPos = default)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable == null) return false;

        damageable.GetDamage(gameObject, damage);
        return true;
    }
}
