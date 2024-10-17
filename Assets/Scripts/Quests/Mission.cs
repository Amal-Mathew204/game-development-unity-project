using System;
using System.Collections.Generic;


namespace Scripts.Quests
{
    public class Mission
    {
        public string MissionTitle { get; private set; }
        public string MissionInfo { get; private set; }
        public List<Mission> SubMissions { get; private set; } = new List<Mission>();
        private bool _missionCompleted = false;

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
            this.SubMissions.Add(subMission);
        }

        /// <summary>
        /// Method adds a list of missions that must be completed within this Mission object
        /// </summary>
        public void AddSubMission(List<Mission> subMissions)
        {
            List<Mission> clone = new List<Mission>(subMissions);
            this.SubMissions.AddRange(clone);
        }

        /// <summary>
        /// Method sets the status of the Mission object to completed
        /// </summary>
        public void SetMissionCompleted()
        {
            _missionCompleted = true;
        }

        /// <summary>
        /// Returns the status of completion of the Mission object
        /// </summary>
        public bool IsMissionCompleted()
        {
            return _missionCompleted;
        }
    }
}
