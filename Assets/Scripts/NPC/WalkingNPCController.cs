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

        //Movement fields
        private int _minTime = 1;
        private int _maxTime = 5;
        private bool _isWalking = false;
        private bool _isRotatingLeft = false;
        private bool _isRotatingRight = false;
        private bool _startedMovement = false;
        [SerializeField] private float _npcRotationSpeed = 3f;
        [SerializeField] private float _movementSpeed = 5f;
        private Rigidbody _rigidBody;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected override void Start()
        {
            base.Start();
            NPCScript = new List<string>() { "Hello", "Good Bye" };
            _rigidBody = GetComponent<Rigidbody>();

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
            if (istalkingToPlayer)
            {
                return;
            }

            if(_startedMovement == false)
            {
                StartCoroutine(RandomNPCMovement());
            }

            //Process Random Movement Conditions
            if (_isRotatingRight)
            {
                transform.Rotate(transform.up * Time.deltaTime * _npcRotationSpeed);
            }
            if (_isRotatingLeft)
            {
                transform.Rotate(transform.up * Time.deltaTime * -_npcRotationSpeed);
            }
            if (_isWalking)
            {
                _rigidBody.AddForce(transform.forward * _movementSpeed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerator RandomNPCMovement()
        {
            _startedMovement = true;
            int rotateTime = Random.Range(_minTime,_maxTime);
            int rotateWait = Random.Range(_minTime,_maxTime);
            int rotateDirection = Random.Range(1, 3);

            int walkWait = Random.Range(_minTime,_maxTime);
            int walkTime = Random.Range(_minTime,_maxTime);


            yield return new WaitForSeconds(walkWait);
            _isWalking = true;
            yield return new WaitForSeconds(walkTime);
            _isWalking = false;
            yield return new WaitForSeconds(rotateWait);

            if(rotateDirection == 1)
            {
                _isRotatingRight = true;
                yield return new WaitForSeconds(rotateTime);
                _isRotatingRight = false;
            }

            if(rotateDirection == 2)
            {
                _isRotatingLeft = true;
                yield return new WaitForSeconds(rotateTime);
                _isRotatingLeft = false;
            }

            _startedMovement = false;
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
