using UnityEngine;
using TMPro;
using System.Collections;
using Scripts.Game;

namespace Scripts.NPC
{
    public class NPCTrigger : MonoBehaviour
    {
        public GameObject bubbleText;        
        private TextMeshPro textComponent;
        public float typeWritingSpeed;
        public string message;


        /// <summary>
        /// The Start Method Intially sets the bubble text SetActive to False 
        /// and gets/sets the typing speed value from Game Settings
        /// The Method contains checks to see if the textComponent inside bubbletext GameObject
        /// </summary>
        protected virtual void Start()
        {
            bubbleText.SetActive(false);
            textComponent = bubbleText.GetComponent<TextMeshPro>();

            if (textComponent == null)
            {
                Debug.LogError("TextMeshPro component not found on bubbleText GameObject.");
            }
            //set NPC SubtitleSpeed
            if (GameSettings.Instance != null)
            {
                typeWritingSpeed = GameSettings.Instance.NPCSubtitleSpeed;
            }
            else
            {
                Debug.LogError("GameSettings is null");
            }
        }
        /// <summary>
        /// This function is called when another object (like the player) enters the trigger collider
        /// Check if the object that entered the trigger has the tag "Player"
        /// Activate the bubble text, making it visible
        /// </summary>
        protected virtual void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(true);
                StartCoroutine(TypeText());
            }
        }

        /// <summary>
        /// This function is called when the object exits the trigger collider
        /// Check if the object that exited the trigger has the tag "Player"
        /// Deactivate the bubble text, hiding it again
        /// </summary>
        protected virtual void OnTriggerExit(Collider other)
        {

            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(false);
            }
            textComponent.text = "";
        }

        /// <summary>
        /// This method allows you to set or update the text displayed in the bubbleText GameObject
        /// Check if the TextMeshPro component was successfully initialized
        /// Set the text of the TextMeshPro component to the provided string
        /// Log an error if the TextMeshPro component was not found
        /// </summary>

        public void SetBubbleText(string newText)
        {

            if (textComponent != null)
            {
                message = newText;
            }
            else
            {
                Debug.LogError("TextMeshPro component not found on bubbleText GameObject.");
            }
        }

        /// <summary>
        /// This is an iterative method which when the bubbleText GameObject is active, the method adds a single character to the text component
        /// every time specified interval.
        /// </summary>
        protected IEnumerator TypeText()
        {
            foreach (char character in message)
            {
                textComponent.text += character;

                if (bubbleText.activeSelf)
                {
                    yield return new WaitForSeconds(typeWritingSpeed);
                }
                else
                {
                    textComponent.text = "";
                    break;
                }
            }
        }
    }
}