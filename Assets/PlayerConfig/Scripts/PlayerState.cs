using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerConfig
{
    public class PlayerState : MonoBehaviour
    {
        [field: SerializeField] public PlayerLocomotionState CurrentLocomotionState { get; set; } = PlayerLocomotionState.Idling;
    }


    public enum PlayerLocomotionState
    {
        Idling = 1,
        Walking = 2,
        Running = 3,
        Sprinting = 3,
        Jumping = 4,
        Falling = 5
    }
}