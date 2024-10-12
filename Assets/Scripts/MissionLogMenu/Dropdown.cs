using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropdown : MonoBehaviour
{
    // Variables holding various texts for the mission log
    public TextMeshProUGUI info;
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI completion;
    public TextMeshProUGUI header;

    // Boolean variables to track completion of each quest
    public bool slopeQuestComplete = false;
    public bool triggerBoxQuestComplete = false;

    private void Start()
    {
        SetHeaderVisibility(false);
        ResetCompletionStatus(); // Initialize completion status
    }

    /// <summary>
    /// Handles the display of information based on the selected dropdown value.
    /// Updates the UI elements to show relevant quest information
    /// and controls the visibility of headers based on the selected value.
    /// </summary>
    public void HandleDropdownData(int option)
    {
        if (option == 0)
        {
            info.text = "";
            SetHeaderVisibility(false);
            completion.text = "";
        }
        else if (option == 1)
        {
            info.text = "Explore the terrain and locate a short, smooth hill. Reach the top of the hill.";
            SetHeaderVisibility(true);
            UpdateCompletionStatus(slopeQuestComplete);
        }
        else if (option == 2)
        {
            info.text = "Explore the terrain and locate the trigger box. Pass under it and listen for the sound effect.";
            SetHeaderVisibility(true);
            UpdateCompletionStatus(triggerBoxQuestComplete);
        }
        else
        {
            info.text = "";
            completion.text = "";
        }
    }

    /// <summary>
    /// Resets the mission log to its deafult state
    /// and updates the UI to reflect this change.
    /// </summary>
    public void ResetDropdown()
    {
        dropdown.value = 0;
        HandleDropdownData(0);
        SetHeaderVisibility(false);
    }

    /// <summary>
    /// Controls visibility of header and completion status
    /// </summary>
    private void SetHeaderVisibility(bool isVisible)
    {
        if (header != null)
        {
            header.gameObject.SetActive(isVisible);
        }

        if (completion != null)
        {
            completion.gameObject.SetActive(isVisible);
        }
    }

    /// <summary>
    /// Update completion status text based on the quest state
    /// </summary>
    public void UpdateCompletionStatus(bool questComplete)
    {
        if (questComplete)
        {
            completion.text = "Complete";
        }
        else
        {
            completion.text = "Incomplete";
        }
    }

    /// <summary>
    /// Reset all quests
    /// </summary>
    public void ResetCompletionStatus()
    {
        slopeQuestComplete = false;
        triggerBoxQuestComplete = false;
    }

    /// <summary>
    /// Set the slope quest as complete
    /// </summary>
    public void CompleteSlopeQuest()
    {
        slopeQuestComplete = true;
    }

    /// <summary>
    /// Set the trigger box quest as complete
    /// </summary>
    public void CompleteTriggerBoxQuest()
    {
        triggerBoxQuestComplete = true;
    }
}
