using UnityEngine;
using System.Collections;
using GameManager = Scripts.Game.GameManager;
using UnityEngine.InputSystem;
using Mission = Scripts.Quests.Mission;

public class PanicTrigger : MonoBehaviour
{
    public GameObject panicPanel; // Assign this in the inspector with your UI panel
    private bool _panicActive = false; // To track the state of the panic panel
    private PlayerInput _playerInput;

    void Start()
    {
        _playerInput = FindObjectOfType<PlayerInput>(); // Ensure this finds the correct PlayerInput component
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
}
