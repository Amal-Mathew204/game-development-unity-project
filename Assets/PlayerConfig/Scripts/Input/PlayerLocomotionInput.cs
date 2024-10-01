using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerConfig
{
    public class PlayerLocomotionInput : MonoBehaviour
    {
        public Vector2 MovementInput { get; private set; }

        public void OnMove(InputValue value)
        {
            MovementInput = value.Get<Vector2>();
        }

    }
}
