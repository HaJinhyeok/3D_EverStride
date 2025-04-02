using UnityEngine;

public class Trail : MonoBehaviour
{
    bool _isActive = false;

    void Start()
    {
        _isActive = GameManager.Instance.Player.transform.GetChild(0).GetComponent<Animator>().GetBool(Define.IsAttacking);
    }

    void Update()
    {
        gameObject.SetActive(_isActive);
    }
}
