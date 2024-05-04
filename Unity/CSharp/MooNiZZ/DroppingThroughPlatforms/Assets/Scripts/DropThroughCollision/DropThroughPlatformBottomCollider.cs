using System;
using UnityEngine;

namespace MooNiZZ
{
    public class DropThroughPlatformBottomCollider : MonoBehaviour
    {
        [SerializeField] private DropThroughPlatform _dropThroughPlatform;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player")) // Make sure it's the player
            {
                _dropThroughPlatform.EnableCollisionWithObject(other);
            }
        }
    }
}