using UnityEngine;
using UnityEngine.Playables;
using Scripts.Player;
using Scripts.Player.Input;
using PlayerCharacter = Scripts.Player.Player;
using TMPro;

namespace Scripts.Item
{
    public class ItemPickup : MonoBehaviour
    {
        public string itemName;
        [SerializeField] private TMP_Text _itemLabel;
        protected bool _isInRange = false;  // This will be true when the player is looking at the item
        protected PlayerState _playerState;
        protected PlayerActionInput _playerActionInput;

        /// <summary>
        /// Initializes the item's dependencies by retrieving the player's state and action input components.
        /// Logs an error if these components are not found. Also updates the item's label with its name.
        /// </summary>

        private void OnEnable()
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

            SetItemLabel();
        }

        /// <summary>
        /// Method Sets Item Label to the Name of the Item
        /// </summary>
        private void SetItemLabel()
        {
            _itemLabel.text = itemName;
        }

        /// <summary>
        /// This method will be called when the raycast hits this object
        /// </summary>
        public void OnRaycastHit()
        {
            _isInRange = true;  
        }

        /// <summary>
        /// This method will be called when the raycast no longer hits this object
        /// </summary>
        public void OnRaycastExit()
        {
            _isInRange = false;
        }

        /// <summary>
        /// Method to try picking up the item if the player is in range and presses the gather button
        /// </summary>
        public virtual void TryPickUp()
        {
            if (_isInRange && _playerActionInput.IsGathering && _playerState.CanGather())
            {
                _playerState.CurrentActionState = PlayerActionState.Gathering;
                ProcessPickUp();
            }
        }

        /// <summary>
        /// The method adds the current selected item into the players inventory
        /// </summary>
        public virtual void ProcessPickUp()
        {
            if (PlayerCharacter.Instance.AddItem(this))
            {
                gameObject.SetActive(false);
            }
        }
    }

}
