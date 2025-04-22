using System;
using System.Collections;
using UnityEngine;

public class CraftTable : MonoBehaviour
{
    public Camera PreviewCamera;
    public Camera CraftCamera;
    public GameObject CraftPanel;
    public GameObject InteractionPanel;
    public GameObject GameUI;
    public GameObject Anvil;
    public static Action<ItemData, float> OnCraftAction;

    Transform _player;
    Transform _camAxis;
    Animator _playerAnimator;
    Vector3 _craftOffset = new Vector3(-1.5f, 0, 0);
    float _interactionDistance = 3f;

    void Start()
    {
        _player = GameManager.Instance.Player.transform;
        _camAxis = GameObject.Find(Define.CamAxis).transform;
        _playerAnimator = _player.GetChild(0).GetComponent<Animator>();
        OnCraftAction += StartCrafting;
        CraftUIOn(false);
    }

    void Update()
    {
        if (Vector3.Distance(_player.position, transform.position) <= _interactionDistance && !_player.GetChild(0).GetComponent<Animator>().GetBool(Define.IsCrafting))
        {
            // 상호작용 활성화
            InteractionPanel.SetActive(true);
            GameManager.Instance.IsPossibleCraft = true;
        }
        else
        {
            InteractionPanel.SetActive(false);
            GameManager.Instance.IsPossibleCraft = false;
            if (GameManager.Instance.IsCraftPanelOn)
                CraftUIOn(false);
        }

        if (Input.GetKeyDown(KeyCode.G) && GameManager.Instance.IsPossibleCraft && !GameManager.Instance.Player.transform.GetChild(0).GetComponent<Animator>().GetBool(Define.IsCrafting))
        {
            if (!GameManager.Instance.IsCraftPanelOn)
            {
                _player.gameObject.GetComponent<PlayerController>().LookNPC(transform.position, _craftOffset);
            }
            CraftUIOn(!GameManager.Instance.IsCraftPanelOn);
        }
    }
    void CraftUIOn(bool state)
    {
        GameManager.Instance.IsCraftPanelOn = state;
        GameManager.Instance.IsUIOn = state;
        CraftCamera?.gameObject.SetActive(state);
        CraftPanel?.SetActive(state);
        PreviewCamera?.gameObject.SetActive(state);
        GameUI.SetActive(!state);
    }

    void StartCrafting(ItemData data, float craftTime)
    {
        StartCoroutine(CoCrafting(data, craftTime));
    }

    IEnumerator CoCrafting(ItemData data, float craftTime)
    {
        GameObject currentWeapon = null;
        Transform position = GameManager.Instance.Player.WeaponPos.transform;
        _playerAnimator.SetBool(Define.IsCrafting, true);
        _camAxis.rotation = Quaternion.Euler(15f, -10f, 0f);
        if (position.childCount > 0)
        {
            currentWeapon = position.GetChild(0).gameObject;
            currentWeapon.SetActive(false);
        }
        GameObject hammer = Instantiate(GameManager.Instance.Weapons[4], position);
        CraftUIOn(false);
        CraftingNoticePanel.OnCraftingNoticeAction?.Invoke(data, craftTime);
        yield return new WaitForSeconds(craftTime);
        _playerAnimator.SetBool(Define.IsCrafting, false);
        Destroy(hammer);
        currentWeapon?.SetActive(true);
    }

    public void PreviewCameraSetting(int layer)
    {
        PreviewCamera.cullingMask = layer;
    }

    private void OnDestroy()
    {
        OnCraftAction -= StartCrafting;
    }
}
