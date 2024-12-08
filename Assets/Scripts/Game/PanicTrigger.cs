using UnityEngine;
using System.Collections;
using Mission = Scripts.Quests.Mission;
using GameManager = Scripts.Game.GameManager;

public class PanicTrigger : MonoBehaviour
{
    public GameObject panicPanel; // Assign this in the inspector with your UI panel

    void OnEnable()
    {
        // Subscribe to the mission status update event
        Mission.OnMissionStatusUpdated += CheckMissionCompletion;
    }

    void OnDisable()
    {
        // Unsubscribe from the mission status update event
        Mission.OnMissionStatusUpdated -= CheckMissionCompletion;
    }

    private void CheckMissionCompletion()
    {
        // Retrieve the specific mission and check its completion status
        Mission specificMission = GameManager.Instance.MissionList.Find(m => m.MissionTitle == "Clean Up");
        if (specificMission != null && specificMission.IsMissionCompleted())
        {
            // Start the panic sequence after a delay
            StartCoroutine(TriggerPanic());
        }
    }

    private IEnumerator TriggerPanic()
    {
        // Wait for a few seconds before showing the panic panel
        yield return new WaitForSeconds(3); // Adjust the delay as necessary
        panicPanel.SetActive(true);

        // Additional actions during the panic, e.g., audio, effects, etc.
        // Optional: StartCoroutine(CalmDownAfterPanic()); if you want to auto-recover from panic
    }

    // Optional: Method to disable the panic panel and reset states after some time
    private IEnumerator CalmDownAfterPanic()
    {
        yield return new WaitForSeconds(10); // Adjust recovery time
        panicPanel.SetActive(false);
        // Reset other panic-related effects here
    }
}
