using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTrigger : MonoBehaviour
{


    public GameObject bubbleText;

    // Start is called before the first frame update


    void Start()
    {
        bubbleText.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
        {
            bubbleText.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            bubbleText.SetActive(false);
        }

    }
    
}
