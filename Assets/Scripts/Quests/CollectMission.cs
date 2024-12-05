using System.Collections.Generic;
using System;

namespace Scripts.Quests
{
    public class CollectMission : Mission
    {
        private int _collectedItems = 0;
        private int _totalNumberOfItems;
        private List<string> _typeOfItems;

        /// <summary>
        /// Event that is triggered whenever a mission's data is updated
        /// </summary>
        public static event Action OnMissionUpdated = delegate { };

        /// <summary>
        /// Class Contructor for Collect Missions
        /// Collect Missions must set total number of items to be collected and any item names specific for the mission
        /// </summary>
        public CollectMission(string missionTitle, string missionInfo, int totalNumberOfItems, List<string> typeOfItems = null) : base(missionTitle, missionInfo)
        {
            _totalNumberOfItems = totalNumberOfItems;
            _typeOfItems = typeOfItems;
        }

        //Check if the collected Item is applicable by the mission
        private bool CheckCollectedItem(string itemName)
        {
            if(_typeOfItems == null)
            {
                return true;
            }
            return _typeOfItems.Contains(itemName);
        }

        /// <summary>
        /// Method checks if item is related to the mission
        /// If true, the item is registered collected by the mission and the method checks if mission is complete
        /// </summary>
        public void RegisterCollectedItem(string itemName)
        {
            if (CheckCollectedItem(itemName) == false)
            {
                return;
            }

            _collectedItems += 1;
            OnMissionUpdated.Invoke();

            if (_collectedItems >= _totalNumberOfItems)
            {
                SetMissionCompleted();

                // If the mission has a parent, check and update the parent's completion status
                if (ParentMission != null)
                {
                    ParentMission.CheckAndUpdateMissionCompletion();
                }
            }
        }

        /// <summary>
        /// Returns the progress of item collection in the format "collected/total"
        /// </summary>
        public string GetItemProgress()
        {
            return $"{_collectedItems}/{_totalNumberOfItems}";
        }

    }
}

