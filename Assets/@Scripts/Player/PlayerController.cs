using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Inventory Inventory;
    public GameObject CraftPanel;
    public Image Stamina;
    public GameObject WeaponPos;
    public GameObject[] Weapons;
    public List<ItemData> IngredientData = new List<ItemData>();

    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    Transform _character;
    Animator _animator;
    Rigidbody _rigidbody;

    Camera _camera;
    Transform _camAxis;
    float _camSpeed = 8f;
    float _mouseX = 0f;
    float _mouseY = 4f;
    float _wheel = -10f;
    float _speed = 5f;
    float _dashSpeed = 15f;
    float _dashTime = 1f;
    float _maxStamina = 100f;
    float _stamina = 100f;
    float _staminaRegenRate = 5f;

    [SerializeField]
    Vector3 _camOffset = new Vector3(0f, 2f, 0f);

    //const float RAY_DISTANCE = 2000f;
    //RaycastHit slopeHit;
    //int groundLayer;

    #region Animation
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

    //public bool IsNextCombo
    //{
    //    get { return _animator.GetBool(Define.IsNextCombo); }
    //    set { _animator.SetBool(Define.IsNextCombo, value); }
    //}

    public bool IsAttacking
    {
        get { return _animator.GetBool(Define.IsAttacking); }
        set { _animator.SetBool(Define.IsAttacking, value); }
    }

    public int WeaponTypeHash
    {
        get { return _animator.GetInteger(Define.WeaponTypeHash); }
        set { _animator.SetInteger(Define.WeaponTypeHash, value); }
    }
    #endregion

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
        _camera.transform.position = new Vector3(0, 0, -3);

        for (int i = 0; i < IngredientData.Count; i++)
        {
            Define.IngredientData.Add(IngredientData[i].IngredientType, IngredientData[i]);
        }
        //InventoryOnOff(false);
        InventoryOnOff(true);
        CraftPanelOnOff(true);

        //WeaponTypeHash = -1; // 맨손
        //WeaponTypeHash = 0; // 돌멩이
        WeaponTypeHash = 1; // 도끼

        //groundLayer = 1 << LayerMask.NameToLayer(Define.GroundTag);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsCraftPanelOn)
        {
            Jump();
            Attack();
            CameraMove();

            // 대쉬
            if (Input.GetKeyDown(KeyCode.LeftShift) && !IsDash)
            {
                StartCoroutine(Dash());
            }



            Move();
            Roll();
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            CraftPanelOnOff(GameManager.Instance.IsCraftPanelOn);            
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryOnOff(GameManager.Instance.IsInventoryOn);
        }

        RecoverStamina();
    }

    private void FixedUpdate()
    {
        stepClimb();
    }

    #region Player Movement
    void Move()
    {
        //bool isOnSlope = IsOnSlope();
        if (Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical))
        {
            float h = Input.GetAxis(Define.Horizontal);
            float v = Input.GetAxis(Define.Vertical);

            Vector3 movement = new Vector3(h, 0, v);
            float velY = _rigidbody.linearVelocity.y;
            transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);

            // local 방향에서 world 방향으로 바꿔주는 역할???
            Vector3 localMovement = transform.TransformDirection(movement);

            //localMovement = isOnSlope ? ProjectDirectionToSlope(localMovement) : localMovement;
            // Vector3 tmpGravity = isOnSlope ? Vector3.zero : Vector3.up * _rigidbody.linearVelocity.y;

            _rigidbody.linearVelocity = localMovement.normalized * _speed + new Vector3(0, velY, 0);
            //_rigidbody.linearVelocity = localMovement.normalized * _speed + tmpGravity;

            _character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), 5 * Time.deltaTime);
        }
        else
        {
            // 속도 0으로 만들어주기
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
        }

        Speed = _rigidbody.linearVelocity.sqrMagnitude;
        _camAxis.position = transform.position + _camOffset;
        _character.eulerAngles = new Vector3(0, _character.eulerAngles.y, 0);
    }
    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                _rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                _rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.2f))
            {
                _rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGround)
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
        if (Input.GetKeyDown(KeyCode.C) && IsGround)
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
            //IsNextCombo = true;
        }
    }
    #endregion

    #region Camera
    void Zoom()
    {
        _wheel += Input.GetAxis(Define.MouseScroll) * 10;
        if (_wheel >= -2)
            _wheel = -2;
        if (_wheel <= -5)
            _wheel = -5;

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
        if (!IsDash)
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

    public void EquipWeapon(Define.WeaponType type)
    {
        GameObject currentWeapon = WeaponPos.transform.GetChild(0).gameObject;
        if (currentWeapon != null)
        {
            // 장착해제 해주고 인벤토리로 반환
            // UnequipWeapon();
        }
        GameObject weapon = Instantiate(Weapons[(int)type], WeaponPos.transform);
        WeaponTypeHash = (int)type;
    }

    public Define.WeaponType UnequipWeapon()
    {
        GameObject weapon = WeaponPos.transform.GetChild(0).gameObject;
        Define.WeaponType weaponType = weapon.GetComponent<Weapon>().GetWeaponType();
        Destroy(weapon);
        if (weapon == null)
            return Define.WeaponType.None;

        return weaponType;
    }

    void CraftPanelOnOff(bool state)
    {
        GameManager.Instance.IsCraftPanelOn = !state;
        CraftPanel.gameObject.SetActive(!state);
    }

    void InventoryOnOff(bool state)
    {
        GameManager.Instance.IsInventoryOn = !state;
        Inventory.gameObject.SetActive(!state);
    }


    // 플레이어가 현재 경사면에 있는지 확인
    //public bool IsOnSlope()
    //{
    //    Ray ray=new Ray(transform.position, Vector3.down);
    //    if(Physics.Raycast(ray,out slopeHit,RAY_DISTANCE,groundLayer))
    //    {
    //        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
    //        return angle != 0;
    //    }
    //    return false;
    //}

    //public Vector3 ProjectDirectionToSlope(Vector3 direction)
    //{
    //    return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    //}
}
