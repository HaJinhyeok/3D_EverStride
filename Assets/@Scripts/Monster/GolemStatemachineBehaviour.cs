using UnityEngine;

public class GolemStatemachineBehaviour : StateMachineBehaviour
{
    bool attackFlag = true;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        float currentTime = stateInfo.normalizedTime;

        if (currentTime >= 0.2 && currentTime <= 0.5)
        {
            if(attackFlag)
            {
                animator.SetBool(Define.InteractionHash, true);
                attackFlag = false;
            }            
        }
        else
        {
            animator.SetBool(Define.InteractionHash, false);
        }
        if (currentTime > 0.9)
        {
            animator.SetBool(Define.IsAttacking, false);
            attackFlag = true;
        }
    }
}
