using MKU.Scripts.Enums;
using MKU.Scripts.Singletons;
using UnityEngine;

namespace MKU.Scripts.Abillity
{
    [CreateAssetMenu(fileName = "AbillityController", menuName = "AbillityController/new Controller", order = 0)]
    public class AbillityController : ScriptableObject
    {
        public states state = states.None;
        
        public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            
        }

        public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!Singleton.Instance._charController.isMoving &&
                !Singleton.Instance._charController.isRunning)
            {
                animator.SetBool(states.Walk.ToString(), Singleton.Instance._charController.isMoving);
                animator.SetBool(states.Run.ToString(), Singleton.Instance._charController.isRunning);
            }
        }

        public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }

        public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}