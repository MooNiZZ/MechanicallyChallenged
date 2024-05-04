using System;
using UnityEngine;

namespace MooNiZZ
{
    public class DropThroughPlatform : MonoBehaviour
    {
        [SerializeField] private Collider2D _collider2D;

        private void Awake()
        {
            _collider2D ??= GetComponent<Collider2D>();
            // _bottomCollider2D ??= GetComponentInChildren<Collider2D>();
        }

        public void DropThrough(Collider2D other)
        {
            Physics2D.IgnoreCollision(_collider2D, other, true);
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            EnableCollisionWithObject(other);
        }

        public void EnableCollisionWithObject(Collider2D other)
        {
            Physics2D.IgnoreCollision(_collider2D, other, false);
        }
    }
}