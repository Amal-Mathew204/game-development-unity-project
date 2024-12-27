using System.Collections;
using System.Collections.Generic;
using Scripts.Quests;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerQuestPointer : MonoBehaviour
    {
        private Transform _target; //target location
        [SerializeField] private float _rotationSpeed = 3.0f; //rotation speed of arrow
        public CollectMission ActiveCollectMission { get; private set; }

        /// <summary>
		/// This method deactivates the Quest Pointer Arrow GameObject 
		/// </summary>
        private void Start()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Sets arrow quest pointer game object as active
        /// </summary>
        public void ActivateArrow(CollectMission collectMission)
        {
            ActiveCollectMission = collectMission;
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets arrow quest pointer game object as inactive
        /// </summary>
        public void DeactivateArrow()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// Rotates the arrow to face the target position using spherical interpolation.
        /// It adjusts the rotation at the rate determined by the rotation speed and frame time.
        /// </summary>
        private void Update()
        {
            if (ActiveCollectMission != null)
            {
                _target = ActiveCollectMission.GetClosestItemTransform();
            }
            if(_target == null)
            {
                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_target.position - transform.position),
            _rotationSpeed * Time.deltaTime);
        }
    }
}

