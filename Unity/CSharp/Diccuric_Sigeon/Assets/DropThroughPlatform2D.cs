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

    public string DefaultLayer {get; set;}

    private void Start()
    {
        DefaultLayer = LayerMask.LayerToName(gameObject.layer);
        _ignoredCollders = new List<Collider2D>();
        _overlappedColliders = new List<Collider2D>();
    }

    public void StartDrop()
    {
        gameObject.layer = (int) Mathf.Log(DropThroughLayer.value, 2f);
        _isStarted = true;
    }

    private void FixedUpdate()
    {
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
        if(!_isStarted) return;

        gameObject.layer = LayerMask.NameToLayer(DefaultLayer);
        _isStarted = false;
        Overlap();

        for(var i = 0 ; i < _overlappedColliders.Count; i++)
        {
            _ignoredCollders.Add(_overlappedColliders[i]);
            Physics2D.IgnoreCollision(Collider, _overlappedColliders[i], true);
        }
    }
    
    private void Overlap()
    {
        _overlappedColliders.Clear();
        Collider.Overlap(ContactFilter, _overlappedColliders);
    }
}
