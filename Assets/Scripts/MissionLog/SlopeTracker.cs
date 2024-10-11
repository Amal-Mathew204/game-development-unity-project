using System.Collections;
using UnityEngine;

public class SlopeTracker : MonoBehaviour
{
    private bool hasGoneUp = false;  // Track if the player has gone up the slope
    private bool hasGoneDown = false; // Track if the player has gone down the slope
    private Dropdown dropdownScript;   // Reference to the Dropdown script

    private void Start()
    {
        // Find the Dropdown script in the scene
        dropdownScript = FindObjectOfType<Dropdown>();

        if (dropdownScript == null)
        {
            Debug.LogError("Dropdown script not found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Get the player's initial height
            float initialHeight = other.transform.position.y;

            // Start checking slope movement
            StartCoroutine(CheckSlopeMovement(other, initialHeight));
        }
    }

    private IEnumerator CheckSlopeMovement(Collider player, float initialHeight)
    {
        while (true)
        {
            float currentHeight = player.transform.position.y;

            // Check if the player has gone up the slope
            if (currentHeight > initialHeight && !hasGoneUp)
            {
                hasGoneUp = true; // Mark as gone up
                Debug.Log("Player has gone up the slope.");
            }

            // Check if the player has come down the slope
            if (currentHeight < initialHeight && hasGoneUp && !hasGoneDown)
            {
                hasGoneDown = true; // Mark as gone down
                Debug.Log("Player has come down the slope.");

                // Mark the quest as complete
                dropdownScript.CompleteSlopeQuest();
                dropdownScript.UpdateCompletionStatus(dropdownScript.slopeQuestComplete);
                break; // Exit the loop once complete
            }

            // Add a small delay to prevent performance issues
            yield return new WaitForSeconds(0.1f);
        }
    }
}
