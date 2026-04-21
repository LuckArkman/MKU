using MKU.Scripts.AISystem;
using UnityEngine;

namespace Unity.FantasyKingdom
{
    public class AttackController : MonoBehaviour
    {
        public AIController aiController;

        public void Attack()
        {
            aiController.Attack();
        }
    }
}
