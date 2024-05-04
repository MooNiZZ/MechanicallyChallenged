# Submission to Mechanically Challenged April by Taffaz

#### Instructions for use

SampleScene has a setup with 3 platforms that can be dropped through
To add additional platforms you can add the Dropthrough prefab as a child to any platform GameObject that has a Collider2D on it.
Hold down and hit space to drop through the platform
Platform will remain as a non-collideable until the player leaves its bounding box.
If changed then the Player layer must be set in the inspector for the GroundChecker on the Player GameObject and DropthroughPlatforms prefab

#### Notes for users
PlayerActions.cs Auto generated from using new Input System
GroundChecker.cs determines when the player is touching the ground and provides an IsGrounded property and the transform that is being stood on.
DropthroughPlatform sets the Collider2D to ignore collisions on the PlayerLayer and then re-enables them when the player collider is no longer overlapping the platform.
PlayerController is a really basic 2d character controller which enables movement, jumping and most importantly the ability to initiate the drop through a platform.