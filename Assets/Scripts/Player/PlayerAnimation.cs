using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using PlayerLocomotionInput = Scripts.Player.Input.PlayerLocomotionInput;

namespace Scripts.Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float locomotionBlendSpeed = 0.02f;

        private PlayerLocomotionInput _playerLocomotionInput;
        private PlayerState _playerState;

        //Locomotion Hashes
        private static int inputXHash = Animator.StringToHash("inputX");
        private static int inputYHash = Animator.StringToHash("inputY");
        private static int inputMagnitudeHash = Animator.StringToHash("inputMagnitude");
        private static int isGroundedHash = Animator.StringToHash("isGrounded");
        private static int isJumpingHash = Animator.StringToHash("isJumping");
        private static int isFallingHash = Animator.StringToHash("isFalling");


        private Vector3 _currentBlendInput = Vector3.zero;

        private float _sprintMaxBlendTreeValue = 1.5f;
        private float _runMaxBlendTreeValue = 1.0f;
        private float _walkMaxBlendTreeValue = 0.5f;

        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotionInput>();
            _playerState = GetComponent<PlayerState>();
        }

        private void Update()
        {
            UpdateAnimationState();
        }

        private void UpdateAnimationState()
        {
            // set local values of animation state
            bool isRunning = _playerState.CurrentLocomotionState == PlayerLocomotionState.Running;
            bool isSprinting = _playerState.CurrentLocomotionState == PlayerLocomotionState.Sprinting;
            bool isJumping = _playerState.CurrentLocomotionState == PlayerLocomotionState.Jumping;
            bool isFalling = _playerState.CurrentLocomotionState == PlayerLocomotionState.Falling;
            bool isGrounded = _playerState.IsPlayerGrounded();

            //All states have the same input magnitude for corresponding blend trees
            bool isRunBendValue = isRunning || isFalling || isJumping;

            Vector2 inputTarget = isSprinting ? _playerLocomotionInput.MovementInput * _sprintMaxBlendTreeValue :
                                  isRunBendValue ? _playerLocomotionInput.MovementInput * _runMaxBlendTreeValue : _playerLocomotionInput.MovementInput * _walkMaxBlendTreeValue;

            _currentBlendInput = Vector3.Lerp(_currentBlendInput, inputTarget, locomotionBlendSpeed * Time.deltaTime);

            _animator.SetBool(isGroundedHash, isGrounded);
            _animator.SetBool(isFallingHash, isFalling);
            _animator.SetBool(isJumpingHash, isJumping);

            _animator.SetFloat(inputXHash, _currentBlendInput.x);
            _animator.SetFloat(inputYHash, _currentBlendInput.y);
            _animator.SetFloat(inputMagnitudeHash, _currentBlendInput.magnitude);


        }
    }
}

