using System;
using UnityEngine;

namespace MooNiZZ
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Collider2D _playerCollider2D;
        [SerializeField] private float _movementSpeed;

        private Collider2D _dropThroughPlatformCollider;
        private DropThroughPlatform _dropThroughPlatform;
        // Start is called before the first frame update

        private void Awake()
        {
            _rigidbody2D ??= GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.S))
            {
                HandleCollisionBasedDropThrough();
            }
            else
            {
                HandleJump();
            }

            HandleMovement();
        }

        private void HandleCollisionBasedDropThrough()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _dropThroughPlatform != null)
            {
                _dropThroughPlatform.DropThrough(_playerCollider2D);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // Using Tags
            if (other.gameObject.CompareTag("DropThroughPlatform"))
            {
                other.gameObject.TryGetComponent(out _dropThroughPlatform);
            }

            // Using Layer
            if (other.gameObject.layer == LayerMask.NameToLayer("DropThroughPlatform"))
            {
                other.gameObject.TryGetComponent(out _dropThroughPlatform);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            // Using Tags
            if (other.gameObject.CompareTag("DropThroughPlatform"))
            {
                // Extra safety check
                var dropThroughPlatform = other.gameObject.GetComponent<DropThroughPlatform>();
                if (dropThroughPlatform != null && _dropThroughPlatform == dropThroughPlatform)
                {
                    _dropThroughPlatform = null;
                }
            }

            // Using Layer
            if (other.gameObject.layer == LayerMask.NameToLayer("DropThroughPlatform"))
            {
                var dropThroughPlatform = other.gameObject.GetComponent<DropThroughPlatform>();
                if (dropThroughPlatform != null && _dropThroughPlatform == dropThroughPlatform)
                {
                    _dropThroughPlatform = null;
                }
            }
        }

        private void HandleJump()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rigidbody2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            }
        }

        private void HandleMovement()
        {
            var velocity = _rigidbody2D.velocity;
            if (Input.GetKey(KeyCode.A))
            {
                velocity.x = -_movementSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                velocity.x = _movementSpeed;
            }

            _rigidbody2D.velocity = velocity;
        }
    }
}