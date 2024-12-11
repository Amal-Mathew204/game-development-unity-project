using System;
using Scripts.Game;
using UnityEngine;

namespace Scripts.Lighting
{
    public class LightingController : MonoBehaviour
    {
        #region Class Variables
        [SerializeField] private float _timeMultiplier = 2f;
        [SerializeField] private float _startTimeHour;
        private DateTime _currentTime;
        #endregion

        /// <summary>
        /// Set Initial Time for the Game
        /// </summary>
        private void Start()
        {
            _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startTimeHour);
        }
        /// <summary>
        /// Method changes the time of day and applies the changes to the lighting game object
        /// </summary>
        private void Update()
        {
            //update time in game
            UpdateTime();
        }

        /// <summary>
        /// Method updates the time in game
        /// </summary>
        private void UpdateTime()
        {
            _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
            GameScreen.Instance.UpdateTimeValue(_currentTime.ToString("HH:mm"));
        }
    }
}