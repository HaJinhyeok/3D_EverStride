using UnityEngine;

public class PlayerStatemachineBehaviour : StateMachineBehaviour
{
    // attack1
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;
        bool isNextCombo = animator.GetBool(Define.IsNextCombo);

        if (currentTime >= 0.7 && currentTime <= 0.9 && isNextCombo)
        {
            int atkComboCount = animator.GetInteger(Define.AttackComboCount);
            atkComboCount = atkComboCount < 1 ? ++atkComboCount : 0;
            animator.SetInteger(Define.AttackComboCount, atkComboCount);
        }
        if (currentTime > 0.9)
        {
            animator.SetInteger(Define.AttackComboCount, 0);
            animator.SetBool(Define.IsAttacking, false);
            animator.SetBool(Define.IsNextCombo, false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsNextCombo, false);
    }
}
