using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A platform that allows the player to go through it 
/// </summary>
public class PhaseablePlatform : MonoBehaviour
{
    [SerializeField] [Tooltip("Which colliders to restrict while in phasing mode")]
    private LayerMask ignorableMask;

    private LayerMask _originalMask;

    /// <summary>
    /// Determines if the platform is in a phaseable state and we should care about tracking the player
    /// </summary>
    private bool _isPhaseableState;

    private PlatformEffector2D _platformEffector;

    private void Awake()
    {
        _platformEffector = GetComponent<PlatformEffector2D>();
        _originalMask = _platformEffector.colliderMask;
    }

    public void RequestPhasing()
    {
        _isPhaseableState = true;
        _platformEffector.colliderMask = ignorableMask;
    }

    /// <summary>
    /// This also could live in a sub component and you could use events that notify a parent: "Hey, the player is no longer in the area, restore state"
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isPhaseableState && other.CompareTag("Player"))
        {
            _isPhaseableState = false;
            _platformEffector.colliderMask = _originalMask;
        }
    }
}