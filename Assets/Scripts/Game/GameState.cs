using System;
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
        /// Method sets the Microchips gameobject text component to the GameScreen Singleton
        /// </summary>
        public void OnEnable()
        {
            GameObject microChipTextField = this.transform.Find("Microchips").gameObject;
            if (microChipTextField != null)
            {
                GameScreen.Instance.SetMicrochipsTextComponent(microChipTextField.GetComponent<TextMeshProUGUI>());
            }
        }

        /// <summary>
        /// Method will set Canvas Game Object in the GameStateCanvas field inside GameManager
        /// Method will also store the total /elapsed GameTimeFrom the Game Manager
        /// </summary>
        private void Start()
        {
            GameManager.Instance.GameStateCanvas = this.gameObject;
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
        
        /// <summary>
        /// This increases the battery level by a specific battery level value
        /// </summary>
        public void SetBatteryLevelIncrease(float batteryLevelIncreaseValue)
        {
            if(batteryLevelIncreaseValue <= 0 &&  batteryLevelIncreaseValue > 1)
            {
                Debug.LogError("Invalid Battery Level Increase");
                return;
            }
            float gameTimeToReduce = (batteryLevelIncreaseValue/100) * _totalGameTime;
            _gameElapsedTime -= gameTimeToReduce;
            //make sure no above 100% percentage vvalues
            if (_gameElapsedTime < 0)
            {
                _gameElapsedTime = 0;
            }
        }


    }
}