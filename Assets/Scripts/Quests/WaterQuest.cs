using Scripts.Game;
using UnityEngine;

namespace Scripts.Quests
{
    public class WaterQuest : MonoBehaviour
    {
        /// <summary>
        /// Detects when the player enters the trigger zone.
        /// Marks the "Water Source Location" mission as completed and updates the mission log UI accordingly.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && GameManager.Instance.GetMission("Water Source Location").IsMissionCompleted() == false)
            {
                GameManager.Instance.SetMissionComplete("Water Source Location");
            }
        }
    }
}

