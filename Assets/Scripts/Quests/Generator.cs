using UnityEngine;
using System;
using System.Collections;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.GarbageDisposal;
using Scripts.Quests;
using Scripts.Audio ;
using Newtonsoft.Json;

namespace Scripts.Quests
{
    public class Generator : MonoBehaviour
    {
        private int _fuelCellDropped = 0;
        private bool _missionComplete = false;
        [SerializeField] private AudioClip _source;
        [SerializeField] private GameObject _rechargePoint;

        #region Save/Load Methods

        /// <summary>
        /// method saves missionComplete and fuelCellDropped as serialized JSON string in PlayerPrefs
        /// </summary>
        public void Save()
        {
            Dictionary<string,object> generatorData = new Dictionary<string, object>();
            generatorData.Add("_missionComplete", _missionComplete);
            generatorData.Add("_fuelCellDropped", _fuelCellDropped);
            PlayerPrefs.SetString("Generator", JsonConvert.SerializeObject(generatorData));
        }

        /// <summary>
        /// method loads data from player prefs which updates mission complete and fuel cells dropped
        /// it also destroys corresponding amount of fuel cells and reactivates charging station if mission has been completed 
        /// </summary>
        public void Load()
        {
            string dictionary = PlayerPrefs.GetString("Generator");
            Dictionary<string, object> generatorData = JsonConvert.DeserializeObject<Dictionary<string, object>>(dictionary);
            _missionComplete = (bool)generatorData["_missionComplete"];
            _fuelCellDropped = Convert.ToInt32(generatorData["_fuelCellDropped"]);
            GameObject[] fuelCells = GameObject.FindGameObjectsWithTag("FuelCell");
            for (int i = 0; i < _fuelCellDropped; i++)
            {
                Destroy(fuelCells[i]);
            }
            if (_missionComplete)
            {
                _rechargePoint.SetActive(true);
            }
        }
        #endregion


        /// <summary>
        /// Updates the Turn on Generator Misison in Container Mission
        /// On completion of the Turn on Generator Mission, the rechargePoint gameobject is activated
        /// </summary>
        private void Update()
        {
            if (_fuelCellDropped == 3 && _missionComplete == false)
            {
                GameManager.Instance.SetMissionComplete("Turn on Generator");
                _rechargePoint.SetActive(true);
                _missionComplete = true;
            }
        }

        /// <summary>
        /// Method for when player enters farm trigger box with all Fuel Cell
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Destroys Fuel Cell object when dropped
            ///Play SFX
            if (other.CompareTag("FuelCell"))
            {
                _fuelCellDropped = _fuelCellDropped + 1;
                AudioManager.Instance.PlaySFX(_source);
                Destroy(other.gameObject);
            }
        }
    }
}
