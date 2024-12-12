using UnityEngine;
using System.Collections;
using System;
using Scripts.Game;
using Scripts.Quests;
using UnityEngine.UI;

namespace Scripts.GarbageDisposal
{
    public class GarbageDisposalController : MonoBehaviour
    {
        #region Class Variables
        public bool isActive { get; set; } = false;
        private bool _handelGarbage = false;
        private GameObject _garbageDetonateButton;
        [SerializeField] private GameObject _plasmaExplosion;
        private bool _plasmaActivated = false;
        [SerializeField] private float _plasmaExpansionTime = 10f;
        private bool _itemsLaunched = false;
        [SerializeField] private float _itemLiveTime = 5f;
        private float _timer = 0f;
        private AudioSource _source;
        #endregion

        #region Start Methods
        /// <summary>
        /// Start Method Gets a reference to the Garbage Detonate Button and if found it sets its on click method
        /// </summary>
        private void Start()
        {
            _garbageDetonateButton = GetGarbageDetonateButton();
            _source = GetComponent<AudioSource>();
            if (_garbageDetonateButton != null)
            {
                _garbageDetonateButton.GetComponent<Button>().onClick.AddListener(HandleGarbageDisposal);
            }
        }
        #endregion


        #region Update
        /// <summary>
        /// Update Methods handles either two conditons
        /// Update method sets a timer for when the plasma is activated, after a certain time the player will lose the game
        /// Update method sets a timer for when the items have been launched. After 5s there are deleted from the game
        /// </summary>
        public void Update()
        {
            if (_plasmaActivated)
            {
                _timer += Time.deltaTime;
                if(_timer > _plasmaExpansionTime)
                {
                    GameManager.Instance.SetPlayerHasLost();
                    _plasmaActivated = false;
                }
            }
            else if (_itemsLaunched)
            {
                _timer += Time.deltaTime;
                if (_timer > _itemLiveTime)
                { 
                    _itemsLaunched = false;
                    DeleteGarbageItems();
                }
            }
        }

        /// <summary>
        /// This method deletes all the items (registered) inside of the garbage container
        /// </summary>
        private void DeleteGarbageItems()
        {
            Container container = GetComponentInChildren<Container>();
            foreach (GameObject garbageItem in container.ItemsInDisposal)
            {
                Destroy(garbageItem);
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
            if (other.CompareTag("Player") && isActive && _handelGarbage == false)
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
        /// This handes the DO NOT PRESS on click event.
        /// Either the garbage launches to space (deleting within 5s)
        /// Or the world detonates
        /// This method only executes its logic once
        /// </summary>
        public void HandleGarbageDisposal()
        {
            if (_handelGarbage)
            {
                return;
            }

            //Set a 99% change to detonate world
            //Set a 1% chance for garbage to launchs to space
            System.Random random = new System.Random();
            int choice = random.Next(1, 101);
            if (choice == 1)
            {
                LaunchGarbage();
            }
            else
            {
                DetonateWorld();
            }
            _handelGarbage = true;
            ToggleGarbageDetonateButtonActiveState(false);
        }

        /// <summary>
        /// This method is used to add a force to each Garbage Object Registered in the
        /// Garbage disposal container
        /// </summary>
        public void LaunchGarbage()
        {
            //Get RigidBody Objects and add force
            Container container = GetComponentInChildren<Container>();
            foreach (GameObject garbageItem in container.ItemsInDisposal)
            {
                Rigidbody itemRigidBody = garbageItem.GetComponent<Rigidbody>();
                itemRigidBody.AddForce(new Vector3(0f, 100000f, -100000f));
            }
            _itemsLaunched = true;
        }

        /// <summary>
        /// This method will enable the effects of the player detonating the word
        /// </summary>
        public void DetonateWorld()
        {
            _plasmaExplosion.SetActive(true);
            _plasmaActivated = true;
            _source.Play();
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
