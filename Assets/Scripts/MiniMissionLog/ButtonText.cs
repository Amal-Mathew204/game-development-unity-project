using UnityEngine;
using TMPro; 

public class DropdownButtonUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown; 
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private string defaultButtonText = "Select a Mission"; 

    void Start()
    {
        UpdateButtonWithPlaceholder();  
        dropdown.onValueChanged.AddListener(delegate { UpdateButtonText(); }); 
    }

    /// <summary>
    /// Updates the button text based on the selection of the dropdown
    /// </summary>
    private void UpdateButtonText()
    {
        if (dropdown.value >= 0 && dropdown.value < dropdown.options.Count)
        {
            buttonText.text = dropdown.options[dropdown.value].text; 
        }
        else
        {
            UpdateButtonWithPlaceholder(); 
        }
    }

    /// <summary>
    /// Sets the button text to the placeholder value
    /// </summary>
    private void UpdateButtonWithPlaceholder()
    {
        buttonText.text = defaultButtonText;
    }
}
