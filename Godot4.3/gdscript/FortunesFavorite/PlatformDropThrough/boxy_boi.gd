extends CharacterBody2D


const SPEED = 300.0
const JUMP_VELOCITY = -600.0


func _physics_process(delta: float) -> void:
	platform_passing_check_method_1();
	platform_passing_check_method_2();
	
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("ui_left", "ui_right")
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	move_and_slide()

func platform_passing_check_method_1():
	if (Input.is_action_pressed("ui_down") && Input.is_action_just_pressed("crouch") ||
		Input.is_action_just_pressed("ui_down") && Input.is_action_pressed("crouch") 
	):
		self.position.y += 1

func platform_passing_check_method_2():
	if (Input.is_action_pressed("ui_down") && Input.is_action_just_pressed("crouch") ||
		Input.is_action_just_pressed("ui_down") && Input.is_action_pressed("crouch") 
	):
		self.set_collision_mask_value(2, false)
	if Input.is_action_just_released("ui_down") || Input.is_action_just_released("crouch"):
		self.set_collision_mask_value(2, true)

