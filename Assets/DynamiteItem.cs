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
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;

public class DynamiteItem : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _miniBlockPrefab; // Prefab for mini-blocks
    [SerializeField] private TextMeshPro _bubbleText;
    [SerializeField] private GameObject _destroyableObject; // Specify layers to check for destructible objects

    [SerializeField] private float _explosionDelay;
    [SerializeField] private float _explosionRadius = 20f; // Radius of the explosion
    [SerializeField] private float _explosionForce = 2f; // Force applied to block
    [SerializeField] private float _blockSpawnRadius = 2f;
    [SerializeField] private int _blockCount = 40; // Number of mini-blocks

    private MissionLogDropdown _dropdown;
    private bool _playerInRange = false;
    private bool _canDetonate = true;

    /// <summary>
    /// Finds the Mission UI component and assigns it to the dropdown variable.
    /// Called when the script starts.
    /// </summary>
    public void Start()
    {
        _dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<MissionLogDropdown>();
    }

    /// <summary>
    /// Detects when the player enters the trigger zone and shows a prompt to detonate.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (_canDetonate)
        {
            if (other.CompareTag("Player"))
            {
                _playerInRange = true;
                GameScreen.Instance.ShowKeyPrompt("Press F to detonate");
            }
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

    /// <summary>
    /// Ensures the key prompt is hidden when the object is disabled.
    /// </summary>
    public void OnDisable()
    {
        GameScreen.Instance.HideKeyPrompt();
    }

    /// <summary>
    /// Handles the entire detonation process: countdown, explosion effect, target destruction, block spawning, and block destruction.
    /// </summary>
    private IEnumerator Detonate()
    {
        // Start countdown and show bubble text
        yield return StartCoroutine(HandleCountdown());

        // Instantiate explosion effect
        InstantiateExplosionEffect();

        // Handle target object destruction if within explosion radius
        HandleTargetDestruction();

        // Spawn mini blocks and apply forces
        List<GameObject> spawnedBlocks = SpawnMiniBlocks();

        // Wait 5 seconds before destroying blocks
        yield return StartCoroutine(DestroyBlocksAfterDelay(spawnedBlocks, 5f));

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

    /// <summary>
    /// Instantiates the explosion effect at the dynamite item's position.
    /// </summary>
    private void InstantiateExplosionEffect()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Debug.Log("Explosion effect instantiated");
    }

    /// <summary>
    /// Destroys the target object if it is within the explosion radius.
    /// Updates mission status accordingly.
    /// </summary>
    private void HandleTargetDestruction()
    {
        if (_destroyableObject != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, _destroyableObject.transform.position);

            if (distanceToTarget <= _explosionRadius)
            {
                Destroy(_destroyableObject);
                Mission mission = _dropdown.GetMission("Blow Up Entrance");
                mission.SetMissionCompleted();

                if (_dropdown.MissionTitles.FindIndex(title => title == mission.MissionTitle) + 1 == _dropdown.dropdown.value)
                {
                    _dropdown.UpdateCompletionStatus(true);
                }
            }
            else
            {
                Debug.Log("Target object too far away");
            }
        }
    }

    /// <summary>
    /// Spawns a number of mini blocks at random positions within a defined radius from the dynamite item.
    /// Applies forces to the blocks to simulate explosion.
    /// </summary>
    private List<GameObject> SpawnMiniBlocks()
    {
        List<GameObject> spawnedBlocks = new List<GameObject>();
        for (int i = 0; i < _blockCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * _blockSpawnRadius;
            GameObject block = Instantiate(_miniBlockPrefab, spawnPosition, Quaternion.identity);
            Rigidbody blockRigidBody = block.GetComponent<Rigidbody>();

            if (blockRigidBody != null)
            {
                Vector3 explosionDirection = (block.transform.position - transform.position).normalized + Vector3.up;
                blockRigidBody.AddForce(explosionDirection * _explosionForce, ForceMode.Impulse);
            }
            spawnedBlocks.Add(block);
        }
        Debug.Log("Blocks created");
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
                Debug.Log($"Destroyed block: {block.name}");
            }
        }
    }
}
