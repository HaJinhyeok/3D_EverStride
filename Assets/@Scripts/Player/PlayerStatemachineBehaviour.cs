using UnityEngine;

public class PlayerStatemachineBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;
        bool isNextCombo = animator.GetBool(Define.IsNextCombo);
        if (currentTime > 0.9 )
        {
            if(!isNextCombo)
            {
                animator.SetInteger(Define.AttackComboCount, 0);
                animator.SetBool(Define.IsNextCombo, false);
                animator.SetBool(Define.IsAttacking, false);
                GameManager.Instance.OnTrailActivate?.Invoke(false);
            }            
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsNextCombo, false);
        GameManager.Instance.OnTrailActivate?.Invoke(false);
    }
}
