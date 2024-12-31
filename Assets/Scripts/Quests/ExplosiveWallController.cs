using Scripts.Game;
using UnityEngine;

namespace Scripts.Quests
{
    public class ExplosiveWallController : MonoBehaviour
    {
        /// <summary>
        /// 
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
