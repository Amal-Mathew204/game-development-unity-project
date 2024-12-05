using UnityEngine;
using System.Collections;
using Scripts.Game;
using Scripts.Quests;
using UnityEngine.UI;

namespace Scripts.GarbageDisposal
{
    public class GarbageDisposalController : MonoBehaviour
    {
        #region Class Variables
        public bool isActive { get; set; } = false;
        private bool _launchedGarbage = false;
        private GameObject _garbageDetonateButton;
        #endregion

        #region Start Methods
        /// <summary>
        /// Start Method Gets a reference to the Garbage Detonate Button and if found it sets its on click method
        /// </summary>
        private void Start()
        {
            _garbageDetonateButton = GetGarbageDetonateButton();
            if(_garbageDetonateButton != null)
            {
                _garbageDetonateButton.GetComponent<Button>().onClick.AddListener(LaunchGarbage);
            }
        }
        #endregion

        #region Trigger Box Methods
        /// <summary>
        /// The OnTrigger Method (when player enters the trigger box)
        /// sets the the GarbageDetonateButton to active
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isActive && _launchedGarbage == false)
            {
                ToggleGarbageDetonateButtonActiveState(true);
            }
        }

        /// <summary>
        /// The OnTrigger Method (when player leaves trigger box)
        /// sets the the GarbageDetonateButton to inactive
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && isActive)
            {
                ToggleGarbageDetonateButtonActiveState(false);
            }
        }
        #endregion

        #region Garbage Detonation Methods
        /// <summary>
        /// This method is used to add an explosive force to each Garbage Object Registered in the
        /// Garbage disposal container
        /// </summary>
        public void LaunchGarbage()
        {
            if (_launchedGarbage)
            {
                return;
            }

            //Get RigidBody Objects
            Container container = GetComponentInChildren<Container>();
            foreach (GameObject garbageItem in container.ItemsInDisposal)
            {
                Rigidbody itemRigidBody = garbageItem.GetComponent<Rigidbody>();
                itemRigidBody.AddForce(new Vector3(0f, 100000f, 100000f));
            }
            _launchedGarbage = true;
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// This method locates the GarbageDetonateButton object. If found it sets the active state of the gameobject based of the
        /// method argument
        /// </summary>
        private void ToggleGarbageDetonateButtonActiveState(bool isActive)
        {
            if(_garbageDetonateButton == null)
            {
                Debug.LogError("Garbage Detonate Button is null");
                return;
            }
            _garbageDetonateButton.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// This method gets the GarbageDetonateButton object from the GameScreen
        /// </summary>
        private GameObject GetGarbageDetonateButton()
        {
            Transform garbageDetonateButton = GameScreen.Instance.transform.Find("GarbageDetonateButton");
            if (garbageDetonateButton == null)
            {
                Debug.LogError("Garbage Detonate Button can not be found");
                return null;
            }
            else
            {
               return garbageDetonateButton.gameObject;
            }
        }
        #endregion
    }
}
