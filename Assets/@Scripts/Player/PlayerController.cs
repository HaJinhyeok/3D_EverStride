using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Inventory Inventory;

    public Image Stamina;

    Transform _character;
    Animator _animator;
    Rigidbody _rigidbody;

    Camera _camera;
    Transform _camAxis;
    float _camSpeed = 8f;
    float _mouseX = 0f;
    float _mouseY = 4f;
    float _wheel = -10f;
    float _speed = 10f;
    float _climeSpeed = 5f;
    float _dashSpeed = 15f;
    float _dashTime = 1f;
    float _maxStamina = 100f;
    float _stamina = 100f;
    float _staminaRegenRate = 5f;

    bool _isClimbing = false;

    Vector3 _camOffset = new Vector3(0, 5, -5);

    public float Speed
    {
        get { return _animator.GetFloat(Define.Speed); }
        set { _animator.SetFloat(Define.Speed, value); }
    }

    public bool IsDash
    {
        get { return _animator.GetBool(Define.IsDash); }
        set { _animator.SetBool(Define.IsDash, value); }
    }

    [SerializeField]
    public bool IsGround
    {
        get { return _animator.GetBool(Define.IsGround); }
        set { _animator.SetBool(Define.IsGround, value); }
    }

    public bool IsClimbing
    {
        get { return _animator.GetBool(Define.IsClimbing); }
        set { _animator.SetBool(Define.IsClimbing, value); }
    }

    public bool IsNextCombo
    {
        get { return _animator.GetBool(Define.IsNextCombo); }
        set { _animator.SetBool(Define.IsNextCombo, value); }
    }

    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.IsAttacking); }
        set { _animator.SetBool(Define.IsAttacking, value); }
    }

    void Start()
    {
        // * 컴포넌트
        _character = transform.GetChild(0);
        _animator = _character.GetComponent<Animator>();
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        // * 콜라이더
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.center = new Vector3(0, 1f, 0);
        capsule.radius = 0.5f;
        capsule.height = 2f;

        // * 카메라
        _camera = Camera.main;
        _camAxis = new GameObject("CamAxis").transform;
        _camera.transform.parent = _camAxis;
        _camera.transform.position = _camOffset;
    }

    private void Update()
    {
        Jump();
        Attack();
        Zoom();
        CameraMove();

        RecoverStamina();

        // 대쉬
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsDash)
        {
            StartCoroutine(Dash());
        }

        Move();
        Roll();
    }

    #region Player Movement
    void Move()
    {
        if (_isClimbing && Input.GetKey(KeyCode.W))
        {
            _rigidbody.linearVelocity = Vector3.up * _climeSpeed * Time.deltaTime;
            _stamina -= Time.deltaTime * 5f;
            if (_stamina <= 0)
            {
                _isClimbing = false;
            }
        }
        else if (Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical))
        {
            float h = Input.GetAxis(Define.Horizontal);
            float v = Input.GetAxis(Define.Vertical);

            Vector3 movement = new Vector3(h, 0, v);
            float velY = _rigidbody.linearVelocity.y;
            transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);

            // local 방향에서 world 방향으로 바꿔주는 역할???
            Vector3 localMovement = transform.TransformDirection(movement);
            _rigidbody.linearVelocity = localMovement.normalized * _speed + new Vector3(0, velY, 0);

            _character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), 5 * Time.deltaTime);
        }
        else
        {
            // 속도 0으로 만들어주기
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        }

        Speed = _rigidbody.linearVelocity.sqrMagnitude;
        _camAxis.position = transform.position + new Vector3(0, 4, 0);
        _character.eulerAngles = new Vector3(0, _character.eulerAngles.y, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround && _rigidbody.linearVelocity.y <= 0)
        {
            IsGround = false;
            _animator.SetTrigger(Define.Jump);
            _rigidbody.AddForce(Vector3.up * 300f);
        }
    }

    IEnumerator Dash()
    {
        if (_stamina >= 20)
        {
            IsDash = true;
            IsDash = true;
            float originalSpeed = _speed;
            _speed = _dashSpeed;
            _stamina -= 20;
            yield return new WaitForSeconds(_dashTime);
            _speed = originalSpeed;
            IsDash = false;
            IsDash = false;
        }
    }

    void Roll()
    {
        if(Input.GetKeyDown(KeyCode.C)&&IsGround)
        {
        _animator.SetTrigger(Define.Roll);
        }
    }

    #endregion

    #region Player Attack
    void Attack()
    {
        // UI 클릭 여부 반환 함수
        bool isUIClick = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        bool isLeftMouseDown = Input.GetMouseButtonDown(0);
        if (isLeftMouseDown == false || isUIClick)
            return;
        if (IsAttacking == false)
        {
            IsAttacking = true;
        }
        else
        {
            IsNextCombo = true;
        }
    }
    #endregion

    #region Camera
    void Zoom()
    {
        _wheel += Input.GetAxis(Define.MouseScroll) * 10;
        if (_wheel >= -10)
            _wheel = -10;
        if (_wheel <= -20)
            _wheel = -20;

        _camera.transform.localPosition = new Vector3(0, 0, _wheel);
    }

    void CameraMove()
    {
        _mouseX += Input.GetAxis(Define.MouseX);
        _mouseY -= Input.GetAxis(Define.MouseY);

        if (_mouseY > 10)
            _mouseY = 10;
        if (_mouseY < 0)
            _mouseY = 0;

        _camAxis.rotation = Quaternion.Euler(new Vector3(_camAxis.rotation.x + _mouseY, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);
    }
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.GroundTag))
        {
            IsGround = true;
        }
    }

    void RecoverStamina()
    {
        if (!_isClimbing && !IsDash)
        {
            _stamina += _staminaRegenRate * Time.deltaTime;
            _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
        }
        Stamina.fillAmount = _stamina / _maxStamina;
    }

    public bool PickUpItem(Item item, int amount = -1)
    {
        if (item != null && Inventory.AddItem(item, amount))
        {
            Destroy(item.gameObject);
            return true;
        }
        return false;
    }
}
