using UnityEngine;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.MiniMissionLog
{
    public class MiniMissionLogController : MonoBehaviour
    {
        [SerializeField] private GameObject _missionLogContainer;
        /// <summary>
        /// At the Start of the Scene the MiniMissionUI Container should not be displayed
        /// </summary>
        private void Start()
        {
            _missionLogContainer.SetActive(false);
        }

        /// <summary>
        /// Method Sets the Mission Information Container to visible
        /// </summary>
        public void OpenMissionLog()
        {
            _missionLogContainer.SetActive(true);
        }
        /// <summary>
        /// Method Sets the Mission Information Container to hidden
        /// </summary>
        public void CloseMissionLog()
        {
            _missionLogContainer.SetActive(false);
        }
    }
}