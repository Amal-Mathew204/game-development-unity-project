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
        private TextMeshPro textComponent;
        private List<string> _textSections;
        private int _currentTextIndex = 0;

        public float typeWritingSpeed;


        /// <summary>

        /// </summary>
        void Start()
        {
            if (bubbleText == null)
            {
                Debug.LogError("BubbleText prefab is not assigned");
            }

            bubbleText.SetActive(true);
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

            // Populate text sections for cycling
            _textSections = new List<string>
            {
                "Hello there! This is the first section of the dialogue.",
                "Here's some more information for you to read.",
                "And here is the final part of the conversation."
            };
            DisplayCurrentText();
        }

        /// <summary>
        /// 
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
                Debug.Log("End of dialogue reached");
                return false; // No more text
            }
        }



        private void DisplayCurrentText()
        {
            if (textComponent != null)
            {
                textComponent.text = _textSections[_currentTextIndex];
            }
        }

        //    /// <summary>
        //    /// This is an iterative method which when the bubbleText GameObject is active, the method adds a single character to the text component
        //    /// every time specified interval.
        //    /// </summary>
        //    IEnumerator TypeText()
        //    {
        //        foreach (char character in message)
        //        {
        //            textComponent.text += character;

        //            if (bubbleText.activeSelf)
        //            {
        //                yield return new WaitForSeconds(typeWritingSpeed);
        //            }
        //            else
        //            {
        //                textComponent.text = "";
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}