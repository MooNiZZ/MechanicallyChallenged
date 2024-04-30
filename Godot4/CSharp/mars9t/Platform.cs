using Godot;

public partial class Platform : Node2D
{
    private static Color Green = new Color(0.067f, 0.667f, 0.067f);
    private static Color Red = new Color(0.667f, 0.055f, 0);

    [Export]
    public bool Passable { get; set; }

    public override void _Ready()
    {
        var platformBody = GetNode<StaticBody2D>("%StaticBody");
        var platformCollision = GetNode<CollisionShape2D>("%CollisionShape");

        // Analogous to character setup, here the platform's collision layers are set
        // The use of common const values makes sure we don't accidentally use wrong numbers
        platformBody.SetCollisionLayerValue(Consts.BLOCKING_PLATFORM_COLLISION_LAYER, !Passable);
        platformBody.SetCollisionLayerValue(Consts.PASSABLE_PLATFORM_COLLISION_LAYER, Passable);

        // This allows to jump through the platform from below
        platformCollision.OneWayCollision = Passable;

        // Handly coloring :)
        Modulate = Passable ? Green : Red;
    }
}
