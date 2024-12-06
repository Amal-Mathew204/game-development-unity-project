using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using UnityEngine.Rendering;
using PlayerManager = Scripts.Player.Player;
using Unity.VisualScripting;
using System.Threading;
using TMPro;
using Scripts.Quests;
using Scripts.Audio;
using System;

namespace Scripts.Item
{
    public class DynamiteItem : MonoBehaviour
    {
        #region Variables
        [Header("Explosion Settings")]
        [SerializeField] private GameObject _explosionEffect;
        [SerializeField] private GameObject _miniBlockPrefab; // Prefab for mini-blocks
        [SerializeField] private GameObject _destroyableObject; // Object to destroy within radius
        [SerializeField] private float _explosionDelay;
        [SerializeField] private float _explosionRadius = 20f; // Radius of the explosion
        [SerializeField] private float _explosionForce = 2f; // Force applied to block
        [SerializeField] private float _blockSpawnRadius = 2f;
        [SerializeField] private int _blockCount = 40; // Number of mini-blocks

        [Header("UI Elements")]
        [SerializeField] private TextMeshPro _bubbleText;

        [Header("Audio")]
        [SerializeField] private AudioClip _explosionAudioClip;

        [Header("Player Effects")]
        [SerializeField] private const float HEALTHREDUCTIONVALUE = 0.25f;

        private bool _playerInRange = false;
        private bool _canDetonate = true;
        #endregion

        #region Start
        /// <summary>
        /// Removes the key prompt from appearing after dropping the item
        /// </summary>
        public void OnDisable()
        {
            if (GameManager.Instance.HasGameEnded == false)
            {
                GameScreen.Instance.HideKeyPrompt();
            }
        }
        #endregion

        #region Trigger Detection
        /// <summary>
        /// Detects when the player enters the trigger zone and shows a prompt to detonate.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (_canDetonate && other.CompareTag("Player"))
            {
                _playerInRange = true;
                GameScreen.Instance.ShowKeyPrompt("Press F to detonate");
            }
        }

        /// <summary>
        /// Detects when the player exits the trigger zone and hides the prompt.
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = false;
                GameScreen.Instance.HideKeyPrompt();
            }
        }
        #endregion

        #region Update 
        /// <summary>
        /// Monitors player input and starts the detonation sequence if the player is in range.
        /// </summary>
        void Update()
        {
            if (_playerInRange && PlayerManager.Instance.getTaskAccepted())
            {
                _canDetonate = false;
                GameScreen.Instance.HideKeyPrompt();
                StartCoroutine(Detonate());
            }
            
            
        }
        #endregion

        #region Detonation 
        /// <summary>
        /// Handles the entire detonation process: countdown, explosion effect, target destruction, block spawning, and block destruction.
        /// </summary>
        private IEnumerator Detonate()
        {
            // Start countdown and show bubble text
            yield return StartCoroutine(HandleCountdown());

            // Disable all child game objects
            Transform[] children = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child != transform)
                {
                    child.gameObject.SetActive(false);
                }
            }

            // Instantiate explosion effect
            InstantiateExplosionEffect();

            // Handle target object destruction if within explosion radius
            HandleTargetDestruction();

            // Handle player damage
            HandlePlayerDamage();

            // Spawn mini blocks and apply forces
            List<GameObject> spawnedBlocks = SpawnMiniBlocks();

            // Wait 7 seconds before destroying blocks
            yield return StartCoroutine(DestroyBlocksAfterDelay(spawnedBlocks, 7f));

            // Destroy the DynamiteItem object after the detonation process
            Destroy(gameObject);
        }

        /// <summary>
        /// Handles the countdown before the explosion, updating the bubble text and waiting for each second.
        /// </summary>
        private IEnumerator HandleCountdown()
        {
            int countdown = 3;
            while (countdown > 0)
            {
                _bubbleText.gameObject.SetActive(true);
                _bubbleText.text = $"{countdown}";
                yield return new WaitForSeconds(1f);
                countdown--;
            }
            _bubbleText.gameObject.SetActive(false);
        }
        #endregion

        #region Explosion 
        /// <summary>
        /// Instantiates the explosion effect at the dynamite item's position and plays the sound effect.
        /// </summary>
        private void InstantiateExplosionEffect()
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(_explosionAudioClip);
            }
        }

        /// <summary>
        /// Reduces the player's battery level if they are within the explosion radius.
        /// </summary>
        private void HandlePlayerDamage()
        {
            Vector3 playerPosition = PlayerManager.Instance.transform.position;

            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);
            if (distanceToPlayer <= _explosionRadius)
            {
                GameManager.Instance.SetBatteryLevelReduction(HEALTHREDUCTIONVALUE);
            }
        }
        /// <summary>
        /// Destroys the target object if it is within the explosion radius and updates mission status accordingly.
        /// </summary>
        private void HandleTargetDestruction()
        {
            if (_destroyableObject != null)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _destroyableObject.transform.position);

                if (distanceToTarget <= _explosionRadius)
                {
                    Destroy(_destroyableObject);
                    GameManager.Instance.SetMissionComplete("Blow Up Entrance");
                }
            }
        }
        #endregion

        #region Rubble Management
        /// <summary>
        /// Spawns a number of mini blocks at random positions within a defined radius from the dynamite item.
        /// Applies forces to the blocks to simulate explosion.
        /// </summary>
        private List<GameObject> SpawnMiniBlocks()
        {
            List<GameObject> spawnedBlocks = new List<GameObject>();
            for (int i = 0; i < _blockCount; i++)
            {
                Vector3 spawnPosition = transform.position + UnityEngine.Random.insideUnitSphere * _blockSpawnRadius;
                GameObject block = Instantiate(_miniBlockPrefab, spawnPosition, Quaternion.identity);
                Rigidbody blockRigidBody = block.GetComponent<Rigidbody>();

                if (blockRigidBody != null)
                {
                    Vector3 explosionDirection = (block.transform.position - transform.position).normalized + Vector3.up;
                    blockRigidBody.AddForce(explosionDirection * _explosionForce, ForceMode.Impulse);
                }
                spawnedBlocks.Add(block);
            }
            return spawnedBlocks;
        }

        /// <summary>
        /// Destroys the spawned blocks after a given delay.
        /// </summary>
        private IEnumerator DestroyBlocksAfterDelay(List<GameObject> blocks, float delay)
        {
            yield return new WaitForSeconds(delay);

            foreach (GameObject block in blocks)
            {
                if (block != null)
                {
                    Destroy(block);
                }
            }
        }
        #endregion
    }
}
