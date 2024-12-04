using UnityEngine;
using System.Collections;
using Scripts.Game;



namespace Scripts.GarbageDisposal
{
    public class GarbageDisposalController : MonoBehaviour
    {
        public bool isActive { get; set; } = false;
        /// <summary>
        /// The OnTrigger Method (when player enters the trigger box)
        /// sets the the GarbageDetonateButton to active
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && isActive)
            {
                ToggleGarbageDetonateButtonActiveState(true);
            }
        }

        /// <summary>
        /// The OnTrigger Method (when player leaves trigger box)
        /// sets the the GarbageDetonateButton to inactive
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && isActive)
            {
                ToggleGarbageDetonateButtonActiveState(false);
            }
        }

        /// <summary>
        /// This method locates the GarbageDetonateButton object. If found it sets the active state of the gameobject based of the
        /// method argument
        /// </summary>
        private void ToggleGarbageDetonateButtonActiveState(bool isActive)
        {
            Transform garbageDetonateButton = GameScreen.Instance.transform.Find("GarbageDetonateButton");
            if (garbageDetonateButton == null)
            {
                Debug.LogError("Garbage Detonate Button can not be found");
            }

            garbageDetonateButton.gameObject.SetActive(isActive);
        }
    }
}
