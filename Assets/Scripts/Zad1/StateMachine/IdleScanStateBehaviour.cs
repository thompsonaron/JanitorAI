using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleScanStateBehaviour : StateMachineBehaviour
{
    private JanitorAI janitorAI;
    private Vector2 currentPos;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        janitorAI = animator.GetComponent<JanitorAI>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (janitorAI.trashTransforms.Count < 1) return;
        janitorAI.target = janitorAI.trashTransforms[0];

        currentPos = animator.transform.position;

        float initialDIstance;
        initialDIstance = GetShortestPath(janitorAI.target.position.x, janitorAI.target.tag);
        int removeIndex = 0;

        for (int i = 0; i < janitorAI.trashTransforms.Count; i++)
        {

            // shortest total path (to the trash and the trash can combined)
            var distance = GetShortestPath(janitorAI.trashTransforms[i].position.x, janitorAI.trashTransforms[i].tag);

            if (distance <= initialDIstance && janitorAI.trashTransforms[i].position.y < 0)
            {
                initialDIstance = distance;
                janitorAI.target = janitorAI.trashTransforms[i];
                removeIndex = i;
            }
        }

        if (janitorAI.target.transform.position.y < 0) 
        {
            janitorAI.trashTransforms.RemoveAt(removeIndex);
            animator.SetBool("foundTarget", true);
        }

        float GetShortestPath(float trashPosX, string tag)
        {
            float distance;
            // shortest path
            if (tag.Equals("BlueTrash"))
            {
                distance = HelperFunctions.CalculateShortestPickupDistance(currentPos.x, trashPosX, janitorAI.blueGarbageTransform.position.x);
            }
            else
            {
                distance = HelperFunctions.CalculateShortestPickupDistance(currentPos.x, trashPosX, janitorAI.redGarbageTransform.position.x);
            }

            return distance;
        }
    }
}
