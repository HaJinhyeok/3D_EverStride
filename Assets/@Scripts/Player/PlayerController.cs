using NUnit.Framework.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    Client Client;
    public Camera ConversationCamera;

    public GameObject ConversationPanelObejct;
    public GameObject PausePanel;
    public GameObject GameUI;
    public Inventory Inventory;
    public ShortcutInventory Shortcut;
    public Image Stamina;
    public GameObject WeaponPos;
    public GameObject ItemPos;
    public GameObject BaseClothes;
    public GameObject PlateClothes;

    public List<ItemData> IngredientData = new List<ItemData>();

    public GameObject stepRayUpper;
    public GameObject stepRayLower;
    float stepSmooth = 0.1f;

    Transform _character;
    Animator _animator;
    Rigidbody _rigidbody;
    Camera _camera;
    Transform _camAxis;
    AudioSource _audioSource;
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

    Vector3 _camOffset = new Vector3(1f, 0.5f, -3f);
    Vector3 _conversationOffSet = new Vector3(0, 0, 3f);

    public float PlayerHp
    {
        get { return _hp; }
        set
        {
            _hp = value;
            PlayerHpBar.PlayerHpAction?.Invoke();
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
    #endregion

    // Player Setting On Awake
    void Awake()
    {
        // * 컴포넌트
        _character = transform.GetChild(0);
        _animator = _character.GetComponent<Animator>();
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;
        _audioSource = GetComponent<AudioSource>();
        Client = GameObject.Find("TestClient").GetComponentInChildren<Client>();

        if (SceneManager.GetActiveScene().name == Define.GameScene)
        {
            _isBossRaid = false;
        }
        else
        {
            _isBossRaid = true;
            ConversationCamera = null;
            _animator.runtimeAnimatorController = Resources.Load(Define.BossRaidAnimatorPath) as RuntimeAnimatorController;
        }

        // * 콜라이더
        CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
        capsule.center = new Vector3(0, 1f, 0);
        capsule.radius = 0.5f;
        capsule.height = 2f;

        // * 카메라
        _camera = Camera.main;
        _camAxis = new GameObject(Define.CamAxis).transform;
        _camera.transform.parent = _camAxis;
        _camera.transform.localPosition = _camOffset;
        ConversationCamera?.gameObject.SetActive(false);

        // * 그 외
        _npcMask = LayerMask.GetMask(Define.NPCMask);
        for (int i = 0; i < IngredientData.Count; i++)
        {
            if (!Define.IngredientData.ContainsKey(IngredientData[i].IngredientType))
                Define.IngredientData.Add(IngredientData[i].IngredientType, IngredientData[i]);
        }
        GameManager.Instance.LoadResources();

        InventoryOn(false);

        // 보스전에서 칼로 놀다가 마을 복귀 후 주먹질 할때 에러 막기용
        GameManager.Instance.OnWeaponChanged.Invoke();

        _hp = Define.PlayerMaxHp;
        WeaponTypeHash = (int)GetCurrentWeaponType();
    }

    private void Start()
    {
        Inventory.UpdateInventory(GameManager.Instance.InventorySlots, GameManager.Instance.ShortcutInventorySlots);
        Shortcut.UpdateShortcutInventory(GameManager.Instance.ShortcutInventorySlots);
        Inventory.UpdateTestWeapons();
        Inventory.UpdateTestIngredients();
        PlayerClothesSetting();
    }

    private void Update()
    {
        GamePause();

        // 대화 중엔 움직임 불가능
        if (!GameManager.Instance.IsConversating && !GameManager.Instance.IsPaused && !_animator.GetBool(Define.IsCrafting))
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
            if (!GameManager.Instance.IsInventoryOn && !GameManager.Instance.IsUIOn)
            {
                CameraMove();
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
        if ((Input.GetButton(Define.Horizontal) || Input.GetButton(Define.Vertical)))
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
            if (!IsAttacking)
            {
                float h = Input.GetAxis(Define.Horizontal);
                float v = Input.GetAxis(Define.Vertical);

                Vector3 movement = new Vector3(h, 0, v);
                float velY = _rigidbody.linearVelocity.y;
                transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);

                // local 방향에서 world 방향으로 바꿔주는 역할???
                Vector3 localMovement = transform.TransformDirection(movement);

                _rigidbody.linearVelocity = localMovement.normalized * _speed + new Vector3(0, velY, 0);
                _character.transform.localRotation = Quaternion.Slerp(_character.transform.localRotation, Quaternion.LookRotation(movement), _rotationSpeed * Time.deltaTime);

                // player 위치 좌표 IOCP 서버로 전송
                Client.SendMessageToServer($"{transform.position.x} {transform.position.y} {transform.position.z}");
            }
            else
            {
                float h = Input.GetAxis(Define.Horizontal);
                float v = Input.GetAxis(Define.Vertical);

                transform.rotation = Quaternion.Euler(new Vector3(0, _camAxis.rotation.y + _mouseX, 0) * _camSpeed);
            }
        }
        else
        {
            // 속도 0으로 만들어주기
            _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
            _audioSource.Stop();
        }

        Speed = _rigidbody.linearVelocity.sqrMagnitude;
        _camAxis.position = transform.position + Vector3.up;
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
            EquipItem(Inventory.UseShortcutItem(0));
        }
        if (Input.GetKeyDown("2"))
        {
            EquipItem(Inventory.UseShortcutItem(1));
        }
        if (Input.GetKeyDown("3"))
        {
            EquipItem(Inventory.UseShortcutItem(2));
        }
        if (Input.GetKeyDown("4"))
        {
            EquipItem(Inventory.UseShortcutItem(3));
        }
        if (Input.GetKeyDown("5"))
        {
            EquipItem(Inventory.UseShortcutItem(4));
        }
    }

    void InteractNPC()
    {
        if (Input.GetKeyDown(KeyCode.G) && GameManager.Instance.IsNPCInteracive)
        {
            Collider[] NPC = Physics.OverlapSphere(transform.position, 2f, _npcMask);
            NPC[0].transform.rotation = Quaternion.LookRotation(transform.position - NPC[0].transform.position);
            LookNPC(NPC[0].transform.position, _conversationOffSet);
            // 대화용 카메라 ON
            ConversationCamera.gameObject.SetActive(true);
            ConversationPanelObejct.gameObject.SetActive(true);
            //GameObject.Find(Define.GameUI).SetActive(false);
            GameUI.SetActive(false);
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

    public void LookNPC(Vector3 lookPos, Vector3 offset)
    {
        transform.position = lookPos + offset;
        _character.rotation = Quaternion.LookRotation(lookPos - transform.position);
    }
    #endregion

    #region Weapon
    public bool EquipItem(Slot currentSlot)
    {
        // 아이템 사용해서 개수 줄어들면 true, 아니면 false
        if (currentSlot.ItemData == null)
            return false;
        // 무기 아이템일 경우
        if (currentSlot.ItemData.ItemType == Define.ItemType.Weapon)
        {
            if (WeaponPos.transform.childCount > 0)
            {
                // 장착해제
                UnequipWeapon();
            }
            GameObject newWeapon = Instantiate(GameManager.Instance.Weapons[(int)currentSlot.ItemData.WeaponType], WeaponPos.transform);
            WeaponTypeHash = (int)currentSlot.ItemData.WeaponType;
            GameManager.Instance.OnWeaponChanged?.Invoke();
            return false;
        }
        // 소비 및 재료 아이템일 경우
        else if (currentSlot.ItemData.ItemType == Define.ItemType.Countable)
        {
            if (currentSlot.ItemData.ItemName == "포션")
            {
                // 포션 사용 및 포션 먹는 애니메이션
                if (_hp >= Define.PlayerMaxHp)
                {
                    UI_Warning.Instance.WarningEffect(Define.AlreadyFullHP);
                    return false;
                }
                else
                {
                    _animator.SetTrigger(Define.Drink);
                    GameObject gameObject = Instantiate(GameManager.Instance.ConsumptionItems[0], ItemPos.transform);
                    gameObject.GetComponent<Collider>().isTrigger = true;
                    //Debug.Log($"Drink {gameObject.name}!");
                    return true;
                }
            }
            else
            {
                Debug.Log("할 동작이 없습니다");
                return false;
            }
        }
        // 장비 아이템일 경우
        else if (currentSlot.ItemData.ItemType == Define.ItemType.Equipment)
        {
            // GameManager에서 현재 플레이어의 장비 장착 상태 확인 후 업그레이드 가능하면 장착시켜줌
            switch (currentSlot.ItemData.Prefab.layer)
            {
                case (int)Define.EquipmentType.Helmet:
                    // 기존 부위의 장비보다 높은 등급이면 장착
                    if (GameManager.Instance.PlayerEquipment.helmet != Define.EquipmentStatus.Iron)
                    {
                        if (GameManager.Instance.PlayerEquipment.helmet == Define.EquipmentStatus.Base)
                        {
                            BaseClothes.transform.Find("Starter_Helmet").gameObject.SetActive(false);
                        }
                        PlateClothes.transform.Find(Define.PlateSetPrefix + "Helmet").gameObject.SetActive(true);
                        GameManager.Instance.PlayerEquipment.helmet = Define.EquipmentStatus.Iron;
                    }
                    // 이미 같은 등급 장착 중이면 이미 장착 중이라고 표시
                    else
                    {
                        UI_Warning.Instance.WarningEffect(Define.AlreadyEquipSameGrade);
                        return false;
                    }
                    break;
                case (int)Define.EquipmentType.Chest:
                    if (GameManager.Instance.PlayerEquipment.chest != Define.EquipmentStatus.Iron)
                    {
                        if (GameManager.Instance.PlayerEquipment.chest == Define.EquipmentStatus.Base)
                        {
                            BaseClothes.transform.Find("Starter_Chest").gameObject.SetActive(false);
                        }
                        PlateClothes.transform.Find(Define.PlateSetPrefix + "Chest").gameObject.SetActive(true);
                        GameManager.Instance.PlayerEquipment.chest = Define.EquipmentStatus.Iron;
                    }
                    else
                    {
                        UI_Warning.Instance.WarningEffect(Define.AlreadyEquipSameGrade);
                        return false;
                    }
                    break;
                case (int)Define.EquipmentType.Shoulders:
                    if (GameManager.Instance.PlayerEquipment.shoulders != Define.EquipmentStatus.Iron)
                    {
                        if (GameManager.Instance.PlayerEquipment.shoulders == Define.EquipmentStatus.Base)
                        {
                            BaseClothes.transform.Find("Starter_Shoulders").gameObject.SetActive(false);
                        }
                        PlateClothes.transform.Find(Define.PlateSetPrefix + "Shoulders").gameObject.SetActive(true);
                        GameManager.Instance.PlayerEquipment.shoulders = Define.EquipmentStatus.Iron;
                    }
                    else
                    {
                        UI_Warning.Instance.WarningEffect(Define.AlreadyEquipSameGrade);
                        return false;
                    }
                    break;
                case (int)Define.EquipmentType.Gloves:
                    if (GameManager.Instance.PlayerEquipment.gloves != Define.EquipmentStatus.Iron)
                    {
                        if (GameManager.Instance.PlayerEquipment.gloves == Define.EquipmentStatus.Base)
                        {
                            BaseClothes.transform.Find("Starter_Gloves").gameObject.SetActive(false);
                        }
                        PlateClothes.transform.Find(Define.PlateSetPrefix + "Gloves").gameObject.SetActive(true);
                        GameManager.Instance.PlayerEquipment.gloves = Define.EquipmentStatus.Iron;
                    }
                    else
                    {
                        UI_Warning.Instance.WarningEffect(Define.AlreadyEquipSameGrade);
                        return false;
                    }
                    break;
                case (int)Define.EquipmentType.Pants:
                    if (GameManager.Instance.PlayerEquipment.pants != Define.EquipmentStatus.Iron)
                    {
                        if (GameManager.Instance.PlayerEquipment.pants == Define.EquipmentStatus.Base)
                        {
                            BaseClothes.transform.Find("Starter_Pants").gameObject.SetActive(false);
                        }
                        PlateClothes.transform.Find(Define.PlateSetPrefix + "Pants").gameObject.SetActive(true);
                        GameManager.Instance.PlayerEquipment.pants = Define.EquipmentStatus.Iron;
                    }
                    else
                    {
                        UI_Warning.Instance.WarningEffect(Define.AlreadyEquipSameGrade);
                        return false;
                    }
                    break;
                case (int)Define.EquipmentType.Boots:
                    if (GameManager.Instance.PlayerEquipment.boots != Define.EquipmentStatus.Iron)
                    {
                        if (GameManager.Instance.PlayerEquipment.boots == Define.EquipmentStatus.Base)
                        {
                            BaseClothes.transform.Find("Starter_Boots").gameObject.SetActive(false);
                        }
                        PlateClothes.transform.Find(Define.PlateSetPrefix + "Boots").gameObject.SetActive(true);
                        GameManager.Instance.PlayerEquipment.boots = Define.EquipmentStatus.Iron;
                    }
                    else
                    {
                        UI_Warning.Instance.WarningEffect(Define.AlreadyEquipSameGrade);
                        return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        return false;
    }

    public Define.WeaponType UnequipWeapon()
    {
        if (WeaponPos.transform.childCount == 0)
        {
            Debug.Log("무기를 장착하고 있지 않습니다");
            UI_Warning.Instance.WarningEffect(Define.NoWeapon);
            return Define.WeaponType.None;
        }
        GameObject weapon = WeaponPos.transform.GetChild(0).gameObject;
        if (weapon == null)
            return Define.WeaponType.None;
        Define.WeaponType weaponType = weapon.GetComponent<WeaponItem>().GetWeaponType();
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
    void InventoryOn(bool state)
    {
        GameManager.Instance.IsInventoryOn = state;
        GameManager.Instance.IsUIOn = state;
        Inventory.gameObject.SetActive(state);
    }
    #endregion

    void PlayerClothesSetting()
    {
        Transform[] baseClothes = BaseClothes.GetComponentsInChildren<Transform>();
        Transform[] plateClothes = PlateClothes.GetComponentsInChildren<Transform>();
        switch (GameManager.Instance.PlayerEquipment.helmet)
        {
            case Define.EquipmentStatus.None:
                plateClothes[4].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Base:
                break;
            case Define.EquipmentStatus.Iron:
                plateClothes[4].gameObject.SetActive(true);
                break;
            default:
                break;
        }
        switch (GameManager.Instance.PlayerEquipment.chest)
        {
            case Define.EquipmentStatus.None:
                baseClothes[2].gameObject.SetActive(false);
                plateClothes[2].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Base:
                baseClothes[2].gameObject.SetActive(true);
                plateClothes[2].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Iron:
                baseClothes[2].gameObject.SetActive(false);
                plateClothes[2].gameObject.SetActive(true);
                break;
            default:
                break;
        }
        switch (GameManager.Instance.PlayerEquipment.shoulders)
        {
            case Define.EquipmentStatus.None:
                plateClothes[6].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Base:
                break;
            case Define.EquipmentStatus.Iron:
                plateClothes[6].gameObject.SetActive(true);
                break;
            default:
                break;
        }
        switch (GameManager.Instance.PlayerEquipment.gloves)
        {
            case Define.EquipmentStatus.None:
                plateClothes[3].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Base:
                break;
            case Define.EquipmentStatus.Iron:
                plateClothes[3].gameObject.SetActive(true);
                break;
            default:
                break;
        }
        switch (GameManager.Instance.PlayerEquipment.pants)
        {
            case Define.EquipmentStatus.None:
                baseClothes[3].gameObject.SetActive(true);
                plateClothes[5].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Base:
                baseClothes[3].gameObject.SetActive(true);
                plateClothes[5].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Iron:
                baseClothes[3].gameObject.SetActive(false);
                plateClothes[5].gameObject.SetActive(true);
                break;
            default:
                break;
        }
        switch (GameManager.Instance.PlayerEquipment.boots)
        {
            case Define.EquipmentStatus.None:
                baseClothes[1].gameObject.SetActive(false);
                plateClothes[1].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Base:
                baseClothes[1].gameObject.SetActive(true);
                plateClothes[1].gameObject.SetActive(false);
                break;
            case Define.EquipmentStatus.Iron:
                baseClothes[1].gameObject.SetActive(false);
                plateClothes[1].gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Define.GroundTag))
        {
            IsGround = true;
        }
    }

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

    public void GamePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isPause = GameManager.Instance.IsPaused;
            GameManager.Instance.IsPaused = !isPause;
            PausePanel.SetActive(!isPause);
            Time.timeScale = 1f - Time.timeScale;
        }
    }
}
