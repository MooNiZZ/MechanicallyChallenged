using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MechanicallyChallenged.Taffaz
{
    public class PlayerController : MonoBehaviour
    {

        Rigidbody2D rb;
        [SerializeField] float moveSpeed = 100f;
        [SerializeField] float jumpForce = 10f;

        PlayerActions playerActions;

        GroundChecker groundChecker;

        public bool isJumping;
        public bool isFalling;

        Vector2 moveDirection = Vector2.zero;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            playerActions = new PlayerActions();
            groundChecker = GetComponent<GroundChecker>();
        }

        private void OnEnable()
        {
            playerActions.Gameplay.Enable();
            playerActions.Gameplay.Jump.performed += OnJump;
            playerActions.Gameplay.Move.performed += OnMove;
            playerActions.Gameplay.Move.canceled += OnMove;
        }

        private void OnDisable()
        {
            playerActions.Gameplay.Disable();
            playerActions.Gameplay.Jump.performed -= OnJump;
            playerActions.Gameplay.Move.performed -= OnMove;
            playerActions.Gameplay.Move.canceled -= OnMove;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    isJumping = true;
                    break;
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    moveDirection = context.ReadValue<Vector2>();
                    break;
                case InputActionPhase.Canceled:
                    moveDirection = Vector2.zero;
                    break;

            }

        }
        private void FixedUpdate()
        {
            Move();
            Jump();

            if (groundChecker.IsGrounded)
            {
                isFalling = false;
            }
        }


        private void Move()
        {
            Vector2 velocity = moveSpeed * Time.fixedDeltaTime * moveDirection;
            rb.velocity = new Vector2(velocity.x, rb.velocity.y);
        }

        private void Jump()
        {
            if (!groundChecker.IsGrounded || !isJumping || isFalling)
            {
                isJumping = false;
                return;
            }



            if (isJumping && moveDirection.y < 0)
            {

                var platform = groundChecker.Ground.gameObject.GetComponentInChildren<DropthroughPlatform>();
                if (platform != null)
                {
                    Debug.Log("Drop");
                    platform.StartDrop();
                    isFalling = true;
                    isJumping = false;
                }
                return;
            }

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
        }
    }
}