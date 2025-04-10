using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GolemController : BossController
{
    //public GameObject HitEffect;

    //Animator _animator;
    //Rigidbody _rigidbody;
    //Transform _player;

    //float _attackRange = 2f;
    //float _speed = 3f;
    //float _hp;
    //float _atk = 10;
    //[Header("SightRange")]
    //public float sightRange = 15f;

    //#region Animator
    //public float Atk
    //{
    //    get { return _atk; }
    //}

    //public float Speed
    //{
    //    get { return _animator.GetFloat(Define.Speed); }
    //    set { _animator.SetFloat(Define.Speed, value); }
    //}

    //public bool IsAttacking
    //{
    //    get { return _animator.GetBool(Define.IsAttacking); }
    //    set { _animator.SetBool(Define.IsAttacking, value); }
    //}
    //#endregion

    //public float Hp
    //{
    //    get { return _hp; }
    //}

    void Start()
    {
        //_animator = GetComponent<Animator>();
        //_rigidbody = GetComponent<Rigidbody>();
        //_player = GameObject.Find("Player").transform;
        _attackRange = 1.5f;
        _speed = 3f;
        _hp = Define.GolemMaxHp;
    }

    public override void Die()
    {
        base.Die();
        foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
        {
            if (quest.Value.name == Define.QuestName.Golem)
            {
                quest.Value.AddNum(1);
            }
        }
    }
    //void FixedUpdate()
    //{
    //    Move();
    //}

    //void Move()
    //{
    //    if (Vector3.Distance(transform.position, _player.position) < _attackRange || _hp <= 0)
    //    {
    //        IsAttacking = true;
    //        _rigidbody.linearVelocity = Vector3.zero;
    //    }
    //    else
    //    {
    //        //IsAttacking = false;
    //        // 플레이어가 일정 거리 내에 있으면 움직임
    //        if (Vector3.Distance(transform.position, _player.position) < sightRange && !IsAttacking)
    //        {
    //            MoveToPlayer();
    //        }
    //    }
    //    if (_hp > 0)
    //    {
    //        RotateToPlayer();
    //    }
    //    // 위로 떠오르지 않게
    //    RaycastHit hitGround;
    //    if (Physics.Raycast(transform.position, -transform.up, out hitGround, 5f))
    //    {
    //        Vector3 currentPos = _rigidbody.position;
    //        currentPos.y = hitGround.point.y;
    //        _rigidbody.position = currentPos;
    //    }
    //    Speed = _rigidbody.linearVelocity.sqrMagnitude;
    //}

    //void RotateToPlayer()
    //{
    //    Vector3 direction = (_player.transform.position - transform.position).normalized;
    //    Quaternion rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    //    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
    //}

    //void MoveToPlayer()
    //{
    //    Vector3 movement = Vector3.forward;
    //    float currentY = _rigidbody.linearVelocity.y;
    //    Vector3 localMovement = transform.TransformDirection(movement).normalized * _speed;
    //    _rigidbody.linearVelocity = localMovement + new Vector3(0, currentY, 0);
    //}

    //public void GetDamage(GameObject attacker, float damage, int bonusDamage = 1, Vector3 hitPos = default)
    //{
    //    if (_animator.GetBool(Define.Die)) return;
    //    _hp -= damage * bonusDamage;
    //    Instantiate(HitEffect, hitPos, Quaternion.Euler(transform.TransformDirection(Vector3.back)));
    //    BossHpBar.BossHpAction?.Invoke();
    //    _animator.SetTrigger(Define.TakeDamage);
    //    _animator.SetBool(Define.InteractionHash, false);
    //    Debug.Log($"Current Golem HP: {_hp}, Attacker name: {attacker.name}");
    //    if (_hp <= 0)
    //    {
    //        Debug.Log("Golem Die");
    //        _animator.SetBool(Define.Die, true);
    //    }
    //}

    //public void Die()
    //{
    //    Destroy(gameObject, 2f);
    //    ResultPanel.ResultPanelAction?.Invoke(true);
    //    foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
    //    {
    //        if (quest.Value.name == Define.QuestName.Golem)
    //        {
    //            quest.Value.AddNum(1);
    //        }
    //    }
    //}
}
