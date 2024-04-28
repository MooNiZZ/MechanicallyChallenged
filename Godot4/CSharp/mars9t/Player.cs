using Godot;

public partial class Player : CharacterBody2D
{
    private static float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    private Timer _dropDownTimer;
    private int _moveDirection;
    private bool _onPassablePlatform;
    private RayCast2D _passablePlatformDetectionRaycast;
    private bool _wantsToJump;

    [Export]
    public float Speed { get; set; } = 300.0f;

    [Export]
    public float JumpSpeed { get; set; } = 600.0f;

    [Export]
    public double DropTimerThresholdSeconds { get; set; } = 0.25;

    public override void _Ready()
    {
        base._Ready();

        _dropDownTimer = new Timer();
        _dropDownTimer.WaitTime = DropTimerThresholdSeconds;
        _dropDownTimer.OneShot = true;
        _dropDownTimer.Autostart = false;
        _dropDownTimer.Timeout += OnGetDownTimerTimeout;
        AddChild(_dropDownTimer);

        _passablePlatformDetectionRaycast = GetNode<RayCast2D>("%PassablePlatformRayCast");
        _passablePlatformDetectionRaycast.SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);

        SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);
    }

    public override void _UnhandledKeyInput(InputEvent inputEvent)
    {
        base._UnhandledKeyInput(inputEvent);

        if (Input.IsActionJustPressed("jump"))
        {
                _wantsToJump = true;
        }

        if (Input.IsActionJustPressed("drop_down"))
        {
                _dropDownTimer.Start();
        }
        else if (Input.IsActionJustReleased("drop_down"))
        {
            _dropDownTimer.Stop();
            SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);
        }

        if (Input.IsActionJustReleased("move_left"))
        {
            if (Input.IsActionPressed("move_right"))
            {
                _moveDirection = 1;
            }
            else
            {
                _moveDirection = 0;
            }
        }

        if (Input.IsActionJustReleased("move_right"))
        {
            if (Input.IsActionPressed("move_left"))
            {
                _moveDirection = -1;
            }
            else
            {
                _moveDirection = 0;
            }
        }

        if (Input.IsActionPressed("move_left"))
        {
            _moveDirection = -1;
        }
        else if (Input.IsActionPressed("move_right"))
        {
            _moveDirection = 1;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector2 velocity = Velocity;

        if (_moveDirection != 0)
        {
            velocity.X += (float)(_moveDirection * Speed);
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

        if (_passablePlatformDetectionRaycast.IsColliding())
        {
            _onPassablePlatform = true;
        }
        else
        {
            if (_onPassablePlatform == true)
            {
                SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);
            }

            _onPassablePlatform = false;
        }
    }

    private void OnGetDownTimerTimeout()
    {
        if (_onPassablePlatform)
        {
            SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, false);
        }
    }
}
