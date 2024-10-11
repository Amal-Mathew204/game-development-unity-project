using UnityEngine;
using TMPro; // Import TextMeshPro namespace to work with TextMeshPro components

public class NPCTrigger : MonoBehaviour
{
    public GameObject bubbleText; // Reference to the 3D bubble text GameObject
    private TextMeshPro textComponent; // Reference to the TextMeshPro component on the bubbleText

    void Start()
    {
        // Ensure the text is hidden initially
        bubbleText.SetActive(false);

        // Get the TextMeshPro component attached to the bubbleText GameObject
        textComponent = bubbleText.GetComponent<TextMeshPro>();

        // Check if the TextMeshPro component is properly found
        if (textComponent == null)
        {
            Debug.LogError("TextMeshPro component not found on bubbleText GameObject.");
        }
    }

    // This function is called when another object (like the player) enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Activate the bubble text, making it visible
            bubbleText.SetActive(true);
        }
    }

    // This function is called when the object exits the trigger collider
    private void OnTriggerExit(Collider other)
    {
        // Check if the object that exited the trigger has the tag "Player"
        if (other.CompareTag("Player"))
        {
            // Deactivate the bubble text, hiding it again
            bubbleText.SetActive(false);
        }
    }

    // This method allows you to set or update the text displayed in the bubbleText GameObject
    public void SetBubbleText(string newText)
    {
        // Check if the TextMeshPro component was successfully initialized
        if (textComponent != null)
        {
            // Set the text of the TextMeshPro component to the provided string
            textComponent.text = newText;
        }
        else
        {
            // Log an error if the TextMeshPro component was not found
            Debug.LogError("TextMeshPro component not found on bubbleText GameObject.");
        }
    }
}