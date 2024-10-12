using System;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // Singleton instance
    public static QuestManager Instance { get; private set; }

    // Event to notify when a quest is completed
    public static event Action<string> OnQuestCompleted;

    private void Awake()
    {
        // Ensure that there's only one instance of QuestManager
        if (Instance == null)
        {
            Instance = this; // Set the singleton instance
            DontDestroyOnLoad(gameObject); // Optional: Persist through scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    /// <summary>
    /// Completes a quest and raises the OnQuestCompleted event.
    /// </summary>
    /// <param name="questName">The name of the quest to complete.</param>
    public void CompleteQuest(string questName)
    {
        // Logic to mark the quest as complete (you can implement your own logic here)
        Debug.Log($"{questName} completed!");

        // Raise the quest completed event
        OnQuestCompleted?.Invoke(questName);
    }
}
