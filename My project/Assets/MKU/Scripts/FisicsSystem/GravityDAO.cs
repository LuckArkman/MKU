using UnityEngine;

namespace Scripts.FisicsSystem
{
    public abstract class GravityDAO
    {
        /// <summary>
        /// Represents the direction in which gravity is applied.
        /// </summary>
        /// <remarks>
        /// This variable defines the vector direction for gravitational force.
        /// By default, it is commonly set to point downward in Unity's coordinate system.
        /// The value may be adjusted depending on specific gravity mechanics or use cases.
        /// </remarks>
        public Vector3 gravityDirection;

        /// <summary>
        /// Represents the acceleration value used to apply gravity to objects.
        /// </summary>
        /// <remarks>
        /// This variable determines the strength of the gravity effect applied to a
        /// CharacterController when the gravity logic is executed. The value is multiplied
        /// with the gravity direction and the frame's delta time to calculate the movement caused by gravity.
        /// </remarks>
        public float accel = 50.0f;

        /// <summary>
        /// Applies gravity to the provided CharacterController.
        /// </summary>
        /// <param name="charController">The CharacterController to which gravity will be applied.</param>
        public abstract void IsGravity(CharacterController charController);

        /// <summary>
        /// Determines whether a character is currently grounded based on the given CharacterController.
        /// </summary>
        /// <param name="charController">The CharacterController component of the character being checked.</param>
        /// <returns>Returns true if the character is grounded; otherwise, returns false.</returns>
        public abstract bool IsGrounded(CharacterController charController);
    }
}