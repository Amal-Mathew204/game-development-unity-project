using UnityEngine;
using TMPro;
using Mission = Scripts.Quests.Mission;
using CollectMission = Scripts.Quests.CollectMission;

public class MissionDetailsCanvas : MonoBehaviour
{
    public static MissionDetailsCanvas Instance { get; private set; }

    [SerializeField] private GameObject canvasObject; // The actual canvas GameObject
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI progressText; // Optional, for collect missions

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDetails(Mission mission)
    {
        Debug.Log($"Showing details for mission: {mission.MissionTitle}");

        if (mission == null)
        {
            Debug.LogError("Mission is null.");
            return;
        }

        titleText.text = mission.MissionTitle;
        descriptionText.text = mission.MissionInfo;

        if (mission is CollectMission collectMission)
        {
            progressText.gameObject.SetActive(true);
            progressText.text = collectMission.GetItemProgress();
            Debug.Log($"Collect mission progress: {collectMission.GetItemProgress()}");
        }
        else
        {
            progressText.gameObject.SetActive(false);
        }

        canvasObject.SetActive(true); // Show the canvas
        Debug.Log("Canvas should now be visible with updated details.");
    }


    public void HideCanvas()
    {
        canvasObject.SetActive(false); // Hide the canvas
    }
}
