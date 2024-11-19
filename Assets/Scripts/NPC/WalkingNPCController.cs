using System.Collections;
using System.Collections.Generic;
using Scripts.MissonLogMenu;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;


namespace Scripts.NPC
{
    /// <summary>
    /// This class handles the movement and the interaction handling with the walking npc
    /// </summary>
    public class WalkingNPCController : NPCTrigger
    {
        #region Class Variables
        [HideInInspector] public bool istalkingToPlayer = false;
        public List<string> NPCScript { get; set; }
        private int _scriptIndex = 0;
        private bool _coroutineActive = false;
        [SerializeField] private float _rotationSpeed = 3f;
        private bool _inTriggerBox = false;
        [SerializeField] private LayerMask _npcLayerMask;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected override void Start()
        {
            base.Start();
            NPCScript = new List<string>() { "Hello", "Good Bye" };
        }

        #region TriggerBox Methods
        /// <summary>
        /// 
        /// </summary>
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(true);
                //Disable Player Movement
                _scriptIndex = 0;
                _inTriggerBox = true;
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

                //enable player movement
                istalkingToPlayer = false;
                _inTriggerBox = false;
            }
        }
        #endregion

        #region Update Method
        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (istalkingToPlayer)
            {
                if (PlayerManager.Instance.getTaskAccepted())
                {
                    HandleNPCConversation();
                }
                RotateToPlayer();
                return;
            }
            HandleNPCMovement();
            if (_inTriggerBox && PlayerManager.Instance.CheckPlayerIsFacingTarget(_npcLayerMask) && PlayerManager.Instance.getTaskAccepted() && _scriptIndex == 0)
            {
                istalkingToPlayer = true;
                PlayerManager.Instance.DisableNPCMovement();
                HandleNPCConversation();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        public void HandleNPCMovement()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void RotateToPlayer()
        {
            Transform playerTransform = PlayerManager.Instance.gameObject.transform;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTransform.position - transform.position),
_rotationSpeed * Time.deltaTime);
            //transform.LookAt(playerTransform.position);
        }
        #endregion


        #region NPCConversation Methods
        /// <summary>
        /// This method deals with having a conversation with the NPC
        /// The method returns a boolean value indicating if the conversation with the player has been finished.
        /// </summary>
        private void HandleNPCConversation()
        {
            //add condition F key pressed
            if(_coroutineActive == false && _scriptIndex < NPCScript.Count)
            {
                SetBubbleText(NPCScript[_scriptIndex]);
                StartCoroutine(StartNPCText());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator StartNPCText()
        {
            _coroutineActive = true;
            yield return StartCoroutine(TypeText());
            _scriptIndex += 1;
            if(_scriptIndex == NPCScript.Count)
            {
                istalkingToPlayer = false;
                PlayerManager.Instance.EnableNPCMovement();
            }
            _coroutineActive = false;
        }
        #endregion
    }
}
