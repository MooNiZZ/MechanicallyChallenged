using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private static float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private int _direction;
    private bool _wantsToJump;

    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float JumpSpeed { get; set; } = 400.0f;

    public override void _Input(InputEvent inputEvent)
    {
        base._Input(inputEvent);

        if (inputEvent is InputEventKey keyEvent)
        {
            if (Input.IsActionJustPressed("jump"))
            {
                _wantsToJump = true;
            }

            if (Input.IsActionJustReleased("move_left"))
            {
                if (Input.IsActionPressed("move_right"))
                {
                    _direction = 1;
                }
                else
                {
                    _direction = 0;
                }
            }

            if (Input.IsActionJustReleased("move_right"))
            {
                if (Input.IsActionPressed("move_left"))
                {
                    _direction = -1;
                }
                else
                {
                    _direction = 0;
                }
            }

            if (Input.IsActionPressed("move_left"))
            {
                _direction = -1;
            }
            else if (Input.IsActionPressed("move_right"))
            {
                _direction = 1;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector2 velocity = Velocity;

        if (_direction != 0)
        {
            velocity.X += (float)(_direction * Speed);
            velocity.X = Mathf.Clamp(velocity.X, -Speed, Speed);
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
        }

        if (_wantsToJump)
        {
            velocity.Y -= JumpSpeed;
            _wantsToJump = false;
        }

        velocity.Y += Gravity * (float)delta;

        Velocity = velocity;

        MoveAndSlide();
    }
}
