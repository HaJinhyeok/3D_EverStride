using UnityEngine;

public class PlayerInteractionEvents : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        //animator = GameManager.Instance.Player.GetComponentInChildren<Animator>();
        animator = GetComponent<Animator>();
    }

    public void OnInteractionHash()
    {
        animator.SetBool(Define.InteractionHash, true);
    }

    public void OffInteractionHash()
    {
        animator.SetBool(Define.InteractionHash, false);
    }
}
