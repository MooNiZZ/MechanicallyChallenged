using Godot;

public partial class Player : CharacterBody2D
{
    private static float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private int _direction;
    private bool _wantsToJump;
    private Timer _getDownTimer;

    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float JumpSpeed { get; set; } = 600.0f;

    [Export]
    public double DropTimerThresholdSeconds { get; set; } = 0.25;

    public override void _Ready()
    {
        base._Ready();

        _getDownTimer = new Timer();
        _getDownTimer.WaitTime = DropTimerThresholdSeconds;
        _getDownTimer.OneShot = true;
        _getDownTimer.Autostart = false;
        _getDownTimer.Timeout += OnGetDownTimerTimeout;
        AddChild(_getDownTimer);
    }

    public override void _Input(InputEvent inputEvent)
    {
        base._Input(inputEvent);

        if (inputEvent is InputEventKey keyEvent)
        {
            if (Input.IsActionJustPressed("jump"))
            {
                _wantsToJump = true;
            }

            if (Input.IsActionJustPressed("drop_down"))
            {
                _getDownTimer.Start();
            }

            if (Input.IsActionJustReleased("drop_down"))
            {
                _getDownTimer.Stop();
                SetCollisionMaskValue(2, true);
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

    private void OnGetDownTimerTimeout()
    {
        SetCollisionMaskValue(2, false);
    }
}
