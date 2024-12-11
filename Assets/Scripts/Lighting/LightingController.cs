using System;
using Scripts.Game;
using UnityEngine;

namespace Scripts.Lighting
{
    public class LightingController : MonoBehaviour
    {
        #region Class Variables
        [SerializeField] private float _timeMultiplier;
        [SerializeField] private float _startTimeHour;
        [SerializeField] private Light _sunLight;
        [SerializeField] private float _sunriseHour;
        [SerializeField] private float _sunsetHour;
        
        private DateTime _currentTime;
        private TimeSpan _sunriseTime;
        private TimeSpan _sunsetTime;
        
        [SerializeField] private Color _dayAmbientLight;
        [SerializeField] private Color _nightAmbientLight;
        [SerializeField] private AnimationCurve _ambientLightCurve;
        
        [SerializeField] private float _maxSunLightIntensity;
        [SerializeField] private Light _moonLight;
        [SerializeField] private float _maxMoonLightIntensity;
        #endregion

        /// <summary>
        /// Method sets Initial Time for the Game, sunrise and sunset time
        /// </summary>
        private void Start()
        {
            _currentTime = DateTime.Now.Date + TimeSpan.FromHours(_startTimeHour);
            _sunriseTime = TimeSpan.FromHours(_sunriseHour);
            _sunsetTime = TimeSpan.FromHours(_sunsetHour);
        }
        /// <summary>
        /// Method changes the time of day and applies the changes to the lighting game object
        /// </summary>
        private void Update()
        {
            //update time in game
            UpdateTime();
            RotateSun();
            UpdateLightRendering();
        }

        /// <summary>
        /// Method updates the time in game
        /// </summary>
        private void UpdateTime()
        {
            _currentTime = _currentTime.AddSeconds(Time.deltaTime * _timeMultiplier);
            GameScreen.Instance.UpdateTimeValue(_currentTime.ToString("HH:mm"));
        }

        /// <summary>
        /// Method rotates the sun (lighting) game object dependent on the time in game
        /// </summary>
        private void RotateSun()
        {
            float sunRotation;
            //day time
            if (_currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime)
            {
                TimeSpan sunriseToSunsetTime = GetTimeDifference(_sunriseTime, _sunsetTime);
                TimeSpan timeFromSunrise = GetTimeDifference(_sunriseTime, _currentTime.TimeOfDay);
                
                double percentageTimeGone = timeFromSunrise.TotalMinutes / sunriseToSunsetTime.TotalMinutes;
                sunRotation = Mathf.Lerp(0, 180, (float)percentageTimeGone);
            }
            //night time
            else
            {
                TimeSpan sunsetToSunriseTime = GetTimeDifference(_sunsetTime, _sunriseTime);
                TimeSpan timeFromSunset = GetTimeDifference(_sunsetTime, _currentTime.TimeOfDay);
                
                double percentageTimeGone = timeFromSunset.TotalMinutes / sunsetToSunriseTime.TotalMinutes;
                sunRotation = Mathf.Lerp(180, 360, (float)percentageTimeGone);
            }
            _sunLight.transform.rotation = Quaternion.AngleAxis(sunRotation, Vector3.right);
        }
        
        /// <summary>
        /// Method returns the time difference between two time spans
        /// </summary>
        private TimeSpan GetTimeDifference(TimeSpan fromTime, TimeSpan toTime)
        {
            TimeSpan difference = toTime - fromTime;
            //Times go over different days
            if (difference.TotalSeconds < 0)
            {
                difference += TimeSpan.FromHours(24);
            }
            return difference;
        }
        
        
        /// <summary>
        /// Adjust light properties in game
        /// Intensity of the sun and moon light adjusted
        /// ambient light colour value adjusted
        /// </summary>
        private void UpdateLightRendering()
        {
            //Get the direction of the sun (-1: up, 0: horizontal, 1: down)
            float dotProduct = Vector3.Dot(_sunLight.transform.forward, Vector3.down);
            //non-linear transitions of light intensity
            _sunLight.intensity = Mathf.Lerp(0, _maxSunLightIntensity,_ambientLightCurve.Evaluate(dotProduct));
            _moonLight.intensity = Mathf.Lerp(_maxMoonLightIntensity, 0, _ambientLightCurve.Evaluate(dotProduct));
            
            RenderSettings.ambientLight = Color.Lerp(_nightAmbientLight, _dayAmbientLight, _ambientLightCurve.Evaluate(dotProduct));
        }
    }
}