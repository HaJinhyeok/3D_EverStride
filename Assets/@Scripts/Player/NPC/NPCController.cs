using UnityEngine;

public class NPCController : MonoBehaviour
{
    public GameObject InteractionPanel;

    float _sightDistance = 7f;
    float _interactionDistance = 2f;
    float _rotationSpeed = 5f;
    bool _isWatching = false;
    Transform _player;
    Animator _animator;

    void Start()
    {
        _player = GameManager.Instance.Player.transform;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if ( distance< _interactionDistance)
        {
            // 상호작용 활성화
            InteractionPanel.SetActive(true);
            GameManager.Instance.IsNPCInteracive = true;
        }
        else
        {
            InteractionPanel.SetActive(false);
            GameManager.Instance.IsNPCInteracive = false;
        }
        if (distance < _sightDistance)
        {
            if (!_isWatching)
            {
                _isWatching = true;
                NPCHey();
            }
            Rotate();
        }
        else
        {
            _isWatching = false;
        }
    }

    void Rotate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_player.position - transform.position), _rotationSpeed * Time.deltaTime);
    }

    public void NPCHey()
    {
        _animator.SetTrigger(Define.NPCHey);
    }

    public void NPCWhy()
    {
        _animator.SetTrigger(Define.NPCWhy);
    }

    public void NPCSuggest()
    {
        _animator.SetTrigger(Define.NPCSuggest);
    }

    public void NPCClapping()
    {
        _animator.SetTrigger(Define.NPCClapping);
    }

    public void NPCPointing()
    {
        _animator.SetTrigger(Define.NPCPointing);
    }
}
