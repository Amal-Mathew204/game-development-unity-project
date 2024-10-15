using Scripts.Player;
using UnityEngine;
using UnityEngine.Playables;

public class ItemPickup : MonoBehaviour
{
    public string itemName;  
    private bool _isInRange = false;  // This will be true when the player is looking at the item

    private PlayerState _playerState;


    void Start()
    {
        _playerState = Player.Instance.gameObject.GetComponent<PlayerState>();
        if(_playerState == null)
        {
            Debug.LogError("Player State is null");
        }
    }
    /// <summary>
    /// This method will be called when the raycast hits this object
    /// </summary>
    public void OnRaycastHit()
    {
        Debug.Log("Press E to pick up " + itemName);
        _isInRange = true;  
    }

    /// <summary>
    /// This method will be called when the raycast no longer hits this object
    /// </summary>
    public void OnRaycastExit()
    {
        Debug.Log("Out of range of " + itemName);
        _isInRange = false;
    }
    /// <summary>
    /// Method to try picking up the item if the player is in range and presses the interact button
    /// </summary>
    public void TryPickUp()
    {
        if (_isInRange && _playerState.CurrentActionState == PlayerActionState.Gathering)
        {
            Player.Instance.AddItem(this);  
            Debug.Log(itemName + " picked up!");
            Destroy(gameObject); 
        }
    }
}
