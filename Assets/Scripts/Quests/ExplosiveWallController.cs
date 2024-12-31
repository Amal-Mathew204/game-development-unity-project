using Scripts.Game;
using UnityEngine;

namespace Scripts.Quests
{
    public class ExplosiveWallController : MonoBehaviour
    {
        /// <summary>
        /// destroys object if mission is marked as completed in the GameManager
        /// </summary>
        private void Start()
        {
            if (GameManager.Instance.GetMission("Blow Up Entrance").IsMissionCompleted())
            {
                Destroy(this.gameObject);
            }
        }
    }
}
