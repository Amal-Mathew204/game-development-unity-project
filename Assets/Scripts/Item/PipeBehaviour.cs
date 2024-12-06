using Scripts.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Item
{
    public class PipeBehaviour : ItemPickup
    {
        [SerializeField] private float snapRange = 0.5f;  // Max range for snapping
        [SerializeField] private Transform[] snapPoints;  // Snap points on this pipe

        private BoxCollider _pipeCollider;

        private bool _isSnapped = false;
        // Start is called before the first frame update
        void Awake()
        {
            _pipeCollider = GetComponentInChildren<BoxCollider>();

            if (_pipeCollider == null)
            {
                Debug.LogError("Box Collider not found on child objects!");
            }

            _pipeCollider.isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_isSnapped)
            {
                return;  // No further snapping logic if already snapped
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            PipeBehaviour otherPipe = other.GetComponentInParent<PipeBehaviour>();
            if (otherPipe != null)
            {
                TrySnap(otherPipe);
            }
        }

    }
}

