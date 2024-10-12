using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public GameObject info;

    public void WhenButtonClicked()
    {
        info.SetActive(false);
    }

}
