# MechanicallyChallenged

## April 2024: Dropping through platforms

This project provides a custom `DropthroughPlatform` node which can be used to create platforms that allow objects to pass through them when certain conditions are met. This is useful for creating games with mechanics such as moving between floors of a building or passing through floors in a dungeon.

### Usage

To use the Drop Through Platforms in your own project you should follow the following steps:


#### Copy the file `dropthrough_platform.gd` inside your project directory

Copy the file `dropthrough_platform.gd` into your project or drag and drop the file into the resources window inside the Godot editor. After that, a new node `DropthroughPlatform` should be available to be used inside your scenes.


#### Add a `DropthroughPlatform` in your scene

You can add a new platform by adding a new child node of type `DropthroughPlatform` inside your scene. This node works like a regular `StaticBody2D` in Godot, you need to add a `CollisionShape` to define its shape and a `Sprite2D` or any other texture node you want for your platform.

Alternatively you can import into your scene the provided scene `Platform.tscn` that contains all the necessary nodes.


#### Trigger the `Drop Through` mechanic

To trigger the `Drop Through` mechanic during the game you need to call the `dropthrough()` method available on the `DropthroughPlatform` object from your character controller:

```GDScript
func _physics_process(delta):
	if is_on_floor() and Input.is_action_pressed("jump") and Input.is_action_pressed("crouch"):
		# The player is trying to drop trough the floor
		try_dropthrough()

func try_dropthrough():
	var platform: Node2D = get_last_slide_collision().get_collider()
	if platform.has_method("dropthrough"):
		# The player is trying to drop trough on a valid platform
		platform.dropthrough()
```

On this example, the line:
```GDScript
if is_on_floor() and Input.is_action_pressed("jump") and Input.is_action_pressed("crouch"):
```
makes sure that the player is laying on the floor and the actions 'jump' and 'crouch' are pressed at the same time. You can adjust this conditions to your own requirements for triggering the drop through mechanic.

After that it calls the method `try_dropthrough`. This method obtains a reference of the surface on witch the player is lying on and checks if it is of the type `DropthroughPlatform`. If it is a valid platform it calls the method `dropthrough` triggering the mechanic.


### Advanced Uses

#### Reactivation time

The `DropthroughPlatform` allows setting the `Reactivation Time` of the Platform (the default is 0.5sec). This value can be adjusted through the Godot inspector or can also be set inside the `_ready` method of the platform:

```GDScript
func _ready():
    reactivation_time = 2.0
```


#### Signals

The `DropthroughPlatform` node emits two signals: `object_passed_through` and `reactivated` when the mechanic is triggered. This signals can be used to further configure the behaviour or appearance of the platform, or to perform any other action inside your scene.

The following example shows how to use this signals for changing the platform color when the player drops through the platform:

```GDScript
func _ready():
	self.reactivated.connect(on_reactivate)
	self.object_passed_through.connect(on_passed_through)


func on_passed_through():
	platform.color = Color("#FF004D")


func on_reactivate():
	platform.color = Color("#83769C")
```


### Example

#### Example scene

An example scene can be found inside the `Examples` directory. This scene contains some  `DropthroughPlatform` and a player with a player script (`player.gd`) implementing the drop through mechanic. Also contains a `plataform.gd` script with an example of how to use the platform signals.


#### Live demo - Itch.io

A live demo of the project can be accessed here: [Itch](https://cod3monk.itch.io/mechanically-challenged-dropping-through-platforms)


### License

This project is licensed under the [MIT license](LICENSE.md).

