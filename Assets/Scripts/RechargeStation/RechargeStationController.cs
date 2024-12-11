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
                    GameScreen.Instance.ShowKeyPrompt("Press F to Buy Charge (5 credits for 10 percent)");
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
                GameManager.Instance.SetBatteryLevelIncrease(10);
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