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
    [SerializeField] private float _explosionDelay;
    [SerializeField] private float _explosionRadius = 20f; // Radius of the explosion
    [SerializeField] private float _explosionForce = 2f; // Force applied to block
    [SerializeField] private float _blockSpawnRadius = 2f;
    [SerializeField] private GameObject _destroyableObject; // Specify layers to check for destructible objects

    [SerializeField] private TextMeshPro _bubbleText;
    [SerializeField] private int _blockCount = 40; // Number of mini-blocks


    private MissionLogDropdown _dropdown;
    private bool _playerInRange = false;
    private bool _canDetonate = true;

    /// <summary>
    /// Works as the first frame of the game
    /// Finds Mission UI interface component and assigns to variable 
    /// </summary>
    public void Start()
    {
        _dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<MissionLogDropdown>();
    }

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

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
            GameScreen.Instance.HideKeyPrompt();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInRange && PlayerManager.Instance.getTaskAccepted())
        {
            {
                _canDetonate = false;

                GameScreen.Instance.HideKeyPrompt();
                

                StartCoroutine(Detonate());
            }
        }
    }
    public void OnDisable()
    {
        GameScreen.Instance.HideKeyPrompt();
        Debug.Log("DynamiteItem script disabled");
    }
    

    private void OnDestroy()
    {
        Debug.Log("DynamiteItem object destroyed");
    }

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
    
    private void InstantiateExplosionEffect()
    {
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Debug.Log("Explosion effect instantiated");
    }

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
