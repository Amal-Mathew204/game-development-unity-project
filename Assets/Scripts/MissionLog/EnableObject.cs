using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject info;

    private void Start()
    {
        info.SetActive(false);
    }

    public void whenButtonClicked()
    {
        info.SetActive(true);
    }


}
