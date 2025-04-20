using UnityEngine;

public class PlayerRunBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameManager.Instance.Player.GetComponent<AudioSource>().Play();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        GameManager.Instance.Player.GetComponent<AudioSource>().Stop();
    }
}
