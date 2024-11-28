using System.Collections;
using System.Collections.Generic;
using Scripts.MissonLogMenu;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;
using Scripts.Game;


namespace Scripts.NPC
{
    /// <summary>
    /// This class handles the movement and the interaction handling with the walking npc
    /// This class inherrits the NPCTrigger Class
    /// </summary>
    public class WalkingNPCController : NPCTrigger
    {
        #region Class Variables
        [HideInInspector] public bool istalkingToPlayer = false;
        public List<string> NPCScript { get; set; } = new List<string>() { "Hello", "How Is Your Day", "Good Bye" };
        private int _scriptIndex = 0;
        private bool _coroutineActive = false;
        [SerializeField] private float _rotationSpeed = 3f;
        private bool _inTriggerBox = false;
        [SerializeField] private LayerMask _npcLayerMask;

        //Random Movement fields
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

        #region Start Methods
        /// <summary>
        /// This method calls the base class start method.
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }
        #endregion

        #region On Enable Methods
        /// <summary>
        /// When the component is activated, this method obtains a reference to the NPC GameObjects Rigid Body
        /// </summary>
        public void OnEnable()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        #endregion

        #region TriggerBox Methods
        /// <summary>
        /// This is method executes when the Player Enters the Trigger Box,
        /// this method sets the text box field visible above th npc, resets the script index to zero
        /// and sets the bool value for if the player in the trigger box to true
        /// </summary>
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(true);
                //Disable Player Movement
                _scriptIndex = 0;
                _inTriggerBox = true;
                GameScreen.Instance.ShowKeyPrompt("Press F Key To Talk To NPC");
            }
        }

        /// <summary>
        /// This is method executes when the Player leaves the Trigger Box,
        /// The method sets the text box field hidden above th npc
        /// The boolean fields for is the player is in the player box and talking to the npc are set to false
        /// </summary>
        protected override void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                bubbleText.SetActive(false);

                //enable player movement
                istalkingToPlayer = false;
                _inTriggerBox = false;
                GameScreen.Instance.HideKeyPrompt();
            }
        }
        #endregion

        #region Update Method
        /// <summary>
        /// The Update Method handles the NPC Logic. The method will check the conditions of the player being in the trigger box
        /// to rotate to the player and communicate with the player if the player trys to talk to hte player
        /// Or the method will instantiate random movement (for when the player is not communciating with the NPC)
        /// </summary>
        private void Update()
        {
            if (istalkingToPlayer)
            {
                //Player pressed F Key to continue talking to NPC
                if (PlayerManager.Instance.getTaskAccepted())
                {
                    HandleNPCConversation();
                }
                RotateToPlayer();
                return;
            }
            HandleNPCMovement();
            //Conditions (including pressing F Key) to start interaction with NPC
            if (_inTriggerBox && PlayerManager.Instance.CheckPlayerIsFacingTarget(_npcLayerMask) && PlayerManager.Instance.getTaskAccepted() && _scriptIndex == 0)
            {
                GameScreen.Instance.HideKeyPrompt();
                istalkingToPlayer = true;
                PlayerManager.Instance.DisablePlayerMovement();
                HandleNPCConversation();
            }

        }
        /// <summary>
        /// This method handles the random movement of the NPC. This consits of 3 modes of movement with there own random wait times
        /// The NPC can walk (using the rigid body to move the player)
        /// The NPC can rotate Left (transform.rotate)
        /// The NPC can rotate Right (transform.rotate)
        /// The NPC rotation each iteration of this method call can only be left or right.
        /// </summary>
        public void HandleNPCMovement()
        {
            if (istalkingToPlayer)
            {
                return;
            }

            //Only start Coroutine Random NPC Method if no iteration of NPCMovement is active
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
        /// This method sets the conditions of what type of movement should be processed by the HandleNPCMethod()
        /// The method sets the random wait and duration times for each type of movement the player can make. The type of movement
        /// the NPC can make is decided by this method by adjusting the corresponding class field boolean values to true or false
        /// Note: No two movements can occur at once
        /// Note: movement/wait times are constricted to a minimum and maxium time (see class variables for more details)
        /// </summary>
        public IEnumerator RandomNPCMovement()
        {
            //start of movement iteration
            _startedMovement = true;
            int rotateTime = Random.Range(_minTime,_maxTime);
            int rotateWait = Random.Range(_minTime,_maxTime);
            int rotateDirection = Random.Range(1, 3);

            int walkWait = Random.Range(_minTime,_maxTime);
            int walkTime = Random.Range(_minTime,_maxTime);

            //handle npc random 
            yield return new WaitForSeconds(walkWait);
            _isWalking = true;
            yield return new WaitForSeconds(walkTime);
            _isWalking = false;
            yield return new WaitForSeconds(rotateWait);

            //condition for rotating right
            if(rotateDirection == 1)
            {
                _isRotatingRight = true;
                yield return new WaitForSeconds(rotateTime);
                _isRotatingRight = false;
            }

            //condition for rotating left
            if(rotateDirection == 2)
            {
                _isRotatingLeft = true;
                yield return new WaitForSeconds(rotateTime);
                _isRotatingLeft = false;
            }

            //end of movement iteration
            _startedMovement = false;
        }

        /// <summary>
        /// This method rotates the NPC to the direction of the Player GameObject Transform Position
        /// </summary>
        public void RotateToPlayer()
        {
            Transform playerTransform = PlayerManager.Instance.gameObject.transform;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerTransform.position - transform.position),
_rotationSpeed * Time.deltaTime);
        }
        #endregion

        #region NPCConversation Methods
        /// <summary>
        /// This method deals with having a conversation with the NPC
        /// The method returns a boolean value indicating if the conversation with the player has been finished.
        /// </summary>
        private void HandleNPCConversation()
        {
            if(_coroutineActive == false && _scriptIndex < NPCScript.Count)
            {
                SetBubbleText(NPCScript[_scriptIndex]);
                StartCoroutine(StartNPCText());
            }
        }

        /// <summary>
        /// This method handles the NPC Communication for the Player. The idea is that this method Sets each Line from its script
        /// by calling the Base Class TypeText() Method to have each dialoge by the NPC appear in typewriting form on screen
        /// </summary>
        private IEnumerator StartNPCText()
        {
            //start of dialouge (single line from script)
            _coroutineActive = true;
            GameScreen.Instance.HideKeyPrompt();
            yield return StartCoroutine(TypeText());
            _scriptIndex += 1;
            //The condition that the NPC no longer has any more lines in its script to say to the player
            if(_scriptIndex == NPCScript.Count)
            {
                istalkingToPlayer = false;
                PlayerManager.Instance.EnablePlayerMovement();
                GameScreen.Instance.HideKeyPrompt();
            }
            else
            {
                GameScreen.Instance.ShowKeyPrompt("Press F Key To Continue");
            }
            //end of dialouge (single line from script)
            _coroutineActive = false;
        }
        #endregion
    }
}
