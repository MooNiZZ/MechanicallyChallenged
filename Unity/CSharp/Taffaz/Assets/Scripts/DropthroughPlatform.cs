using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MechanicallyChallenged.Taffaz
{
    public class DropthroughPlatform : MonoBehaviour
    {

        PlayerController player;
        Collider2D platformCollider;
        LayerMask originalExclude;
        [SerializeField] LayerMask playerLayer;

        private void Start()
        {
            player = FindObjectOfType<PlayerController>();
            platformCollider = transform.parent.GetComponent<Collider2D>();
            playerLayer = LayerMask.GetMask("Player");
        }

        public void StartDrop()
        {
            platformCollider.excludeLayers = playerLayer;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log(collision.gameObject.name);
            if (collision == player.gameObject.GetComponent<Collider2D>())
            {
                platformCollider.excludeLayers = 0;
            }
        }

    }
}