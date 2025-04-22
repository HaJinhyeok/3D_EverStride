using UnityEngine;

public class CharacterInteractionEvents : MonoBehaviour
{
    Animator animator;
    Transform weaponPos;

    void Start()
    {
        animator = GetComponent<Animator>();
        weaponPos = GameObject.Find("jointItemR").transform;
    }

    public void OnInteractionHash()
    {
        animator.SetBool(Define.InteractionHash, true);
    }

    public void OffInteractionHash()
    {
        animator.SetBool(Define.InteractionHash, false);
    }

    public void OffIsAttacking()
    {
        animator.SetBool(Define.IsAttacking, false);
    }

    public void OnWeaponSound()
    {
        weaponPos.GetChild(0).GetComponent<AudioSource>().Play();
    }
}
