using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
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

    bool _isGround = true;
    bool _isDashing = false;
    bool _isClimbing = false;

    Vector3 _camOffset = new Vector3(0, 5, -10);

    void Start()
    {
        // * 컴포넌트
        _character = transform.GetChild(0);
        _animator = _character.GetComponent<Animator>();
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        // * 콜라이더
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.center = new Vector3(0, 2.95f, 0);
        capsule.radius = 1f;
        capsule.height = 6f;

        // * 카메라
        _camera = Camera.main;
        _camAxis = new GameObject("CamAxis").transform;
        _camera.transform.parent = _camAxis;
        _camera.transform.position = _camOffset;
    }

    private void Update()
    {
        Jump();
        Zoom();
        CameraMove();

        RecoverStamina();

        // 대쉬
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isDashing)
        {
            StartCoroutine(Dash());
        }

        RaycastHit hit;
        if (Physics.Raycast(_character.position, _character.forward, out hit, 1f))
        {
            if (hit.collider.CompareTag("Climbable") && Input.GetKey(KeyCode.W))
            {
                _isClimbing = true;
                Vector3 newVelocity = _rigidbody.linearVelocity;
                newVelocity.y = _climeSpeed;
                _rigidbody.linearVelocity = newVelocity;
            }
        }
        Move();
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

        _camAxis.position = transform.position + new Vector3(0, 4, 0);
        _character.eulerAngles = new Vector3(0, _character.eulerAngles.y, 0);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGround)
        {
            Debug.Log("Jump");
            _isGround = false;
            _rigidbody.AddForce(Vector3.up * 500f);
        }
    }

    IEnumerator Dash()
    {
        if (_stamina > 0)
        {
            _isDashing = true;
            float originalSpeed = _speed;
            _speed = _dashSpeed;
            yield return new WaitForSeconds(_dashTime);
            _speed = originalSpeed;
            _isDashing = false;
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
        if (collision.gameObject.CompareTag("Floor"))
        {
            _isGround = true;
        }
    }

    void RecoverStamina()
    {
        if (!_isClimbing && !_isDashing)
        {
            _stamina += _staminaRegenRate * Time.deltaTime;
            _stamina = Mathf.Clamp(_stamina, 0, _maxStamina);
        }
    }

}
