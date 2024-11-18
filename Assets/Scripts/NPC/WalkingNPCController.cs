using System.Collections;
using System.Collections.Generic;
using Scripts.MissonLogMenu;
using UnityEngine;



namespace Scripts.NPC
{
    /// <summary>
    /// This class handles the movement and the interaction handling with the walking npc
    /// </summary>
    public class WalkingNPCController : NPCTrigger
    {
        #region Class Variables
        public bool istalkingToPlayer = false;
        public List<string> NPCScript { get; set; };
        #endregion

        protected override void Start()
        {
            base.Start();
            NPCScript = new List<string>();
        }

        #region TriggerBox Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(true);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(false);

            }
        }
        #endregion

        #region NPCConversation Methods
        /// <summary>
        /// This method deals with having a conversation with the NPC
        /// </summary>
        private void HandleNPCConversation()
        {

        }
        #endregion
    }
}
