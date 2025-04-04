using UnityEngine;

public class PlayerStatemachineBehaviour : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;
        bool isNextCombo = animator.GetBool(Define.IsNextCombo);

        //if (currentTime >= 0.2 && currentTime <= 0.8 && isNextCombo)
        //{
        //    int atkComboCount = animator.GetInteger(Define.AttackComboCount);
        //    atkComboCount = atkComboCount < 2 ? ++atkComboCount : 0;
        //    animator.SetInteger(Define.AttackComboCount, atkComboCount);
        //}
        if (currentTime > 0.9 )
        {
            if(!isNextCombo)
            {
                animator.SetInteger(Define.AttackComboCount, 0);
                animator.SetBool(Define.IsNextCombo, false);
                animator.SetBool(Define.IsAttacking, false);
                //if (animator.GetCurrentAnimatorStateInfo(0).IsName("SwordCombo3"))
                //{
                //    Debug.Log("HelloUpdate on " + currentTime);
                //}
                GameManager.Instance.OnTrailActivate?.Invoke(animator.GetBool(Define.IsAttacking));
            }            
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsNextCombo, false);
        //animator.SetBool(Define.IsAttacking, false);
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("SwordCombo3"))
        //{
        //    Debug.Log($"Exit of {animator.GetCurrentAnimatorStateInfo(0).IsName("SwordCombo3")} at {stateInfo.normalizedTime}");
        //}
        GameManager.Instance.OnTrailActivate?.Invoke(animator.GetBool(Define.IsAttacking));
    }
}
