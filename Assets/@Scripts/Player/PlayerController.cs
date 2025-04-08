using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    public Camera CraftCamera;
    public Camera PreviewCamera;
    public Camera ConversationCamera;
    public GameObject CraftPanel;
    public GameObject ConversationPanelObejct;
    public Inventory Inventory;
    public ShortcutInventory Shortcut;
    public Image Stamina;
    public GameObject WeaponPos;
    public List<ItemData> IngredientData = new List<ItemData>();

    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    //[SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    Transform _character;
    Animator _animator;
    Rigidbody _rigidbody;
    Camera _camera;
    Transform _camAxis;
    LayerMask _npcMask;

    float _camSpeed = 6f;
    float _rotationSpeed = 10f;
    float _mouseX = 0f;
    float _mouseY = 4f;
    float _wheel = -10f;
    float _speed = 5f;
    float _dashSpeed = 10f;
    float _dashTime = 1f;
    float _maxStamina = 100f;
    float _stamina = 100f;
    float _staminaRegenRate = 5f;
    float _hp;
    // 마을맵인지 보스전맵인지 확인
    bool _isBossRaid;

    [SerializeField]
    Vector3 _camOffset = new Vector3(0f, 1f, -1f);

    //const float RAY_DISTANCE = 2000f;
    //RaycastHit slopeHit;
    //int groundLayer;

    public float PlayerHp
    {
        get { return _hp; }
        set
        {
            _hp = value;
        }
    }

    #region Animator
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

    public int WeaponTypeHash
    {
        get { return _animator.GetInteger(Define.WeaponTypeHash); }
        set { _animator.SetInteger(Define.WeaponTypeHash, value); }
    }

    //public bool IsCombatMode
    //{
    //    get { return _animator.GetBool(Define.IsCombatMode); }
    //    set { _animator.SetBool(Define.IsCombatMode, value); }
    //}
    #endregion

    // Player Setting On Awake
    void Awake()
    {
        // * 컴포넌트
        _character = transform.GetChild(0);
        _animator = _character.GetComponent<Animator>();
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        if (SceneManager.GetActiveScene().name == Define.BossScene)
        {
            _isBossRaid = true;
            CraftCamera = null;
            PreviewCamera = null;
            ConversationCamera = null;
            //IsCombatMode = true;
            _animator.runtimeAnimatorController = Resources.Load(Define.BossRaidAnimatorPath) as RuntimeAnimatorController;
        }
        else
        {
            _isBossRaid = false;
            //IsCombatMode = false;
        }

        // * 콜라이더
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.center = new Vector3(0, 1f, 0);
        capsule.radius = 0.5f;
        capsule.height = 2f;

        // * 카메라
        _camera = Camera.main;
        _camAxis = new GameObject("CamAxis").transform;
        _camera.transform.parent = _camAxis;
        //_camera.transform.localPosition = Vector3.zero;
        _camera.transform.localPosition = new Vector3(0, 0, -3);
        ConversationCamera?.gameObject.SetActive(false);

        // * 그 외
        _npcMask = LayerMask.GetMask(Define.NPCMask);
        for (int i = 0; i < IngredientData.Count; i++)
        {
            if (!Define.IngredientData.ContainsKey(IngredientData[i].IngredientType))
                Define.IngredientData.Add(IngredientData[i].IngredientType, IngredientData[i]);
        }
        //InventoryOnOff(false);
        InventoryOn(false);
        CraftUIOn(false);
        GameManager.Instance.LoadResources();
        // 보스전에서 칼로 놀다가 마을 복귀 후 주먹질 할때 에러 막기용
        GameManager.Instance.OnWeaponChanged.Invoke();
        Inventory.UpdateTestWeapons();

        _hp = Define.PlayerMaxHp;
        WeaponTypeHash = (int)GetCurrentWeaponType();

        //groundLayer = 1 << LayerMask.NameToLayer(Define.GroundTag);
    }

    private void Update()
    {
        // 대화 중엔 움직임 불가능
        if (!GameManager.Instance.IsConversating)
        {
            Jump();
            Attack();
            Move();
            Roll();

            InteractNPC();

            // 대쉬
            if (Input.GetKeyDown(KeyCode.LeftShift) && !IsDash)
            {
                StartCoroutine(Dash());
            }

            // x를 눌러 무장해제
            if (Input.GetKeyDown(KeyCode.X))
            {
                UnequipWeapon();
            }
            // ui 켜지면 카메라 정신사납지 않게 가만히
            if (!GameManager.Instance.IsUIOn)
            {
                CameraMove();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                CraftUIOn(!GameManager.Instance.IsCraftPanelOn);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                InventoryOn(!GameManager.Instance.IsInventoryOn);
            }
            // 1~5번 단축키
            UseShortcut();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ConversationPanel.OnConversationExit?.Invoke();
            }
        }

        // 스태미나 회복
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
        if ((Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical)) && !IsAttacking)
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

            _character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), _rotationSpeed * Time.deltaTime);
        }
        else if ((Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical)) && IsAttacking)
        {
            float h = Input.GetAxis(Define.Horizontal);
            float v = Input.GetAxis(Define.Vertical);

            transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);
            //Vector3 movement = new Vector3(h, 0, v);
            //_character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), _rotationSpeed * Time.deltaTime);
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
            _rigidbody.AddForce(Vector3.up * 200f);
        }
    }

    IEnumerator Dash()
    {
        if (_stamina >= 20 && WeaponTypeHash == -1)
        {
            IsDash = true;
            float originalSpeed = _speed;
            _speed = _dashSpeed;
            _stamina -= 20;
            yield return new WaitForSeconds(_dashTime);
            _speed = originalSpeed;
            IsDash = false;
        }
    }

    void Roll()
    {
        // 구르기는 보스전에서만 사용 가능
        if (!_isBossRaid) return;
        if (Input.GetKeyDown(KeyCode.C) && IsGround)
        {
            if (_stamina >= Define.PlayerRollStamina && !_animator.GetBool(Define.NoDamageMode))
            {
                _stamina -= Define.PlayerRollStamina;
                _animator.SetTrigger(Define.Roll);
                _animator.SetBool(Define.IsAttacking, false);
            }
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
        if (!IsGround)
            return;
        if (IsAttacking == false)
        {
            IsAttacking = true;
            GameManager.Instance.OnTrailActivate?.Invoke(true);
        }
        else
        {
            IsNextCombo = true;
            GameManager.Instance.OnTrailActivate?.Invoke(true);
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

    #region Player Utility


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

    void UseShortcut()
    {
        if (Input.GetKeyDown("1"))
        {
            //EquipWeapon(GameManager.Instance.Weapons[1].GetComponent<WeaponItem>());
            EquipWeapon(Shortcut.UseShortcutItem(0), 0);
        }
        if (Input.GetKeyDown("2"))
        {
            EquipWeapon(Shortcut.UseShortcutItem(1), 1);
        }
        if (Input.GetKeyDown("3"))
        {
            EquipWeapon(Shortcut.UseShortcutItem(2), 2);
        }
        if (Input.GetKeyDown("4"))
        {
            EquipWeapon(Shortcut.UseShortcutItem(3), 3);
        }
        if (Input.GetKeyDown("5"))
        {
            EquipWeapon(Shortcut.UseShortcutItem(4), 4);
        }
    }

    void InteractNPC()
    {
        if (Input.GetKeyDown(KeyCode.G) && GameManager.Instance.IsNPCInteracive)
        {
            Collider[] NPC = Physics.OverlapSphere(transform.position, 2f, _npcMask);
            NPC[0].transform.rotation = Quaternion.LookRotation(transform.position - NPC[0].transform.position);
            LookNPC(NPC[0].transform.position);
            // 대화용 카메라 ON
            ConversationCamera.gameObject.SetActive(true);
            ConversationPanelObejct.gameObject.SetActive(true);
            GameObject.Find(Define.GameUI).SetActive(false);
            GameManager.Instance.IsConversating = true;

            // 퀘스트 없으면 새로 주고
            if (GameManager.Instance.QuestDictionary.Count == 0)
            {
                int rnd = Random.Range(0, 2);
                if (rnd == 0)
                {
                    ConversationPanel.OnConversationStart(Define.NPC_Quest_Wood, true, true);
                }
                else
                {
                    ConversationPanel.OnConversationStart(Define.NPC_Quest_Golem, true, true);
                }
            }
            else
            {
                bool isDone = false;
                foreach (KeyValuePair<Define.QuestName, Quest> quest in GameManager.Instance.QuestDictionary)
                {
                    // 완료된 퀘스트가 있으면
                    if (quest.Value.isComlete)
                    {
                        isDone = true;
                        break;
                    }
                }
                if (isDone)
                {
                    ConversationPanel.OnConversationStart(Define.NPC_QUEST_COMPLETE, true, false);
                }
                else
                {
                    ConversationPanel.OnConversationStart(Define.NPC_QUEST_ING, false, false);
                }
            }
        }
    }

    void LookNPC(Vector3 pos)
    {
        transform.position = pos - new Vector3(0, 0, -3);
        _character.rotation = Quaternion.LookRotation(pos - transform.position);
    }
    #endregion

    #region Weapon
    public void EquipWeapon(Slot currentSlot, int idx)
    {
        //Slot slot = Inventory.FindItemInInventory(weapon);
        //if (slot == null)
        //{
        //    Debug.Log($"인벤토리에 {weapon.ItemData.name}이(가) 없습니다");
        //    return;
        //}
        if (currentSlot.ItemData == null)
            return;
        // 장비 아이템일 경우
        if (currentSlot.ItemData.ItemType == Define.ItemType.Equipment)
        {
            if (WeaponPos.transform.childCount > 0)
            {
                // 장착해제
                UnequipWeapon();
            }
            GameObject newWeapon = Instantiate(GameManager.Instance.Weapons[(int)currentSlot.ItemData.WeaponType], WeaponPos.transform);
            WeaponTypeHash = (int)currentSlot.ItemData.WeaponType;
            GameManager.Instance.OnWeaponChanged?.Invoke();
        }
        // 소비 및 재료 아이템일 경우
        else if (currentSlot.ItemData.ItemType == Define.ItemType.Countable)
        {
            Debug.Log("할 동작이 없습니다");
        }

        //slot.UpdateSlot(null, 0);
    }

    public Define.WeaponType UnequipWeapon()
    {
        if (WeaponPos.transform.childCount == 0)
        {
            Debug.Log("무기를 장착하고 있지 않습니다");
            return Define.WeaponType.None;
        }
        GameObject weapon = WeaponPos.transform.GetChild(0).gameObject;
        if (weapon == null)
            return Define.WeaponType.None;
        Define.WeaponType weaponType = weapon.GetComponent<WeaponItem>().GetWeaponType();
        //Inventory.AddItem(weapon.GetComponent<WeaponItem>(), 1);
        Destroy(weapon);
        // 맨손
        WeaponTypeHash = -1;
        GameManager.Instance.OnWeaponChanged?.Invoke();

        return weaponType;
    }

    public Define.WeaponType GetCurrentWeaponType()
    {
        if (WeaponPos.transform.childCount == 0)
        {
            return Define.WeaponType.None;
        }
        else
        {
            return WeaponPos.transform.GetChild(0).gameObject.GetComponent<WeaponItem>().ItemData.WeaponType;
        }
    }
    #endregion

    #region UI
    void CraftUIOn(bool state)
    {
        if (CraftPanel == null)
            return;
        GameManager.Instance.IsCraftPanelOn = state;
        GameManager.Instance.IsUIOn = state;
        CraftCamera?.gameObject.SetActive(state);
        CraftPanel?.SetActive(state);
        PreviewCamera?.gameObject.SetActive(state);
    }

    void InventoryOn(bool state)
    {
        GameManager.Instance.IsInventoryOn = state;
        GameManager.Instance.IsUIOn = state;
        Inventory.gameObject.SetActive(state);
    }
    #endregion

    public void GetDamage(GameObject attacker, float damage, int bonusDamage = 1, Vector3 hitPos = default)
    {
        if (_hp <= 0) return;
        if (_animator.GetBool(Define.NoDamageMode))
        {
            Debug.Log("Take No Damage!");
        }
        else
        {
            _hp -= damage * bonusDamage;
            PlayerHpBar.PlayerHpAction?.Invoke();
            _animator.SetTrigger(Define.TakeDamage);
            Debug.Log($"Current Player HP: {_hp}, Attacker name: {attacker.name}");
            if (_hp <= 0)
            {
                _animator.SetTrigger(Define.Die);
            }
        }
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
