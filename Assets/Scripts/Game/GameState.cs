using UnityEngine;
using System.Collections;
using TMPro;

public class GameState : MonoBehaviour
{
    private float _gameElapsedTime = 0f;
    private float _totalGameTime;
    [SerializeField] private TMP_Text _batteryLevelTextField;
    private int _batteryLevel = 100;

    /// <summary>
    /// Method will set Canvas Game Object in the GameStateCanvas field inside GameManager
    /// Method will also store the total /elapsed GameTimeFrom the Game Manager
    /// </summary>
    private void Start()
    {
        GameManager.Instance.GameStateCanvas = this.gameObject.transform.Find("Canvas").gameObject;
        _totalGameTime = GameManager.Instance.GameTime;
        _gameElapsedTime = GameManager.Instance.GameTimeElapsed;

    }

    private void Update()
    {
        if(_batteryLevel > 0)
        {
            AdjustBatteryLevel();
            if (_batteryLevel <= 0)
            {
                GameManager.Instance.SetPlayerHasLost();
                SetBatteryLevel(0);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void AdjustBatteryLevel()
    {
        _gameElapsedTime += Time.deltaTime;
        SetBatteryLevel(Mathf.RoundToInt(((_totalGameTime - _gameElapsedTime) / _totalGameTime) * 100));
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetBatteryLevel(int level)
    {
        _batteryLevel = level;
        _batteryLevelTextField.text = $"Battery Level: {_batteryLevel}%";
    }
}
