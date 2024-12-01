using UnityEngine;
using TMPro;
using System.Collections;
using Scripts.Game;
using System.Collections.Generic;

namespace Scripts.NPC
{
    public class StartNPC : MonoBehaviour
    {
        [SerializeField] private GameObject bubbleText;
        private TextMeshPro _textComponent;
        private List<string> _textSections;
        private int _currentTextIndex = 0;

        public float typeWritingSpeed;


        /// <summary>
        /// Initializes the StartNPC component. Sets up the text component, assigns typewriting speed, and initializes dialogue sections.
        /// </summary>
        void Start()
        {
            if (bubbleText == null)
            {
                Debug.LogError("BubbleText prefab is not assigned");
                return;
            }


            bubbleText.SetActive(true);
            _textComponent = bubbleText.GetComponent<TextMeshPro>();

            if (_textComponent == null)
            {
                Debug.LogError("TextMeshPro component not found on bubbleText GameObject.");
                return;
            }
            //set NPC SubtitleSpeed
            if (GameSettings.Instance != null)
            {
                typeWritingSpeed = GameSettings.Instance.NPCSubtitleSpeed;
            }
            else
            {
                Debug.LogError("GameSettings is null");
                return;
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

        // <summary>
        /// Cycles through the dialogue text sections. Moves to the next section if available and displays it. Returns false if at the end of the dialogue.
        /// True if more text remains, false if at the end of the dialogue
        /// </summary>
       
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
                
                return false; // No more text
            }
        }


        /// <summary>
        /// Displays the current section of text in the TextMeshPro component.
        /// </summary>
        private void DisplayCurrentText()
        {
            if (_textComponent != null)
            {
                _textComponent.text = _textSections[_currentTextIndex];
                GameManager.Instance.ChangeEnterTextFieldVisibility(true);
            }
            
        }
    }
}