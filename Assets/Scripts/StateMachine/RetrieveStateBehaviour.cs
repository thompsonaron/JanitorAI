using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetrieveStateBehaviour : StateMachineBehaviour
{
    private JanitorAI janitorAI;
    private Transform garbageCanTransform;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        janitorAI = animator.GetComponent<JanitorAI>();
        if (janitorAI.target.gameObject.tag.Equals("BlueTrash"))
        {
            garbageCanTransform = janitorAI.blueGarbageTransform;
            janitorAI.rb.velocity = new Vector2(-janitorAI.janitorVelocity, 0.0f);
        }
        else
        {
            garbageCanTransform = janitorAI.redGarbageTransform;
            janitorAI.rb.velocity = new Vector2(janitorAI.janitorVelocity, 0.0f);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(janitorAI.transform.position.x - garbageCanTransform.position.x) < janitorAI.janitorReach)
        {
            animator.SetBool("isWalkingToBlueTrash", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        janitorAI.DestroyCarryingTrash();
        janitorAI.rb.velocity = Vector2.zero;
    }
}