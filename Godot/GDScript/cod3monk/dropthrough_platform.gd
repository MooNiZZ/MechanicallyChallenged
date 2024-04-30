## Specialized `StaticBody2D` that allows another objecs to 'Drop Through' by
## disabling collision layers temporarily.
class_name DropthroughPlatform extends StaticBody2D

const DEFAULT_COLLISION_LAYER: int = 1

## Emitted when an object pass through the platform
signal object_passed_through
## Emitted when the platform reactivates
signal reactivated

## Time that takes to the platform to reactivate after a dropthrough
@export var reactivation_time: float = 0.5


## Disables a collision layer for the platform (Layer #1 by default), allowing
## the object to move through it. The platform will reactivate itself after
## `reactivation_time` seconds.
func dropthrough(layer: int = DEFAULT_COLLISION_LAYER):
	var timer = get_tree().create_timer(reactivation_time)
	timer.connect("timeout", self.reactivate.bind(layer))

	set_collision_layer_value(layer, false)
	self.object_passed_through.emit()


## Reactivates the collision layer for the platform.
func reactivate(layer: int):
	set_collision_layer_value(layer, true)
	self.reactivated.emit()
