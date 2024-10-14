using Godot;

public partial class Bullet : Node2D
{
    [Export] Area2D area2D_;
    public Vector2 moveDirection_ { get; set; }
    public float speed_ { get; set; } = 700;
    public float knockback = 20f;
    public Timer timer_;
    bool collided_ = false;

    public override void _Ready() 
    {
        var callable = new Callable(this, "HitSomething");
        area2D_.Connect("area_entered", callable);
        area2D_.BodyEntered += HitSomething;
    }

    public void HitSomething(Node2D body)
    {
        if (body.Name == "Hurtbox" && !collided_)
        {
            // Possibly different logic depending on upgrades or enemies
            collided_ = true;
            Enemy enemy = body.GetParent<Enemy>();
            enemy.TakeDamage(1, knockback: moveDirection_ * knockback);
            QueueFree();
        }
        if (body.GetType() != typeof(Player))
            QueueFree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        GlobalPosition = GlobalPosition + moveDirection_ * speed_ * (float)delta;
    }
}
