using Scripts.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerManager = Scripts.Player.Player;
using Scripts.Player.Input;
using Scripts.Player;
using System;

namespace Scripts.Item
{
    public class PipeEditor : MonoBehaviour
    {
        [SerializeField] private GameObject _parentPipe;

        private PlayerInput _playerInput;
        private PlayerUIInput _playerUIInput;
        private PlayerController _playerController;
        private BoxCollider _boxCollider;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameScreen.Instance.ShowKeyPrompt("Edit Pipe");
                _boxCollider = GetComponent<BoxCollider>();
                _playerUIInput = PlayerManager.Instance.GetComponent<PlayerUIInput>();

                _playerUIInput.SetCurrentPipe(_parentPipe);
            }
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameScreen.Instance.HideKeyPrompt();

            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _playerInput = PlayerManager.Instance.GetComponent<PlayerInput>();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.Instance.getTaskAccepted())
            {
                GameScreen.Instance.HideKeyPrompt();
                EditPipe();
            }

        }

        private void EditPipe()
        {

            Debug.Log("edit pipe function");
            _playerInput.SwitchCurrentActionMap("UI");
            _boxCollider.isTrigger = false;
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Enable physics
                rb.constraints = RigidbodyConstraints.FreezePosition |
                                 RigidbodyConstraints.FreezeRotationX |
                                 RigidbodyConstraints.FreezeRotationZ;
            }
        }

        public void MovePipe(Vector3 movement)
        {
            // Move the pipe object by the given vector
            transform.position += movement;

            Debug.Log($"Pipe moved by {movement}");
            _boxCollider.isTrigger = true;
            _playerInput.SwitchCurrentActionMap("Player");
        }


        //    if (context.performed)
        //    {
        //        Debug.Log($"Rotating Pipe: {name}");
        //        Debug.Log($"Before Rotation: {transform.eulerAngles}");
        //        transform.Rotate(0, 90f, 0, Space.Self);
        //        Debug.Log($"After Rotation: {transform.eulerAngles}");
        //        Debug.Log("Pipe rotated 90 degrees to the left.");
        //    }
        //}

    }
}
