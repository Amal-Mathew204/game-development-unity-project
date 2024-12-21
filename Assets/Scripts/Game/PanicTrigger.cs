using UnityEngine;
using System.Collections;
using Scripts.Audio;
using UnityEngine.InputSystem;
using Mission = Scripts.Quests.Mission;
using UnityEngine.UI;
using AudioManager = Scripts.Audio.AudioManager;

namespace Scripts.Game
{
    public class PanicTrigger : MonoBehaviour
    {
        public GameObject _panicPanel;
        private Image _panelImage;
        private bool _panicActive = false;
        private bool _isFlickering = false;
        public static PlayerInput _playerInput;
        public AudioClip panicSoundClip; 
        

        /// <summary>
        /// Locates and assigns the PlayerInput component from the player GameObject tagged "Player"
        /// Retrieves the Image component from the assigned panicPanel
        /// </summary>
        void Start()
        {
            _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
            _panelImage = _panicPanel.GetComponent<Image>();

            if (_panelImage == null)
            {
                Debug.LogError("No Image component found on the panic panel!");
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

        /// <summary>
        /// Delays the activation of panic mode, then triggers the panic state and starts the flickering effect
        /// </summary>
        private IEnumerator TriggerPanic()
        {
            yield return new WaitForSeconds(3); // 3 second delay
            TogglePanic(true);
            AudioManager.Instance.PlaySFXLoop(panicSoundClip);
            StartFlickering();
        }

        /// <summary>
        /// Pauses the game, switches input controls to UI mode, and makes the cursor visible and unlocked,
        /// When deactivated, it resumes normal game operations, reverts input controls to player mode,
        /// and hides and locks the cursor 
        /// </summary>
        private void TogglePanic(bool isActive)
        {
            _panicPanel.SetActive(isActive);
            _panicActive = isActive;

            // Lock or unlock the game based on the panic state
            if (isActive)
            {
                Time.timeScale = 0; // Pause the game
                _playerInput.SwitchCurrentActionMap("UI"); 
                Cursor.lockState = CursorLockMode.None; 
                Cursor.visible = true; 
            }
            else
            {
                Time.timeScale = 1; // Resume the game
                _playerInput.SwitchCurrentActionMap("Player"); 
                Cursor.lockState = CursorLockMode.Locked; 
                Cursor.visible = false; 
            }
        }

        /// <summary>
        /// Waits for a set recovery time before deactivating the panic state
        /// </summary>
        private IEnumerator CalmDownAfterPanic()
        {
            yield return new WaitForSeconds(10); // 10 seconds for recovery time
            TogglePanic(false);
            AudioManager.Instance.StopSFXLoop();
        }

        /// <summary>
        /// Initiates the flickering effect on the panic panel if it is not already active
        /// </summary>
        public void StartFlickering()
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
    }
}
