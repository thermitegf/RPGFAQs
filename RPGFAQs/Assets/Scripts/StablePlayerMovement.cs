using System;
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
        public float MaxSpeed = 5f;
        public float Deadzone = 0.1f;
        public float TalkingRange = 2f;
        public bool FreezePlayer = false;
        private Rigidbody2D _rb;
        private Vector2 _previousDirection = Vector2.zero;
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

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _dialogue = GameObject.Find("LightBackground").GetComponent<DialogueManager>();
        }

        void Update()
        {
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
            if (_previousDirection == Vector2.zero)
            {
                if (hSpeed == vSpeed)
                {
                    // In a stalemate between axes, the axis used is chosen by a coin flip.
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
                if (hSpeed != _previousDirection.x)
                {
                    input.x = hSpeed;
                }
                else if (vSpeed != _previousDirection.y)
                {
                    input.y = vSpeed;
                }
                else
                {
                    input = _previousDirection;
                }
            }

            if (input.magnitude < Deadzone)
            {
                input = Vector2.zero;
            }
            input.x *= MaxSpeed;
            input.y *= MaxSpeed;
            _rb.velocity = input;
        }

        void Speak()
        {
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
    }
}
