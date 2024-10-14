using Godot;

public partial class Bullet : Node2D
{
    [Export] Area2D area2D_;
    public Vector2 moveDirection_ { get; set; }
    public float speed_ { get; set; } = 700;
    public Timer timer_;

    public override void _Ready() 
    {
        var callable = new Callable(this, "HitSomething");
        area2D_.Connect("area_entered", callable);
        area2D_.BodyEntered += HitSomething;
    }

    public void HitSomething(Node2D body)
    {
        if (body.GetType() != typeof(Player))
            QueueFree();
        if (body is Enemy enemy)
        {
            // Possibly different logic depending on upgrades or enemies
            QueueFree();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        GlobalPosition = GlobalPosition + moveDirection_ * speed_ * (float)delta;

    }
}
