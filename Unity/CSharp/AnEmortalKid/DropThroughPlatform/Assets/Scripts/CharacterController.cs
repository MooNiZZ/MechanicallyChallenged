using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField] [Tooltip("How much force to apply when moving horizontally")]
    private float _horizontalForce;

    [SerializeField] [Tooltip("Force to apply when jumping")]
    private float _jumpForce;

    [Header("Detection Settings")] [SerializeField] [Tooltip("How far to check for the ground")]
    private float groundableDistance;

    [SerializeField] [Tooltip("What types of objects to find during ground checks")]
    private LayerMask groundMask;

    #region inputs

    private float _horizontalInput;

    private bool _jumpRequested;

    private bool _jumpDownRequested;

    #endregion

    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        // Some sort of buffer so we can stash the request while update runs and execute it in fixed update
        if (!_jumpRequested)
        {
            _jumpRequested = Input.GetButtonDown("Jump");
        }

        var verticalAxis = Input.GetAxis("Vertical");
        // we requested to jump down
        if (verticalAxis < 0)
        {
            _jumpDownRequested = true;
        }

        // If you want to see what the inputs are
        // Debug.LogFormat("Inputs horizontal:{0}  jump: {1} jumpDown: {2}", _horizontalInput, _jumpRequested, _jumpDownRequested);
    }

    private void FixedUpdate()
    {
        if (_jumpRequested)
        {
            _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            // allow another jump
            // if you had a limited number of jumps , you would do that here too
            _jumpRequested = false;
        }

        // handle this check to see if we can phase through
        if (_jumpDownRequested)
        {
            // Cast a ray down and see if the platform below us is a PhaseablePlatform
            var raycastHit = Physics2D.Raycast(transform.position, Vector2.down, groundableDistance, groundMask);
            Debug.DrawRay(transform.position, Vector2.down * groundableDistance, Color.red, 0.5f);
            if (raycastHit.collider)
            {
                Debug.LogFormat("Collider hit {0}", raycastHit.collider.gameObject.name);
                // Request to be let through
                if (raycastHit.collider.TryGetComponent<PhaseablePlatform>(out PhaseablePlatform pp))
                {
                    pp.RequestPhasing();
                }
            }

            _jumpDownRequested = false;
        }

        // move char
        _rigidBody.AddForce(new Vector2(_horizontalInput, 0) * _horizontalForce);
    }

    private void OnDrawGizmos()
    {
        // Draw the ground check
        var collider = GetComponent<Collider2D>();
        var size = collider.bounds.size;
        var extents = collider.bounds.extents;
        var centerPosition = transform.position;
        var leftCoordinate = new Vector3(centerPosition.x - extents.x, centerPosition.y - groundableDistance, 0);
        var rightCoordinate = new Vector3(centerPosition.x + extents.x, centerPosition.y - groundableDistance, 0);

        // draw where our ground check will go
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftCoordinate, rightCoordinate);
    }
}