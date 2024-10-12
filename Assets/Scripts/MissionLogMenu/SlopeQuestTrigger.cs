using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeQuestTrigger : MonoBehaviour
{
    public Dropdown dropdown; // Reference to the Dropdown script 
    public float hillTopYPosition;
    public float threshold;
    private Transform _playerTransform; 

    private void Start()
    {
        // Find the player object in the scene 
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// Check if player has reached the top of the hill and updates the quest status
    /// </summary>
    private void Update()
    {
        if (_playerTransform.position.y >= hillTopYPosition - threshold)
        {
            if (!dropdown.slopeQuestComplete) // Prevent repeating the completion
            {
                // Complete the slope quest
                dropdown.CompleteSlopeQuest();
                dropdown.UpdateCompletionStatus(dropdown.slopeQuestComplete);
            }
        }
    }
}
