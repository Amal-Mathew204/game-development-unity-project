using UnityEngine;
using System.Collections;
using GameManager = Scripts.Game.GameManager;
using UnityEngine.InputSystem;
using Mission = Scripts.Quests.Mission;
using UnityEngine.UI;

public class PanicTrigger : MonoBehaviour
{
    public GameObject panicPanel; // Assign this in the inspector with your UI panel
    private bool _panicActive = false; // To track the state of the panic panel
    private PlayerInput _playerInput;
    private Image panelImage; // The Image component of the panel
    private bool _isFlickering = false;

    [System.Obsolete]
    void Start()
    {
        _playerInput = FindObjectOfType<PlayerInput>(); // Ensure this finds the correct PlayerInput component
        panelImage = panicPanel.GetComponent<Image>();
        if (panelImage == null)
        {
            Debug.LogError("No Image component found on the panic panel!");
        }
    }

    void OnEnable()
    {
        Mission.OnMissionStatusUpdated += CheckMissionCompletion;
    }

    void OnDisable()
    {
        Mission.OnMissionStatusUpdated -= CheckMissionCompletion;
    }

    private void CheckMissionCompletion()
    {
        Mission specificMission = GameManager.Instance.MissionList.Find(m => m.MissionTitle == "Clean Up");
        if (specificMission != null && specificMission.IsMissionCompleted())
        {
            StartCoroutine(TriggerPanic());
        }
    }

    private IEnumerator TriggerPanic()
    {
        yield return new WaitForSeconds(3); // Delay before triggering the panic
        TogglePanic(true);
        StartFlickering();
    }

    private void TogglePanic(bool isActive)
    {
        panicPanel.SetActive(isActive);
        _panicActive = isActive;

        // Lock or unlock the game based on the panic state
        if (isActive)
        {
            Time.timeScale = 0; // Pause the game
            _playerInput.SwitchCurrentActionMap("UI"); // Switch to UI input mode
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
            Cursor.visible = true; // Show the cursor
        }
        else
        {
            Time.timeScale = 1; // Resume the game
            _playerInput.SwitchCurrentActionMap("Player"); // Switch back to player control
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            Cursor.visible = false; // Hide the cursor
        }
    }

    // Optional: Method to disable the panic panel and reset states after some time
    private IEnumerator CalmDownAfterPanic()
    {
        yield return new WaitForSeconds(10); // Adjust recovery time
        TogglePanic(false);
    }

    public void StartFlickering()
    {
        if (!_isFlickering)
        {
            _isFlickering = true;
            StartCoroutine(FlickerEffect());
        }
    }

    public void StopFlickering()
    {
        _isFlickering = false;
    }

    private IEnumerator FlickerEffect()
    {
        while (_isFlickering)
        {
            float minAlpha = Random.Range(0.2f, 0.5f); // Random minimum alpha value
            float maxAlpha = Random.Range(0.6f, 1.0f); // Random maximum alpha value
            float duration = Random.Range(0.05f, 0.5f); // Random duration for one cycle of alpha changes

            // Gradually increase the alpha to maxAlpha
            float startAlpha = panelImage.color.a;
            for (float t = 0; t <= 1; t += Time.unscaledDeltaTime / duration)
            {
                float alpha = Mathf.Lerp(startAlpha, maxAlpha, t);
                panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha);
                yield return null;
            }

            // Random pause at max alpha
            yield return new WaitForSecondsRealtime(Random.Range(0.05f, 0.2f));

            // Gradually decrease the alpha back to minAlpha
            startAlpha = panelImage.color.a;
            for (float t = 0; t <= 1; t += Time.unscaledDeltaTime / duration)
            {
                float alpha = Mathf.Lerp(startAlpha, minAlpha, t);
                panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, alpha);
                yield return null;
            }

            // Random pause at min alpha
            yield return new WaitForSecondsRealtime(Random.Range(0.05f, 0.2f));
        }
    }
}
