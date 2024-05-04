using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraFollower : MonoBehaviour
{
    [SerializeField] [Tooltip("Transform for the player to follow")]
    private Transform player;

    // update with physics
    private void FixedUpdate()
    {
        var playerPos = player.position;
        var cameraTransform = transform;
        cameraTransform.position = new Vector3(playerPos.x, playerPos.y, cameraTransform.position.z);
    }
}