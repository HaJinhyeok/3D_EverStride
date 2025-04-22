using UnityEngine;

public class Axe : Weapon
{
    void Start()
    {
        _atk = 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 공격 중이고, 모션이 약간 진행된 상태여야 공격 판정
        if (!other.GetComponent<PlayerController>() && _animator.GetBool(Define.IsAttacking) && _animator.GetBool(Define.InteractionHash))
        {
            DoAttack(other.gameObject, _atk, this.transform.position);
            _animator.SetBool(Define.InteractionHash, false);
        }
    }

    public override bool DoAttack(GameObject target, float damage, Vector3 hitPos = default)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        if (damageable == null) return false;

        // 나무에게는 추가 대미지
        if (target.GetComponent<TreeObject>())
            damageable.GetDamage(gameObject, damage, 2, hitPos);
        else
            damageable.GetDamage(gameObject, damage, 1, hitPos);
        return true;
    }
}
