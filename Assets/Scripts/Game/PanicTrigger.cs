using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Mission = Scripts.Quests.Mission;
using UnityEngine.UI;
using AudioManager = Scripts.Audio.AudioManager;
using PlayerManager = Scripts.Player.Player;
using TMPro;
using Cinemachine;

namespace Scripts.Game
{
    public class PanicTrigger : MonoBehaviour
    {
        #region Varaibles   
        public GameObject panicPanel;
        private Image _panelImage;
        private bool _panicActive = false;
        private bool _isFlickering = false;
        private static PlayerInput _playerInput;
        public AudioClip panicSoundClip;
        public AudioClip radioSoundClip;
        
        // Thought management
        public GameObject thoughtPrefab;
        public Transform thoughtContainer;
        private bool _isDisplayingThoughts = false;
        private int _currentThoughtIndex = 0;
        private GameObject _currentThought;
        
        // Timing configuration
        [SerializeField] private float thoughtDisplayDuration = 0.5f;
        [SerializeField] private float fadeInDuration = 0.2f;
        [SerializeField] private float fadeOutDuration = 0.2f;
        [SerializeField] private float initialPanicDelay = 1f;

        private string[] _thoughts = new string[]
        {
            "This is too much work...", "How am i supposed to to this by myself", "I just can't.", "I'm going to fail!",
            "What do I do?"
        };
        



        #endregion

        #region Initialisation
        /// <summary>
        /// Locates and assigns the PlayerInput component from the player GameObject tagged "Player"
        /// Retrieves the Image component from the assigned panicPanel
        /// </summary>
        void Start()
        {
            _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            _panelImage = panicPanel.GetComponent<Image>();

            
            if (_panelImage == null)
            {
                Debug.LogError("No Image component found on the panic panel!");
            }
            
            if (thoughtPrefab == null)
            {
                Debug.LogError("No thought prefab assigned!");
            }

            if (thoughtContainer == null)
            {
                // Create a container if none is assigned
                thoughtContainer = new GameObject("ThoughtContainer").transform;
                thoughtContainer.SetParent(panicPanel.transform);
                thoughtContainer.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Subscribes to the OnMissionStatusUpdated event when the GameObject is enabled
        /// </summary>
        void OnEnable()
        {
            Mission.OnMissionStatusUpdated += CheckMissionCompletion;
        }

        /// <summary>
        /// Unsubscribes from the OnMissionStatusUpdated event when the GameObject is disabled
        /// </summary>
        void OnDisable()
        {
            Mission.OnMissionStatusUpdated -= CheckMissionCompletion;
        }

        /// <summary>
        /// Checks if a specific mission, identified by title, has been completed
        /// Initiates panic sequence if the mission is found
        /// </summary>
        private void CheckMissionCompletion()
        {
            Mission specificMission = GameManager.Instance.MissionList.Find(m => m.MissionTitle == "Clean Up");
            if (specificMission != null && specificMission.IsMissionCompleted())
            {
                StartCoroutine(TriggerPanic());
            }
        }
        #endregion

        #region Panic Mode
        /// <summary>
        /// Delays the activation of panic mode, then triggers the panic state and starts the flickering effect
        /// With robot's thoughts and panic sound
        /// </summary>
        private IEnumerator TriggerPanic()
        {
            yield return new WaitForSeconds(3); // 3-second delay
            TogglePanic(true);
            PlayerManager.Instance.SwitchToFirstPerson();
            AudioManager.Instance.PlaySFXLoop(panicSoundClip);
            StartFlickering();
            StartDisplayingThoughts();
            
            //yield return new WaitForSeconds(10); // Duration of active panic before calming down starts
            StartCoroutine(CalmDownAfterPanic());
        }
        

        /// <summary>
        /// Pauses the game, switches input controls to UI mode, and makes the cursor visible and unlocked,
        /// When deactivated, it resumes normal game operations, reverts input controls to player mode,
        /// and hides and locks the cursor 
        /// </summary>
        private void TogglePanic(bool isActive)
        {
            panicPanel.SetActive(isActive);
            _panicActive = isActive;

            // Lock or unlock the game based on the panic state
            if (isActive)
            {
                // Time.timeScale = 0; // Pause the game
                PlayerManager.Instance.DisablePlayerMovement();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }
            else
            {
                // Time.timeScale = 1; // Resume the game
                PlayerManager.Instance.EnablePlayerMovement();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        /// <summary>
        /// Waits for a set recovery time before deactivating the panic state
        /// </summary>
        private IEnumerator CalmDownAfterPanic()
        {
            yield return new WaitForSeconds(7); // 7-second for recovery time
            AudioManager.Instance.StopSFXLoop();
            StopFlickering(); 
            AudioManager.Instance.PlaySFX(radioSoundClip); 
            _panelImage.color = new Color(0.678f, 0.847f, 0.902f, 0.3f);
            
            _isDisplayingThoughts = false;
            if (_currentThought != null)
            {
                StartCoroutine(FadeAndDestroyThought(_currentThought));
                _currentThought = null; // Clear the reference to the current thought
            }
            
            DisplaySpecificMessage("Reinforcements are on their way...");
        }
        #endregion

        #region Flickering Effect
        /// <summary>
        /// Initiates the flickering effect on the panic panel if it is not already active
        /// </summary>
        private void StartFlickering()
        {
            if (!_isFlickering)
            {
                _isFlickering = true;
                StartCoroutine(FlickerEffect());
            }
        }

        /// <summary>
        /// Stops the flickering effect on the panic panel by setting the flickering state to inactive
        /// </summary>
        public void StopFlickering()
        {
            _isFlickering = false;
        }

        /// <summary>
        /// Continuously adjusts the alpha transparency of the panic panel's image to create a flickering effect.
        /// </summary>
        private IEnumerator FlickerEffect()
        {
            while (_isFlickering)
            {
                float minAlpha = Random.Range(0.2f, 0.5f); // Random minimum alpha value
                float maxAlpha = Random.Range(0.6f, 1.0f); // Random maximum alpha value
                float duration = Random.Range(0.05f, 0.5f); // Random duration for one cycle of alpha changes

                // Gradually increase the alpha to maxAlpha
                float startAlpha = _panelImage.color.a;
                for (float t = 0; t <= 1; t += Time.unscaledDeltaTime / duration)
                {
                    float alpha = Mathf.Lerp(startAlpha, maxAlpha, t);
                    _panelImage.color = new Color(_panelImage.color.r, _panelImage.color.g, _panelImage.color.b, alpha);
                    yield return null;
                }

                // Random pause at max alpha
                yield return new WaitForSecondsRealtime(Random.Range(0.05f, 0.2f));

                // Gradually decrease the alpha back to minAlpha
                startAlpha = _panelImage.color.a;
                for (float t = 0; t <= 1; t += Time.unscaledDeltaTime / duration)
                {
                    float alpha = Mathf.Lerp(startAlpha, minAlpha, t);
                    _panelImage.color = new Color(_panelImage.color.r, _panelImage.color.g, _panelImage.color.b, alpha);
                    yield return null;
                }

                // Random pause at min alpha
                yield return new WaitForSecondsRealtime(Random.Range(0.05f, 0.2f));
            }
        }
        #endregion

        #region Thought Management
        /// <summary>
        /// Initiates the display of thoughts if not already in progress.
        /// </summary>
        private void StartDisplayingThoughts()
        {
            if (!_isDisplayingThoughts)
            {
                _isDisplayingThoughts = true;
                _currentThoughtIndex = 0;
                StartCoroutine(DisplayThoughtsSequence());
            }
        }
       
        /// <summary>
        /// Manages the sequence of displaying thoughts in a loop as long as _isDisplayingThoughts is true.
        /// This coroutine handles the instantiation and cycling of thought texts using a prefab.
        /// </summary>
        private IEnumerator DisplayThoughtsSequence()
        {
            while (_isDisplayingThoughts)
            {
                // Create new thought
                GameObject newThought = Instantiate(thoughtPrefab, thoughtContainer); 
                newThought.transform.localPosition = Vector3.zero; 
                newThought.transform.localPosition = Vector3.zero; 
                
                // Retrieve and configure the TextMeshPro component of the new thought object for display.
                TextMeshProUGUI tmpText = newThought.GetComponent<TextMeshProUGUI>();
                if (tmpText != null)
                {
                    tmpText.text = _thoughts[_currentThoughtIndex];
                    tmpText.alpha = 0f;
                    tmpText.alignment = TextAlignmentOptions.Center;
                }

                // Fade out existing thought
                if (_currentThought != null)
                {
                    StartCoroutine(FadeAndDestroyThought(_currentThought));
                }

                _currentThought = newThought;

                // Fade in new thought
                yield return StartCoroutine(FadeThought(_currentThought, true));
                
                yield return new WaitForSecondsRealtime(thoughtDisplayDuration);
                
                // Move onto the next thought
                _currentThoughtIndex = (_currentThoughtIndex + 1) % _thoughts.Length;
            }
        }
        
        /// <summary>
        /// Displays a specific thought message.
        /// </summary>
        /// <summary>
        /// Displays a specific thought message with a typewriter effect.
        /// </summary>
        private void DisplaySpecificMessage(string message)
        {
            GameObject newThought = Instantiate(thoughtPrefab, thoughtContainer); 
            newThought.transform.localPosition = Vector3.zero;
            TextMeshProUGUI tmpText = newThought.GetComponent<TextMeshProUGUI>();
            if (tmpText != null)
            {
                StartCoroutine(TypewriterEffect(tmpText, message, 0.05f)); // Adjust delay to suit your needs
            }
            // Optionally fade out and destroy the old thought if needed
            if (_currentThought != null)
            {
                StartCoroutine(FadeAndDestroyThought(_currentThought));
            }
            _currentThought = newThought; // Update the current thought reference
        }


        /// <summary>
        /// Coroutine that simulates a typewriter effect for displaying text.
        /// </summary>
        private IEnumerator TypewriterEffect(TextMeshProUGUI textComponent, string fullText, float delay)
        {
            textComponent.text = ""; // Start with an empty string
            foreach (char c in fullText)
            {
                textComponent.text += c; // Add each character one at a time
                yield return new WaitForSeconds(delay); // Wait before adding the next character
            }
        }

        /// <summary>
        /// // Coroutine to fade a thought in or out.
        /// </summary>
        private IEnumerator FadeThought(GameObject thought, bool fadeIn)
        {
            TextMeshProUGUI tmpText = thought.GetComponent<TextMeshProUGUI>();
            if (tmpText == null) yield break;

            float duration = fadeIn ? fadeInDuration : fadeOutDuration;
            float startAlpha = fadeIn ? 0f : 1f;
            float endAlpha = fadeIn ? 1f : 0f;

            // Loop over the duration of the fade
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                // Calculate the current alpha value using linear interpolation
                float alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
                // Apply the calculated alpha to the text component
                tmpText.alpha = alpha;
                yield return null;
            }
            tmpText.alpha = endAlpha;
        }

        /// <summary>
        /// Coroutine that fades out a thought GameObject and then destroys it.
        /// </summary>
        private IEnumerator FadeAndDestroyThought(GameObject thought)
        {
            yield return StartCoroutine(FadeThought(thought, false));
            Destroy(thought);
        }
        #endregion
     
    }
}