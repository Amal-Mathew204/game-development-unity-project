using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Game;
using UnityEngine.Rendering;

public class DynamiteItem : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private float _explosionDelay;

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
        if(_playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            {
                StartCoroutine(Detonate());
            }
        }
    }

    private IEnumerator Detonate()
    {
        GameScreen.Instance.HideKeyPrompt(); // Hide the key prompt
        Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

        yield return new WaitForSeconds(_explosionDelay);

        
    }
}
