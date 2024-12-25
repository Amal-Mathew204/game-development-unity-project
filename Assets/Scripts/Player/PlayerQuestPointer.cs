using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Player
{
    public class PlayerQuestPointer : MonoBehaviour
    {
        [HideInInspector] public Transform target; //target location
        [SerializeField] private float _rotationSpeed = 3.0f; //rotation speed of arrow


        /// <summary>
		/// This method sets an initial target for the PlayerQuestPointer
		/// </summary>
        private void Start()
        {
            target = GameObject.Find("Water_box").transform;
            Debug.Log(target);
        }

        /// <summary>
        /// Rotates the arrow to face the target position using spherical interpolation.
        /// It adjusts the rotation at the rate determined by the rotation speed and frame time.
        /// </summary>
        private void Update()
        {
            if(target == null)
            {
                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position),
            _rotationSpeed * Time.deltaTime);
        }
    }
}

