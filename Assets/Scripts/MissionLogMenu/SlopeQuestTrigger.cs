using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeQuestTrigger : MonoBehaviour
{
    [SerializeField] private Dropdown _dropdown;
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
            Mission mission = GameManager.Instance.MissionList.Find(mission => mission.MissionTitle == "Slippery Slope");
            if (mission != null && !mission.IsMissionCompleted()) // Prevent repeating the completion
            {
                mission.SetMissionCompleted();
                _dropdown.UpdateCompletionStatus(mission.IsMissionCompleted());
            }
        }
    }
}
