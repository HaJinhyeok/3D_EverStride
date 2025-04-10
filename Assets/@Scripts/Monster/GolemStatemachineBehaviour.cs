using UnityEngine;

public class GolemStatemachineBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsCombo, false);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;
        int comboCount = animator.GetInteger(Define.ComboCount);

        if (currentTime > 0.9&&animator.GetBool(Define.IsAttacking))
        {
            animator.SetBool(Define.IsCombo, true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(Define.InteractionHash, false);
        animator.SetBool(Define.IsAttacking, false);
        animator.SetBool(Define.IsCombo, false);
    }
}
