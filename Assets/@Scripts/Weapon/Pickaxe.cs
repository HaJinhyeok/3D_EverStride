using UnityEngine;

public class Pickaxe : Weapon, IAttackable
{
    void Start()
    {
        _atk = 10;
    }
    private void OnTriggerEnter(Collider other)
    {
        // ���Ŀ� ������ �ȴٸ�, ��Ʈ����Ʈ�� ����Ʈ �־ ��������
        // ���� ���̰�, ����� �ణ ����� ���¿��� ���� ����
        if (!other.GetComponent<PlayerController>()&&_animator.GetBool(Define.IsAttacking) && _animator.GetBool(Define.InteractionHash))
        {
            DoAttack(other.gameObject, _atk);
            _animator.SetBool(Define.InteractionHash, false);
        }
    }

    public override bool DoAttack(GameObject target, float damage, Vector3 hitPos = default)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable == null) return false;

        // �����Դ� �߰� �����
        if (target.GetComponent<RockObject>())
            damageable.GetDamage(gameObject, damage, 2);
        else
            damageable.GetDamage(gameObject, damage);
        return true;
    }
}
