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
  public class PipeSnapChecker : MonoBehaviour
  {
        [SerializeField] private GameObject _parentOfItemObject;  
        [SerializeField] private Rigidbody _itemRigidbody;
        public bool SnapperActive = false;
        private bool _isInTriggerBox = false;
        [SerializeField] private GameObject _snapPointOne;
        [SerializeField] private GameObject _snapPointTwo;
        private float _maxDistance = 10f;
    
        private void OnTriggerEnter(Collider other)
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

            Vector3 otherSnapPointOne = otherPipeSnapChecker._snapPointOne.transform.position;
            Vector3 otherSnapPointTwo = otherPipeSnapChecker._snapPointTwo.transform.position;
            Vector3 pipeSnapPointOne = _snapPointOne.transform.position;
            Vector3 pipeSnapPointTwo = _snapPointTwo.transform.position;
            
            
            Vector3 offset3 = pipeSnapPointOne - otherSnapPointTwo;
            Vector3 offset4 = pipeSnapPointTwo - otherSnapPointOne;
            bool connected = CheckOffsetSnapPoints(other, offset3, _snapPointOne, otherPipeSnapChecker._snapPointTwo ) ? true : CheckOffsetSnapPoints(other,offset4, _snapPointTwo, otherPipeSnapChecker._snapPointOne );

            if (!connected)
            {
                Debug.Log("pipes not connected");
            }
    
          }
        }

        private bool CheckOffsetSnapPoints(Collider other, Vector3 offset, GameObject pipeSnapPoint, GameObject otherSnapPoint)
        {
          Debug.Log("Check Offset Snap Points" + (offset.magnitude <= _maxDistance));        
          if (offset.magnitude <= _maxDistance)
          {
            MoveOtherPipe(other, offset);
            float angle = (Vector3.Angle(pipeSnapPoint.transform.right, otherSnapPoint.transform.right));
            Debug.Log("Angle: " + angle);
            if (Convert.ToInt32(angle) == 90 || Convert.ToInt32(angle) == -90)
            {
              other.transform.rotation = Quaternion.Euler(0, _parentOfItemObject.transform.eulerAngles.y+90, 0);
              angle = (Vector3.Angle(pipeSnapPoint.transform.right, otherSnapPoint.transform.right));
              Debug.Log("Angle: " + angle);
              Vector3 newOffset = pipeSnapPoint.transform.position - otherSnapPoint.transform.position;
              MoveOtherPipe(other, newOffset);
            }
            return true;
          }
          return false;
        }

        private void MoveOtherPipe(Collider other, Vector3 offset)
        {
          
                other.transform.position += offset;
                other.GetComponent<Rigidbody>().isKinematic = true;
                Debug.Log("snap to other pipe");
            
        }
        private void OnDisable()
        {
          SnapperActive = false;
          _isInTriggerBox = false;
          GameScreen.Instance.HideKeyPrompt();
          _itemRigidbody.isKinematic = false;
        }

        private void Update()
        {
          if (_isInTriggerBox && SnapperActive == false)
          {
            if (PlayerManager.Instance.getTaskAccepted())
            {
              GameScreen.Instance.HideKeyPrompt();
              SnapperActive = true;
              _itemRigidbody.isKinematic = true;
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

