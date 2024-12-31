using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.Quests
{
    public class SeedPlantingQuest : MonoBehaviour
    {
        private const int _totalSeeds = 4;
        private int _seedsPlanted = 0;
        private bool _missionCompleted = false;
        [SerializeField] private FarmTrigger[] _farmTriggers;

        
        #region Save and Load Methods

        public void Save()
        {
            Dictionary<string, object> seedQuestData = new Dictionary<string, object>();
            // seedQuestData.Add("_seedsPlanted", _seedsPlanted);
            // seedQuestData.Add("_missionCompleted", _missionCompleted);
            bool[] farmTriggersActive = new bool[_farmTriggers.Length];
            for (int i = 0; i < _farmTriggers.Length; i++)
            {
                farmTriggersActive[i] = _farmTriggers[i].IsSeedBagInside();
            }
            seedQuestData.Add("_farmTriggersActive", JsonConvert.SerializeObject(farmTriggersActive));
            PlayerPrefs.SetString("SeedPlantingQuest", JsonConvert.SerializeObject(seedQuestData));
        }

        public void Load()
        {
            Dictionary<string, object> seedQuestData = JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString("SeedPlantingQuest"));
            GameObject[] seedBags = GameObject.FindGameObjectsWithTag("SeedBag");
            bool[] farmTriggersActive = JsonConvert.DeserializeObject<bool[]>((string)seedQuestData["_farmTriggersActive"]);
            int seedBagsToRemoveIndex = 0;
            for (int i = 0; i < _farmTriggers.Length; i++)
            {
                if (farmTriggersActive[i])
                {
                    _farmTriggers[i].ActivatePlantVisuals(seedBags[seedBagsToRemoveIndex]);
                    seedBagsToRemoveIndex++;
                }
            }
        }
        #endregion
        
        /// <summary>
        /// Updates the Plant Seed Mission 
        /// </summary>
        private void Update()
        {
            if (_seedsPlanted == _totalSeeds && !_missionCompleted)
            {
                GameManager.Instance.SetMissionComplete("Plant Seed");
                _missionCompleted = true;
            }
        }

        /// <summary>
        /// Increments Plant seed Variable 
        /// </summary>
        public void IncrementSeedsPlanted()
        {
            _seedsPlanted += 1;
        }
    }

}
