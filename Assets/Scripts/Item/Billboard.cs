using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.Item
{
    public class Billboard : MonoBehaviour
    {
        private Transform _cameraTransform;
        // Start is called before the first frame update
        void Start()
        {
            _cameraTransform = Camera.main.transform;
        }
        /// <summary>
        /// This method is called every frame and makes the item text face the camera
        /// </summary>
        void Update()
        {
            transform.LookAt(transform.position + _cameraTransform.forward);
        }
    }
}
