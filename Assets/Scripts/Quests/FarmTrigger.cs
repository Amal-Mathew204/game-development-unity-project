using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Item;
using System.Collections.Generic;

namespace Scripts.Quests
{
    public class FarmTrigger : MonoBehaviour
    {
        private bool _isSeedBagInside = false;
        [SerializeField] private GameObject _dirtPile;
        [SerializeField] private GameObject _arrowIndicator;
        [SerializeField] private SeedPlantingQuest _seedPlantingQuest;

        /// <summary>
        /// Method for when player enters farm trigger box with all seeds
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            ///Checks if seed bag has been dropped and if there is also no other seed bags
            if (other.CompareTag("SeedBag") && _isSeedBagInside == false)
            {
                ActivatePlantVisuals(other.gameObject);
            }
        }

        /// <summary>
        /// Method for activating plant visuals( activate plant deactivate arrow )
        /// </summary>
        public void ActivatePlantVisuals(GameObject seedBag)
        {
            ///Destroys seed bag and actived dirtpile object
            ///Arrow indicator is also decativited and the planting quest is incremeted
            Destroy(seedBag);
            _isSeedBagInside = true;
            _dirtPile.SetActive(true);
            _arrowIndicator.SetActive(false);
            _seedPlantingQuest.IncrementSeedsPlanted();
        }

        /// <summary>
        /// returns state of _isSeedBagInside
        /// </summary>
        public bool IsSeedBagInside()
        {
            return _isSeedBagInside;
        }
    }
}

