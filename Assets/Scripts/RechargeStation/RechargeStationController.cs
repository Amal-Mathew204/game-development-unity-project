using System;
using Scripts.Game;
using Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.RechargeStation
{
    public class RechargeStationController : MonoBehaviour
    {
        [SerializeField] private int _costPerCharge = 5;
        private bool _playerInTriggerBox = false;
        private bool _canRecharge = false;
        private int _rechargePercentage = 10;
        
        /// <summary>
        /// Method Sets the Recharge Percentage for each transaction made in the recharge station
        /// </summary>
        private void OnEnable()
        {
            if (GameSettings.Instance != null)
            {
                _rechargePercentage = GameSettings.Instance.GameDifficultySettings.RechargePercentage;
            }
        }

        /// <summary>
        /// When Player Enters Trigger Box Give Option for Player to Buy Charge if they have
        /// enough microschips
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                _playerInTriggerBox = true;
                if (PlayerManager.Instance.MicroChips >= _costPerCharge)
                {
                    _canRecharge = true;
                    GameScreen.Instance.ShowKeyPrompt($"Press F to Buy Charge (5 credits for {_rechargePercentage} percent)");
                }
            }
        }
        
        /// <summary>
        /// Reset Recharge Station (and class Variables once player leads)
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            _playerInTriggerBox = false;
            _canRecharge = false;
            GameScreen.Instance.HideKeyPrompt();
        }
        
        /// <summary>
        /// Method handles the processing of the Player
        /// buying charge
        /// </summary>
        private void Update()
        {
            if (!_playerInTriggerBox)
            {
                return;
            }

            if (!_canRecharge)
            {
                return;
            }

            if (PlayerManager.Instance.getTaskAccepted())
            {
                GameManager.Instance.SetBatteryLevelIncrease(_rechargePercentage);
                PlayerManager.Instance.MicroChips -= _costPerCharge;
                if (PlayerManager.Instance.MicroChips < _costPerCharge)
                {
                    _canRecharge = false;
                    GameScreen.Instance.HideKeyPrompt();
                }
            }
        }
    }
}