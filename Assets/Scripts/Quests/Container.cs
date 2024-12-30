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
            containerData.Add("_oilDropped", _oilDropped);
            PlayerPrefs.SetString("Container", JsonConvert.SerializeObject(containerData));
        }

        public void Load()
        {
            string dictionary = PlayerPrefs.GetString("Container");
            Dictionary<string, object> containerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dictionary);
            _missionComplete = (bool)containerData["_missionComplete"];
            _oilDropped = Convert.ToInt32(containerData["_oilDropped"]);
            GameObject[] oilBarrels = GameObject.FindGameObjectsWithTag("Barrel");
            Debug.Log(oilBarrels.Length);
            for (int i = 0; i < _oilDropped; i++)
            {
                Destroy(oilBarrels[i]);
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
            if (_oilDropped == 5 && _missionComplete==false)
            {
                 GameManager.Instance.SetMissionComplete("Place Barrels in Container");
                //set GarbageDisposalController Active
                _garbageDisposalController.isActive = true;
                _missionComplete = true;
            }
        }

        /// <summary>
        /// Method for when player enters farm trigger box with all barrels
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Destroys barrel object when dropped 
            if (other.CompareTag("Barrel"))
            {
                _oilDropped = _oilDropped + 1;
                ItemsInDisposal.Add(other.gameObject);
            }
        }
    }
}
