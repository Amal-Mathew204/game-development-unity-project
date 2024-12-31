using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.GarbageDisposal;
using Newtonsoft.Json;
using System;

namespace Scripts.Quests
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private GarbageDisposalController _garbageDisposalController;
        private int _oilDropped = 0;
        private bool _missionComplete = false;
        public List<GameObject> ItemsInDisposal = new List<GameObject>();


        #region Save/Load Methods

        public void Save()
        {
            Dictionary<string, object> containerData = new Dictionary<string, object>();
            containerData.Add("_missionComplete", _missionComplete);
            Dictionary<string, string> itemsNamesInDisposal = new Dictionary<string, string>();
            foreach (GameObject item in ItemsInDisposal)
            {
                itemsNamesInDisposal[item.name] = item.transform.position.ToString();
            }
            containerData.Add("ItemsInDisposal", JsonConvert.SerializeObject(itemsNamesInDisposal));
            
            PlayerPrefs.SetString("Container", JsonConvert.SerializeObject(containerData));
        }

        public void Load()
        {
            string dictionary = PlayerPrefs.GetString("Container");
            Dictionary<string, object> containerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dictionary);
            _missionComplete = (bool)containerData["_missionComplete"];
            Dictionary<string, string> itemsNamesInDisposal = JsonConvert.DeserializeObject<Dictionary<string, string>>((string)containerData["ItemsInDisposal"]);
            foreach (string itemName in itemsNamesInDisposal.Keys)
            {
                GameObject item = GameObject.Find(itemName);
                ItemsInDisposal.Add(item);
                item.transform.position = GameManager.StringToVector3(itemsNamesInDisposal[itemName]);
            }
            if (_missionComplete)
            {
                _garbageDisposalController.isActive = true;
            }
        }
        #endregion

        /// <summary>
        /// Updates the Place Barrel in Container Mission 
        /// </summary>
        private void Update()
        {
            if (!_missionComplete)
            {
                if (_oilDropped == 5)
                {
                    GameManager.Instance.SetMissionComplete("Place Barrels in Container");
                    //set GarbageDisposalController Active
                    DisablePickUpForOilBarrels();
                    _garbageDisposalController.isActive = true;
                    _missionComplete = true;
                }
                else
                {
                    CheckOilBarrelHaveLeftContainer();
                }
            }

        }

        /// <summary>
        /// Method for when player enters farm trigger box with all barrels
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///register barrel object when dropped 
            if (other.CompareTag("Barrel") && _missionComplete == false)
            {
                RegisterOilBarrelInContainer(other.gameObject);
                Debug.Log("Oil Dropped: " + _oilDropped);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            ///deregister barrel object when dropped 
            if (other.CompareTag("Barrel") && _missionComplete == false)
            {
               DeregisterOilBarrelInContainer(other.gameObject);
            }
        }

        /// <summary>
        /// This method disables the item pickup component for each barrel inside of the container
        /// </summary>
        private void DisablePickUpForOilBarrels()
        {
            foreach (GameObject item in ItemsInDisposal)
            {
                item.GetComponent<ItemPickup>().enabled = false;
            }
        }

        /// <summary>
        /// This method checks if all oil barrels are active inside of the Garbage Container
        /// The check is used in the update method to see if the user has placed a oil barrel back into its inventory
        /// </summary>
        private void CheckOilBarrelHaveLeftContainer()
        {
            List<GameObject> itemsToDeregister = new List<GameObject>();
            foreach (GameObject item in ItemsInDisposal)
            {
                if (!item.activeInHierarchy)
                {
                    itemsToDeregister.Add(item);
                }
            }

            foreach (GameObject item in itemsToDeregister)
            {
                DeregisterOilBarrelInContainer(item);
            }
            
        }

        /// <summary>
        /// This method registers an oil barrel game object inside the Garbage Container
        /// </summary>
        public void RegisterOilBarrelInContainer(GameObject item)
        {
            _oilDropped = _oilDropped + 1;
            ItemsInDisposal.Add(item);
        }

        /// <summary>
        /// This method deregisters an oil barrel game object inside the Garbage Container
        /// </summary>
        public void DeregisterOilBarrelInContainer(GameObject item)
        {
            _oilDropped = _oilDropped - 1;
            if (_oilDropped < 0)
            {
                _oilDropped = 0;
            }
            ItemsInDisposal.Remove(item);
        }
    }
}
