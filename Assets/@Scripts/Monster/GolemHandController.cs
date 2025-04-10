using UnityEngine;

public class GolemHandController : MonoBehaviour
{
    public GameObject Monster;
    Animator _animator;

    private void Start()
    {
        _animator = Monster.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag) && _animator.GetBool(Define.IsAttacking) && _animator.GetBool(Define.InteractionHash))
        {
            other.GetComponent<PlayerController>().GetDamage(Monster, Monster.GetComponent<BossController>().Atk);
            _animator.SetBool(Define.InteractionHash, false);
        }
    }
}
