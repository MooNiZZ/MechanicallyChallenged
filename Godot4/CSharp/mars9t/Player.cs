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

        // Set the timer that acts as a "you have to hold for this long to initiate a drop"
        _dropDownTimer = new Timer();
        _dropDownTimer.WaitTime = DropTimerThresholdSeconds;
        _dropDownTimer.OneShot = true;
        _dropDownTimer.Autostart = false;
        _dropDownTimer.Timeout += DropDown_OnTimerTimeout;
        AddChild(_dropDownTimer);

        // Set character's collision masks based on some handy const values
        // All collision-related setup is done in code; this streamlines how we manage them
        SetCollisionMaskValue(Consts.BLOCKING_PLATFORM_COLLISION_LAYER, true);
        SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);

        // Configuration of the raycast can be done fully in the Inspector, partially there and here, or fully here
        // In this example, the ray's positionining is done in the Inspector but the collision is handled in code
        _passablePlatformDetectionRaycast = GetNode<RayCast2D>("%PassablePlatformRayCast");

        // Similar to chracter, set ray's collision mask; this one should detect only passable platforms
        _passablePlatformDetectionRaycast.SetCollisionMaskValue(Consts.BLOCKING_PLATFORM_COLLISION_LAYER, false);
        _passablePlatformDetectionRaycast.SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);

        // This property has to be set to true for this solution to properly reset the character's collision
        // It allows ray to still detect collision while the character has already started falling through
        // Thanks to this offset, we can reset character's collision once the ray stops detecting collision on its own
        // This allows to not rely on timer or a second shape collision to reset character's collision once it's fully below the platform
        _passablePlatformDetectionRaycast.HitFromInside = true;
    }

    public override void _UnhandledKeyInput(InputEvent inputEvent)
    {
        base._UnhandledKeyInput(inputEvent);

        Common_HandleInput();

        DropDown_HandleInput();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Common_PhysicsProcess(delta);

        DropDown_PhysicsProcess(delta);
    }

    #region Drop Down Methods

    /// <summary>
    /// This handles input related solely to the drop down
    /// The timer is set on action press and reset on its release
    /// Character's collision is also bein reset on the release to make sure we always get back to the neutral state
    /// </summary>
    private void DropDown_HandleInput()
    {
        if (Input.IsActionJustPressed("drop_down"))
        {
            _dropDownTimer.Start();
        }
        else if (Input.IsActionJustReleased("drop_down"))
        {
            _dropDownTimer.Stop();
            SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, true);
        }
    }

    /// <summary>
    /// This handles character's collision by checking raycast state
    /// </summary>
    /// <param name="delta">Delta between frames in seconds. Left this parameter out of convention. Otherwise it's not needed here</param>
    private void DropDown_PhysicsProcess(double delta)
    {
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

    /// <summary>
    /// Called when the timer for drop down detection fires
    /// </summary>
    private void DropDown_OnTimerTimeout()
    {
        if (_onPassablePlatform)
        {
            SetCollisionMaskValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, false);
        }
    }

    #endregion

    #region Common Methods

    /// <summary>
    /// This method contains all, more or less, typical input
    /// This is totally not necessary for the solution itself to work
    /// Adapt this or use any other general input handling code
    /// </summary>
    private void Common_HandleInput()
    {
        if (Input.IsActionJustPressed("jump"))
        {
            _wantsToJump = true;
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

    /// <summary>
    /// Similar to the input method this contains all, more or less, typical physics movement code
    /// This is totally not necessary for the solution itself to work (maybe outside of gravity so we actually fall through)
    /// Adapt this or use any other general movement handling code
    /// </summary>
    /// <param name="delta">Delta between frames in seconds</param>
    private void Common_PhysicsProcess(double delta)
    {
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
    }

    #endregion
}
