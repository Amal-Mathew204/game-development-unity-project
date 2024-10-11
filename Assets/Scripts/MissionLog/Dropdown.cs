using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dropdown : MonoBehaviour
{
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


    //Displays information relevant to the dropdown option
    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            info.text = "";
            SetHeaderVisibility(false);
            completion.text = "";
        }
        else if (val == 1)
        {
            info.text = "Explore the terrain and locate the slope. Go up and down the slope.";
            SetHeaderVisibility(true);
            UpdateCompletionStatus(slopeQuestComplete);
        }
        else if (val == 2)
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

    //Reset the dropdown mennu 
    public void ResetDropdown()
    {
        dropdown.value = 0;
        HandleInputData(0);
        SetHeaderVisibility(false);
    }


    //Controls visibility of header and completion status
    private void SetHeaderVisibility(bool isVisible)
    {
        if (header == null)
        {
            Debug.LogError("Header is not assigned in the Inspector.");
        }
        else
        {
            header.gameObject.SetActive(isVisible);
        }

        if (completion == null)
        {
            Debug.LogError("Completion text is not assigned in the Inspector.");
        }
        else
        {
            completion.gameObject.SetActive(isVisible);
        }
    }


    // Method to update completion status text based on the quest state
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

    // Method to reset all quests, if needed
    public void ResetCompletionStatus()
    {
        slopeQuestComplete = false;
        triggerBoxQuestComplete = false;
    }

    // Example method to set a quest as complete (call these when conditions are met in the game)
    public void CompleteSlopeQuest()
    {
        slopeQuestComplete = true;
    }

    public void CompleteTriggerBoxQuest()
    {
        triggerBoxQuestComplete = true;
    }
}
