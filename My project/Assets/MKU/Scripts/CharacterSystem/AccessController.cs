using System;
using UnityEngine;

namespace MKU.Scripts.CharacterSystem
{
    public class AccessController : MonoBehaviour
    {
        public CharController charController;

        private void Start()
        {
            
        }

        public void OnCombo()
        {
            CharSettings._Instance.hit++;
            charController.combo = true;
        }

        public void HitAttack()
        {
            charController.combo = false;
            charController._base.Status.PhysicalAttack *= 2;
            if(charController.GetTarget())charController.OnAttackHit();
        }

        public void OffCombo() => charController.combo = false;
        
    }
}