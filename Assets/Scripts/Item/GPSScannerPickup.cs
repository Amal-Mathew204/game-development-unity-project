using UnityEngine;
using System.Collections;
using PlayerCharacter = Scripts.Player.Player;
using Scripts.Player;
//Third Party Package
using Ilumisoft.RadarSystem;

namespace Scripts.Item
{
    public class GPSScannerPickup : ItemPickup
    {
        #region Class Variables
        
        public Radar radar;
        #endregion

        #region PickUp Methods

        /// <summary>
        /// Method to try picking up the item if the player is in range and presses the gather button
        /// Specifically for the GPS Scanner as we need to activate the radar
        /// </summary>
        public override void TryPickUp()
        {
            if (_isInRange && _playerActionInput.IsGathering && _playerState.CanGather())
            {
                _playerState.CurrentActionState = PlayerActionState.Gathering;
                ProcessPickUp();
            }
        }
        
        /// <summary>
        /// The method adds the current selected item into the players inventory
        /// This method also activates the Scanner Radar once the item has been picked up
        /// </summary>
        public override void ProcessPickUp()
        {
            if (PlayerCharacter.Instance.AddItem(this))
            {
                gameObject.SetActive(false);

                if(itemName == "GPS Scanner" && radar != null)
                {
                    PlayerCharacter.Instance.radar.ActivateRadar();
                       
                }
            }
        }
        #endregion
    }
}
