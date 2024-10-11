using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : MonoBehaviour
{
    public GameObject info;

    public void whenButtonClicked()
    {
        info.SetActive(false);
    }

}
