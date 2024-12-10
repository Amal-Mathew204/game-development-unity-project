using UnityEngine;
using GameManager = Scripts.Game.GameManager;

namespace Scripts.Quests
{
    public class SeedPlantingQuest : MonoBehaviour
    {
        [SerializeField] private const int _totalSeeds = 4;
        private int _seedsPlanted = 0;
        private bool _missionCompleted = false;

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
