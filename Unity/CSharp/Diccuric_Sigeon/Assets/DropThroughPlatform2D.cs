using System.Collections.Generic;
using UnityEngine;

public class DropThroughPlatform2D : MonoBehaviour
{
    public Collider2D Collider;
    public LayerMask DropThroughLayer;
    private List<Collider2D> _overlappedColliders;
    private List<Collider2D> _ignoredCollders;
    public ContactFilter2D ContactFilter;
    private bool _isStarted;

    private void Start()
    {
        _ignoredCollders = new List<Collider2D>();
        _overlappedColliders = new List<Collider2D>();
    }

    public void StartDrop()
    {
        _isStarted = true;
    }

    private void FixedUpdate()
    {
        if(_isStarted)
        {
            Overlap();
            Ignore();
        }

        if(_ignoredCollders.Count > 0)
        {
            Overlap();

            for(var i = _ignoredCollders.Count - 1; i > -1; i--)
            {
                if(!_overlappedColliders.Contains(_ignoredCollders[i]))
                {
                    Physics2D.IgnoreCollision(Collider, _ignoredCollders[i], false);
                    _ignoredCollders.RemoveAt(i);
                }
            }
        }
    }

    public void StopDrop()
    {
        _isStarted = false;
    }
    
    private void Overlap()
    {
        _overlappedColliders.Clear();
        Collider.Overlap(ContactFilter, _overlappedColliders);
    }

    private void Ignore()
    {
        for(var i = 0 ; i < _overlappedColliders.Count; i++)
        {
            _ignoredCollders.Add(_overlappedColliders[i]);
            Physics2D.IgnoreCollision(Collider, _overlappedColliders[i], true);
        }
    }
}
