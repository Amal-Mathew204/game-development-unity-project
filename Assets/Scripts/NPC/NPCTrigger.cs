using UnityEngine;
using TMPro;
using System.Collections;
using Scripts.Game;
using System.Collections.Generic;

namespace Scripts.MissonLogMenu
{
    public class NPCTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject bubbleText;
        private TextMeshProUGUI _textMeshPro;
        private List<string> _textSections;
        private int _currentTextIndex = 0;

        
        private TextMeshPro textComponent;
        
        public float typeWritingSpeed;
        public string message;

        private void Awake()
        {
   
            if (bubbleText != null )
            {
                bubbleText.SetActive(true);
                _textMeshPro = bubbleText.GetComponent<TextMeshProUGUI>();
                if (_textMeshPro == null)
                {
                    Debug.LogError("TextMeshPro component not found in bubble text prefab");
                }

                // Populate text sections for cycling
                _textSections = new List<string>
                {
                    "Hello there! This is the first section of the dialogue.",
                    "Here's some more information for you to read.",
                    "And here is the final part of the conversation."
                };

                DisplayCurrentText();
            }
            else
            {
                Debug.LogError("BubbleText prefab is not assigned");
            }
        }
        public bool CycleBubbleText()
        {
            if (_textSections == null || _textSections.Count == 0)
            {
                Debug.LogWarning("No text sections available to cycle through");
                return false;
            }

            // Increment and check if at the end
            if (_currentTextIndex < _textSections.Count - 1)
            {
                _currentTextIndex++;
                DisplayCurrentText();
                return true; // More text remains
            }
            else
            {
                Debug.Log("End of dialogue reached");
                return false; // No more text
            }
        }

        private void DisplayCurrentText()
        {
            if (_textMeshPro != null)
            {
                _textMeshPro.text = _textSections[_currentTextIndex];
            }
        }
        /// <summary>
        /// Ensure the text is hidden initially
        /// Get the TextMeshPro component attached to the bubbleText GameObject
        /// Check if the TextMeshPro component is properly found
        /// </summary>
        void Start()
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
        private void OnTriggerEnter(Collider other)
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
        
        //private void OnTriggerExit(Collider other)
        //{

        //    if (other.CompareTag("Player"))
        //    {
        //        bubbleText.SetActive(false);
        //    }
        //    textComponent.text = "";
        //}

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
        IEnumerator TypeText()
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