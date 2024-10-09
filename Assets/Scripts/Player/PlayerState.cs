using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerState : MonoBehaviour
    {
        [field: SerializeField] public PlayerLocomotionState CurrentLocomotionState { get; set; } = PlayerLocomotionState.Idling;

        public bool IsPlayerGrounded()
        {
            return IsStateGroundedState(CurrentLocomotionState);
        }

        public bool IsStateGroundedState(PlayerLocomotionState state)
        {
            return state != PlayerLocomotionState.Jumping && state != PlayerLocomotionState.Falling;
        }
    }


    public enum PlayerLocomotionState
    {
        Idling = 1,
        Walking = 2,
        Running = 3,
        Sprinting = 4,
        Jumping = 5,
        Falling = 6
    }
}
