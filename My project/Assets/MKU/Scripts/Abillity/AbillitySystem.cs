using MKU.Scripts.Enums;
using MKU.Scripts.Singletons;
using UnityEngine;
using CharacterController = MKU.Scripts.Character.CharacterController;

namespace MKU.Scripts.Abillity
{
    public class AbillitySystem : StateMachineBehaviour
    {
        public states state = states.None;
        public AbillityController abillity = null;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            abillity.OnStateEnter(animator, stateInfo, layerIndex);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            abillity.OnStateExit(animator, stateInfo, layerIndex);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Singleton.Instance.controller.isMoving)animator.SetBool(states.Walk.ToString(), Singleton.Instance.controller.isMoving);
            if (Singleton.Instance.controller.isRunning)animator.SetBool(states.Run.ToString(), Singleton.Instance.controller.isRunning);
            abillity.OnStateUpdate(animator, stateInfo, layerIndex);
        }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            abillity.OnStateMove(animator, stateInfo, layerIndex);
        }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            abillity.OnStateIK(animator, stateInfo, layerIndex);
        }
    }
}