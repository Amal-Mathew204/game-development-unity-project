using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    /// <summary>
    /// Class for Storing and Changing the various States of the Player
    /// </summary>
    public class PlayerState : MonoBehaviour
    {
        [field: SerializeField] public PlayerLocomotionState CurrentLocomotionState { get; set; } = PlayerLocomotionState.Idling;
        [field: SerializeField] public PlayerActionState CurrentActionState { get; set; } = PlayerActionState.Idling;

        /// <summary>
        /// Checks if the Player is not airborne
        /// </summary>
        public bool IsPlayerGrounded()
        {
            return IsStateGroundedState(CurrentLocomotionState);
        }

        /// <summary>
        /// Checks if a state is not the Jumping or Falling State
        /// </summary>
        public bool IsStateGroundedState(PlayerLocomotionState state)
        {
            return state != PlayerLocomotionState.Jumping && state != PlayerLocomotionState.Falling;
        }
    }


    /// <summary>
    /// Enum for the Locomotion States of the Player
    /// </summary>
    public enum PlayerLocomotionState
    {
        Idling = 1,
        Walking = 2,
        Running = 3,
        Sprinting = 4,
        Jumping = 5,
        Falling = 6
    }

    /// <summary>
    /// Enum for the Action States of the Player
    /// </summary>
    public enum PlayerActionState
    {
        Idling = 0,
        Gathering = 1
    }
}
