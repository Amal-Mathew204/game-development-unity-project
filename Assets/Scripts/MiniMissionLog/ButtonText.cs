using UnityEngine;
using TMPro; 

public class DropdownButtonUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown; 
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private string _defaultButtonText = "Select a Mission"; 

    void Start()
    {
        UpdateButtonWithPlaceholder();  
        _dropdown.onValueChanged.AddListener(delegate { UpdateButtonText(); }); 
    }

    /// <summary>
    /// Updates the button text based on the selection of the dropdown
    /// </summary>
    private void UpdateButtonText()
    {
        if (_dropdown.value >= 0 && _dropdown.value < _dropdown.options.Count)
        {
            _buttonText.text = _dropdown.options[_dropdown.value].text; 
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
        _buttonText.text = _defaultButtonText;
    }
}
