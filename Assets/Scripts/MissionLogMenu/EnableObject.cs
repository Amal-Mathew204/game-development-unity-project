using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.MissonLogMenu
{
    public class NewBehaviourScript : MonoBehaviour
    {
        public GameObject info;

        private void Start()
        {
            info.SetActive(false);
        }

        public void WhenButtonClicked()
        {
            info.SetActive(true);
        }


    }
}
