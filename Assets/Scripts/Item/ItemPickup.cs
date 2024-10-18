using UnityEngine;
using UnityEngine.Playables;
using Scripts.Player;
using Scripts.Player.Input;
using PlayerCharacter = Scripts.Player.Player;


namespace Scripts.Item
{
    public class ItemPickup : MonoBehaviour
    {
        public string itemName;  
        private bool _isInRange = false;  // This will be true when the player is looking at the item

        private PlayerState _playerState;
        private PlayerActionInput _playerActionInput;


        private void Start()
        {
            _playerState = PlayerCharacter.Instance.gameObject.GetComponent<PlayerState>();
            _playerActionInput = PlayerCharacter.Instance.gameObject.GetComponent<PlayerActionInput>();
            if (_playerState == null)
            {
                Debug.LogError("Player State is null");
            }
            if(_playerActionInput == null)
            {
                Debug.LogError("Player State is null");
            }
        }
        /// <summary>
        /// This method will be called when the raycast hits this object
        /// </summary>
        public void OnRaycastHit()
        {
            //Debug.Log("Press E to pick up " + itemName);
            _isInRange = true;  
        }

        /// <summary>
        /// This method will be called when the raycast no longer hits this object
        /// </summary>
        public void OnRaycastExit()
        {
            //Debug.Log("Out of range of " + itemName);
            _isInRange = false;
        }
        /// <summary>
        /// Method to try picking up the item if the player is in range and presses the interact button
        /// </summary>
        public void TryPickUp()
        {
            if (_isInRange && _playerActionInput.IsGathering && _playerState.CanGather())
            {
                _playerState.CurrentActionState = PlayerActionState.Gathering;
                if (PlayerCharacter.Instance.AddItem(this))
                {
                    //TODO: Confirm if we can save a copy of the game object in inventory so ItemPickUp class component remains in game
                    gameObject.SetActive(false);
                }
            }
        }
    }

}
