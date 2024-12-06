using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using PlayerManager = Scripts.Player.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Item
{
  public class PipeSnapChecker : MonoBehaviour
  {
    private bool _snapperActive = false;
    private bool _isInTriggerBox = false;
    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        _isInTriggerBox = true;
        if (_snapperActive == false)
        {
          GameScreen.Instance.ShowKeyPrompt("Confirm Set Pipe");
        }

      }
      if (other.CompareTag("Pipe") && _snapperActive)
      {
        Debug.Log("Pipe snapped");
      }
    }

    private void OnDisable()
    {
      _snapperActive = false;
      _isInTriggerBox = false;
      GameScreen.Instance.HideKeyPrompt();
    }

    private void Update()
    {
      if (_isInTriggerBox && _snapperActive == false)
      {
        if (PlayerManager.Instance.getTaskAccepted())
        {
          GameScreen.Instance.HideKeyPrompt();
          _snapperActive = true;
        }
      }
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        _isInTriggerBox = false;
        GameScreen.Instance.HideKeyPrompt();
      }
    }
  }  
}

