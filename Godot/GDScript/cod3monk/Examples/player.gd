extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -600.0

var gravity = ProjectSettings.get_setting("physics/2d/default_gravity")


func _physics_process(delta):
	if Input.is_action_pressed("jump") and is_on_floor():
		if Input.is_action_pressed("crouch"):
			# The player is trying to drop trough the floor
			self.try_dropthrough()
		else:
			# Regular jump
			velocity.y = JUMP_VELOCITY

	if not is_on_floor():
		velocity.y += gravity * delta

	var direction = Input.get_axis("left", "right")
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	move_and_slide()


## Try to drop through the colliding surface
func try_dropthrough():
	var collider_object: Node2D = get_last_slide_collision().get_collider()
	if collider_object.has_method("dropthrough"):
		collider_object.dropthrough()

