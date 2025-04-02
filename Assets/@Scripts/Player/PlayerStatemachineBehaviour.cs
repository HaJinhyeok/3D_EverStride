using UnityEngine;

public class PlayerStatemachineBehaviour : StateMachineBehaviour
{
    bool attackFlag = true;

    // attack1
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;
        bool isNextCombo = animator.GetBool(Define.IsNextCombo);

        if (currentTime >= 0.2 && currentTime <= 0.8)
        {
            // �ִϸ��̼��� ��� ���� ����� �Ŀ� ������Ʈ�� ��ƾ� attackȿ�� �ߵ�
            if (attackFlag)
            {
                animator.SetBool(Define.InteractionHash, true);
                attackFlag = false;
            }
        }
        else
        {
            animator.SetBool(Define.InteractionHash, false);
        }
        if (currentTime >= 0.2 && currentTime <= 0.9 && isNextCombo)
        {
            int atkComboCount = animator.GetInteger(Define.AttackComboCount);
            atkComboCount = atkComboCount < 2 ? ++atkComboCount : 0;
            animator.SetInteger(Define.AttackComboCount, atkComboCount);
        }
        if (currentTime > 0.9)
        {
            animator.SetInteger(Define.AttackComboCount, 0);
            animator.SetBool(Define.IsNextCombo, false);
            animator.SetBool(Define.IsAttacking, false);
            GameManager.Instance.OnTrailActivate?.Invoke(animator.GetBool(Define.IsAttacking));
            attackFlag = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(Define.IsNextCombo, false);
        animator.SetBool(Define.IsAttacking, false);
        GameManager.Instance.OnTrailActivate?.Invoke(animator.GetBool(Define.IsAttacking));
    }
}
