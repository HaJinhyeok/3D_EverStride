using UnityEngine;

public abstract class Weapon : MonoBehaviour, IAttackable
{
    protected float _atk;
    protected Transform _player;
    protected Animator _animator;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform;
        _animator = _player.GetChild(0).GetComponent<Animator>();
    }

    public abstract bool DoAttack(GameObject target, float damage, Vector3 hitPos = default);
}
