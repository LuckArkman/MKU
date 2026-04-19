using System;
using Scripts.FisicsSystem;
using UnityEngine;

namespace MKU.Scripts.FisicsSystem
{
    [Serializable]
    public class Gravity : GravityDAO
    {
        /// <summary>
        /// Applies gravity to the provided CharacterController.
        /// </summary>
        /// <param name="charController">The CharacterController to which gravity will be applied.</param>
        public override void IsGravity(CharacterController charController)
        {
            charController.Move( gravityDirection * accel * Time.deltaTime);            
        }

        /// <summary>
        /// Determines whether a character is currently grounded using the CharacterController's state.
        /// </summary>
        /// <param name="charController">The CharacterController component associated with the character.</param>
        /// <returns>True if the character is grounded; otherwise, false.</returns>
        public override bool IsGrounded(CharacterController charController)
        {
            return charController.isGrounded;
        }
    }
}