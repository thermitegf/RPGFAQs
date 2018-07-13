using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class StablePlayerMovement : MonoBehaviour
    {
        // The maximum speed in units per second the player will move
        public float MaxSpeed { get; } = 5f;
        public float Deadzone = 0.1f;
        public float TalkingRange = 2f;
        public bool FreezePlayer = false;
        public Vector2 PreviousDirection = Vector2.zero;
        public Vector2 PreviousMovement { get; private set; } = Vector2.zero;
        private Vector2 _previousPosition;
        private Rigidbody2D _rb;
        private DialogueManager _dialogue;
        private DialogueSource _currentBubble;
        private bool _isTalking;
        public bool IsTalking
        {
            get { return _isTalking; }
            set
            {
                _isTalking = value;
                // The player cannot move while talking.
                FreezePlayer = value;
            }
        }
        // The amount of time a player is unable to speak after ending a dialogue
        private float _speechCooldown = 0.75f;
        private bool _inSpeechCooldown = false;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _dialogue = FindObjectOfType<DialogueManager>();
            _previousPosition = transform.position;
        }

        void Update()
        {
            // Don't respond to input while the game is paused.
            if (Pauser.IsPaused)
            {
                return;
            }
            Move();
            Speak();
        }

        void Move()
        {
            if (FreezePlayer)
            {
                _rb.velocity = Vector2.zero;
                return;
            }
            float hSpeed = Input.GetAxis(InputConstants.MOV_H_AXIS);
            float vSpeed = Input.GetAxis(InputConstants.MOV_V_AXIS);

            Vector2 input = Vector2.zero;
            if (PreviousDirection == Vector2.zero)
            {
                if (hSpeed == vSpeed)
                {
                    if (Time.deltaTime % 2 == 0)
                    {
                        input.x = hSpeed;
                    }
                    else
                    {
                        input.y = vSpeed;
                    }
                }
                else if (Mathf.Abs(hSpeed) > Mathf.Abs(vSpeed))
                {
                    input.x = hSpeed;
                }
                else
                {
                    input.y = vSpeed;
                }
            }
            else
            {
                if (PreviousDirection.x != 0f)
                {
                    if (hSpeed != 0f)
                    {
                        input.x = hSpeed;
                    }
                    else
                    {
                        input.y = vSpeed;
                    }
                }
                else if (PreviousDirection.y != 0f)
                {
                    if (vSpeed != 0f)
                    {
                        input.y = vSpeed;
                    }
                    else
                    {
                        input.x = hSpeed;
                    }
                }
            }

            if (input.magnitude < Deadzone)
            {
                input = Vector2.zero;
            }
            PreviousDirection = input;
            input.x *= MaxSpeed;
            input.y *= MaxSpeed;
            _rb.velocity = input;
            PreviousMovement = (Vector2)transform.position - _previousPosition;
            _previousPosition = transform.position;
        }

        void Speak()
        {
            if (_inSpeechCooldown)
            {
                return;
            }
            if (!IsTalking)
            {
                _currentBubble = _dialogue.ActivateNearestTalkable(transform.position, TalkingRange);
                if (_currentBubble != null && Input.GetButtonUp(InputConstants.SUBMIT_AXIS))
                {
                    IsTalking = true;
                    _currentBubble.InitiateDialogue(this);
                }
            }
        }

        IEnumerator SpeechCooldown()
        {
            _inSpeechCooldown = true;
            yield return new WaitForSeconds(_speechCooldown);
            _inSpeechCooldown = false;

        }
    }
}
