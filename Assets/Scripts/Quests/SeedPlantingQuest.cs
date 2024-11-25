using UnityEngine;
using System.Collections;
using MissionLogDropdown = Scripts.MissonLogMenu.Dropdown;
namespace Scripts.Quests
{
    public class SeedPlantingQuest : MonoBehaviour
    {
        private MissionLogDropdown _dropdown;
        [SerializeField] private const int _totalSeeds = 4;
        private int _seedsPlanted = 0;


        /// <summary>
        /// Works as the first frame of the game
        /// Finds Mission UI interface component and assigns to variable 
        /// </summary>
        private void Start()
        {
            _dropdown = GameObject.FindGameObjectWithTag("MissionUI").GetComponent<MissionLogDropdown>();

        }

        /// <summary>
        /// Updates the Plant Seed Mission 
        /// </summary>
        private void Update()
        {
            if (_seedsPlanted == _totalSeeds)
            {
                ///Gets Plant seed missions
                if(_dropdown == null)
                {
                    Debug.LogError("Mission UI Dropdown Component not Found");
                }
                Mission mission = _dropdown.GetMission("Plant Seed");
                if(mission == null)
                {
                    Debug.LogError("Mission Plant Farm Not Found");
                }
                ///Marks Mission as completed 
                mission.SetMissionCompleted();
                ///Updates Completion Status
                if (_dropdown.MissionTitles.FindIndex(title => title == mission.MissionTitle) + 1 == _dropdown.dropdown.value)
                {
                    _dropdown.UpdateCompletionStatus(true);
                }
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
