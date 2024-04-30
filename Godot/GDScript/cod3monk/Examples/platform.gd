# This is an example  illustrating how to use the events fired by the
# `Dropthrough Platform`.
extends DropthroughPlatform


@onready var platform = $Polygon2D


func _ready():
	# Connect the corresponding signals emitted from the Dropthrough Platform
	self.reactivated.connect(on_reactivate)
	self.object_passed_through.connect(on_passed_through)

	# Adjust the reactivation_time
	# self.reactivation_time = 0.6

func on_passed_through():
	# In this example we change the color of the platform to inform the player
	# that the platform mechanic has been triggered.
	platform.color = Color("#FF004D")
	# Debug log
	print("Passing through the Platform")


func on_reactivate():
	# The platform has reactivated itself and we change back the color to the
	# original value.
	platform.color = Color("#83769C")
	# Debug log
	print("Platform Reactivated")
