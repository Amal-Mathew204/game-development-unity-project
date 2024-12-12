using Scripts.Game;
using System.Collections;
using UnityEngine;
using PlayerManager = Scripts.Player.Player;

namespace Scripts.Item
{
    public class PipeAttachChecker : MonoBehaviour
    {
        [SerializeField] private float rayLength = 4f; // How far the ray will go
        [SerializeField] private LayerMask _pipeLayer;
        private PipeAttachChecker[] otherPipeAttachChecker;
        private int _point;
        private bool pipesAligned = false; // Current alignment state
        private Coroutine alignmentCoroutine; // Reference to the coroutine

        void Update()
        {
            if (PlayerManager.Instance.getTaskAccepted())
            {
                GameScreen.Instance.HideKeyPrompt();
                Balls();
            }
        }

        public void CheckForPipeAttachment()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, _pipeLayer))
            {
                if (hit.collider.CompareTag("Pipe"))
                {
                    Debug.Log("Ray hit a pipe: " + hit.collider.name);
                    otherPipeAttachChecker = hit.collider.GetComponentsInChildren<PipeAttachChecker>();

                    if (otherPipeAttachChecker != null)
                    {
                        if (otherPipeAttachChecker[0].ReciprocalCheck(this.gameObject))
                        {
                            Debug.Log("Both pipes aligned at attach point 1.");
                            
                            _point = 0;
                            UpdateAlignmentState(true);
                        }
                        else if (otherPipeAttachChecker[1].ReciprocalCheck(this.gameObject))
                        {
                            Debug.Log("Both pipes aligned at attach point 2.");
                            _point = 1;
                            UpdateAlignmentState(true);
                        }
                        else
                        {
                            UpdateAlignmentState(false);
                        }
                    }
                    else
                    {
                        Debug.Log("Other pipe not found.");
                        UpdateAlignmentState(false);
                    }
                }
            }
        }

        private void UpdateAlignmentState(bool aligned)
        {
            if (alignmentCoroutine != null)
            {
                StopCoroutine(alignmentCoroutine); // Stop any ongoing coroutine
            }

            alignmentCoroutine = StartCoroutine(CheckPipeAlignment(aligned));
        }

        private IEnumerator CheckPipeAlignment(bool aligned)
        {
            // Add a slight delay (optional) to avoid flickering in UI updates
            yield return new WaitForSeconds(0.1f);

            if (aligned)
            {
                GameScreen.Instance.ShowKeyPrompt("Attach?");
                Debug.Log("Attach prompt displayed.");
            }
            else
            {
                GameScreen.Instance.HideKeyPrompt();
                Debug.Log("Attach prompt hidden.");
            }
        }

        public bool ReciprocalCheck(GameObject originalPipe)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength, _pipeLayer))
            {
                Debug.Log("Pipes both hit.");
                return true;
            }

            Debug.Log("Pipes not hit.");
            return false;
        }

        public void Balls()
        {
            Transform currentAttachPoint = this.transform;

            if (otherPipeAttachChecker != null)
            {
                Transform otherAttachPoint = otherPipeAttachChecker[_point].transform;
                Vector3 movementVector = otherAttachPoint.position - currentAttachPoint.position;

                PipeEditor pipeEditor = this.GetComponentInParent<PipeEditor>();
                if (pipeEditor != null)
                {
                    pipeEditor.MovePipe(movementVector);
                }
                else
                {
                    Debug.Log("Pipe editor not found on parent.");
                }
            }
            else
            {
                Debug.Log("Other pipe attach checker not found.");
            }
        }
    }
}
