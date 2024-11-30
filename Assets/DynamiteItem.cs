using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using UnityEngine.Rendering;
using PlayerManager = Scripts.Player.Player;

public class DynamiteItem : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _explosionDelay;
    [SerializeField] private float _explosionRadius = 5f; // Radius of the explosion
    [SerializeField] private float _explosionForce = 2f; // Force applied to block
    [SerializeField] private GameObject _targetObject; // Specify layers to check for destructible objects
    [SerializeField] private List<GameObject> _invisibleBlocks;

    private bool _playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
            GameScreen.Instance.ShowKeyPrompt("Press F to detonate");
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
                StartCoroutine(Detonate());
            }
        }
    }

    private IEnumerator Detonate()
    {

        yield return new WaitForSeconds(_explosionDelay);

        GameScreen.Instance.HideKeyPrompt(); // Hide the key prompt
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);

        if (_targetObject != null)
        {
            Destroy(_targetObject);
        }

        

        foreach (GameObject block in _invisibleBlocks)
        {
            block.SetActive(true);

            Rigidbody blockRigidBody = block.GetComponent<Rigidbody>();
            if (blockRigidBody != null)
            {
                Vector3 explosionDirection = Vector3.up + Random.insideUnitSphere;
                blockRigidBody.AddForce(explosionDirection * _explosionForce, ForceMode.Impulse);
            }


        }
        

    }
}
