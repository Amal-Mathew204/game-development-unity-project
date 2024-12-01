using UnityEngine;
using System.Collections;
using TMPro;

namespace Scripts.Game
{
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
            GameManager.Instance.SetGameState(this);
        }

        /// <summary>
        /// Update Method Adjusts the Battery Level of the Player on the condition that
        /// Battery is not equal to zero and the Player has not won.
        /// </summary>
        private void Update()
        {
            if (_batteryLevel > 0 && GameManager.Instance.HasPlayerWonGame == false)
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
        /// Method reduces the battery Percentage level
        /// </summary>
        private void AdjustBatteryLevel()
        {
            _gameElapsedTime += Time.deltaTime;
            SetBatteryLevel(Mathf.RoundToInt(((_totalGameTime - _gameElapsedTime) / _totalGameTime) * 100));
        }

        /// <summary>
        /// Method sets the battery level by setting the class field and TMP Text Field
        /// </summary>
        private void SetBatteryLevel(int level)
        {
            _batteryLevel = level;
            _batteryLevelTextField.text = $"Battery Level: {_batteryLevel}%";
        }

        /// <summary>
        /// This reduces the battery level of the player by a percentage
        /// </summary>
        
        public void SetBatteryLevelReduction(float percentageReduction)
        {
            if(percentageReduction <= 0 &&  percentageReduction > 1)
            {
                Debug.LogError("Invalid Percentage Reduction");
                return;
            }
            float gameTimeLeft = (_batteryLevel / 100f) * _totalGameTime;
            _gameElapsedTime += gameTimeLeft * percentageReduction;
        }


    }
}