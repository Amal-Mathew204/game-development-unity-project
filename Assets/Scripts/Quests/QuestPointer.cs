using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Quests
{
    public class QuestPointer : MonoBehaviour
    {
        public Transform target; //target location
        public float rotationSpeed; //rotation speed of arrow

        /// <summary>
        /// Rotates the arrow to face the target position using spherical interpolation.
        /// It adjusts the rotation at the rate determined by the rotation speed and frame time.
        /// </summary>
        private void Update()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position),
                rotationSpeed * Time.deltaTime);
        }
    }
}

