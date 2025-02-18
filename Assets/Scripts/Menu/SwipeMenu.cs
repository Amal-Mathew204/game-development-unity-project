using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.Menu
{
    public class SwipeMenu : MonoBehaviour
    {
        public GameObject scrollbar;
        float scroll_pos = 0;
        float[] pos;

        /// <summary>
        /// Handles smooth transitions for a scrollbar-controlled UI element 
        /// Calculates positions for child elements based on the number of children and evenly spaces them
        /// </summary>
        private void Update()
        {
            // Calculate the positions for each child element based on the number of children
            pos = new float[transform.childCount];
            float distance = 1f / (pos.Length - 1); // Distance between each position

            
            for (int i = 0; i < pos.Length; i++)
            {
                pos[i] = distance * i; 
            }
            // Handle user input on the scrollbar
            if (PlayerManager.Instance.CheckLeftMouseButtonDown())
            {
                scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < pos.Length; i++)
                {
                    if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                    {
                        scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    }
                }
            }

            // Scale the child elements based on their proximity to the scrollbar's handle
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                    for (int a = 0; a < pos.Length; a++)
                    {
                        if (a != i)
                        {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
            }
        }
    }
}
