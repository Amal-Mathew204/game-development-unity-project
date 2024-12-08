using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using PlayerManager = Scripts.Player.Player;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEditor.Networking.PlayerConnection;

namespace Scripts.Item
{
  public class PipeBencSnapChecker : PipeSnapChecker
  {
    protected  override void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag("Player"))
      {
        _isInTriggerBox = true;
        if (SnapperActive == false)
        {
          GameScreen.Instance.ShowKeyPrompt("Confirm Set Pipe");
        }

      }

      if (other.CompareTag("Pipe") && SnapperActive)
      {
        PipeSnapChecker otherPipeSnapChecker = other.gameObject.GetComponentInChildren<PipeSnapChecker>();
        if (otherPipeSnapChecker == null)
        {
          return;
        }
            
        //force drop pipe to be in same direction as set pipe 
        other.transform.rotation = Quaternion.Euler(0, _parentOfItemObject.transform.eulerAngles.y, 0);
        
        //Check Which snap point end is closer to the other pipe 
        float distance1 = Vector3.Distance(_snapPointOne.transform.position, other.transform.position);
        float distance2 = Vector3.Distance(_snapPointTwo.transform.position, other.transform.position);
        if (Math.Abs(distance1) > _maxDistance && Math.Abs(distance2) > _maxDistance)
        {
          Debug.Log(other.name + " is out of range.");
          return;
        }

        if (Math.Abs(distance1) > Math.Abs(distance2))
        {
          Debug.Log("Snapper Point 2 is closer to the pipe");
          Vector3 offset = _snapPointTwo.transform.position - otherPipeSnapChecker._snapPointOne.transform.position;
          MoveOtherPipe(other, offset);
        }
        else
        {
            Debug.Log("Snapper Point 1 is closer to the pipe");
            other.transform.rotation = Quaternion.Euler(0, _parentOfItemObject.transform.eulerAngles.y + 90, 0);
            Vector3 offset = _snapPointOne.transform.position - otherPipeSnapChecker._snapPointOne.transform.position;
            MoveOtherPipe(other, offset);
        }
    
      }
    }
  }
}

