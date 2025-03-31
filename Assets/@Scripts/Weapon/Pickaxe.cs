using UnityEngine;

public class Pickaxe : Weapon, IAttackable
{
    void Start()
    {
        _atk = 10;
    }
    private void OnTriggerEnter(Collider other)
    {
        // 추후에 여유가 된다면, 히트포인트에 이펙트 넣어도 괜찮을듯
        // 공격 중이고, 모션이 약간 진행된 상태여야 공격 판정
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

        // 돌에게는 추가 대미지
        if (target.GetComponent<RockObject>())
            damageable.GetDamage(gameObject, damage, 2);
        else
            damageable.GetDamage(gameObject, damage);
        return true;
    }
}
