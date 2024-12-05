using System.Collections.Generic;
using System;

namespace Scripts.Quests
{
    public class Mission
    {
        public string MissionTitle { get; private set; }
        public string MissionInfo { get; private set; }
        public Mission ParentMission { get; private set; }
        public List<Mission> SubMissions { get; private set; } = new List<Mission>();
        private bool _missionCompleted = false;

        /// <summary>
        /// Event that is triggered whenever the status of any mission changes 
        /// </summary>
        public static event Action OnMissionStatusUpdated = delegate { };

        /// <summary>
        /// On Class Intialisation set the missions title and information
        /// </summary>
        public Mission(string missionTitle, string missionInfo)
        {
            this.MissionTitle = missionTitle;
            this.MissionInfo = missionInfo;
        }

        /// <summary>
        /// Method adds any missions that must be completed within this Mission object
        /// </summary>
        public void AddSubMission(Mission subMission)
        {
            subMission.ParentMission = this;
            this.SubMissions.Add(subMission);
        }

        /// <summary>
        /// Method adds a list of missions that must be completed within this Mission object
        /// </summary>
        public void AddSubMission(List<Mission> subMissions)
        {
            List<Mission> clone = new List<Mission>(subMissions);
            foreach (Mission subMission in subMissions)
            {
                subMission.ParentMission = this; // Set the parent mission for each sub-mission
                this.SubMissions.Add(subMission);
            }
        }

        /// <summary>
        /// This method checks if a mission has sub missions
        /// </summary>
        public bool hasSubMissions()
        {
            return this.SubMissions.Count != 0;
        }


        /// <summary>
        /// Method sets the status of the Mission object to completed
        /// </summary>
        public void SetMissionCompleted()
        {
            _missionCompleted = true;
            OnMissionStatusUpdated.Invoke();

            if (ParentMission != null)
            {
                ParentMission.CheckAndUpdateMissionCompletion();
            }

        }

        /// <summary>
        /// Returns the status of completion of the Mission object
        /// </summary>
        public bool IsMissionCompleted()
        {
            return _missionCompleted;
        }

        /// <summary>
        /// Check all sub-missions and update the completion status of this mission.
        /// </summary>
        public void CheckAndUpdateMissionCompletion()
        {
            if (SubMissions.Count > 0 && SubMissions.TrueForAll(subMission => subMission.IsMissionCompleted()))
            {
                SetMissionCompleted();
            }
        }
    }
}
