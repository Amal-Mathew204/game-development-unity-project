using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using UnityEngine.Rendering;
using PlayerManager = Scripts.Player.Player;
using Unity.VisualScripting;
using System.Threading;
using TMPro;

public class DynamiteItem : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _explosionDelay;
    [SerializeField] private float _explosionRadius = 5f; // Radius of the explosion
    [SerializeField] private float _explosionForce = 2f; // Force applied to block
    [SerializeField] private GameObject _targetObject; // Specify layers to check for destructible objects
    [SerializeField] private List<GameObject> _invisibleBlocks;
    [SerializeField] private TextMeshPro _bubbleText;

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

                GameScreen.Instance.HideKeyPrompt();
                

                StartCoroutine(Detonate());
            }
        }
    }

    private IEnumerator Detonate()
    {

        int countdown = 3;

        // Show bubble text and countdown
        while (countdown > 0)
        {
            _bubbleText.gameObject.SetActive(true); // Ensure bubble text is visible
            _bubbleText.text = $"{countdown}";      // Update countdown text
            yield return new WaitForSeconds(1f);   // Wait 1 second
            countdown--;
        }

        _bubbleText.gameObject.SetActive(false);

        //yield return new WaitForSeconds(_explosionDelay);

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
