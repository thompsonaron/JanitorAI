using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekStateBehaviour : StateMachineBehaviour
{
    private JanitorAI janitorAI;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        janitorAI = animator.GetComponent<JanitorAI>();

        if (janitorAI.transform.position.x < janitorAI.target.position.x)
        {
            janitorAI.rb.velocity = new Vector2 (janitorAI.janitorVelocity, 0.0f);
        }
        else
        {
            janitorAI.rb.velocity = new Vector2(-janitorAI.janitorVelocity, 0.0f);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(janitorAI.transform.position.x - janitorAI.target.position.x) < janitorAI.janitorReach)
        {
            janitorAI.rb.velocity = Vector2.zero;
            CarryTrash();
            animator.SetBool("isWalkingToBlueTrash", true);
            animator.SetBool("foundTarget", false);
        }
    }

    private void CarryTrash()
    {
        janitorAI.target.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        janitorAI.target.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        janitorAI.target.gameObject.transform.position = janitorAI.carryPosition.position;
        janitorAI.target.gameObject.transform.SetParent(janitorAI.carryPosition);
    }
}