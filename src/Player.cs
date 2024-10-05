using System.Collections.Generic;
using Godot;

public partial class Player : CharacterBody2D
{
    [Export] public float speed_ { get; set; } = 100;
    [Export] AnimationPlayer animationPlayer_;
    [Export] PackedScene bullet_;
    [Export] Node2D bulletList_;
    [Export] Node2D shootLeftSpawn_;
    [Export] Node2D shootRightSpawn_;
    [Export] Node2D shootUpSpawn_;
    [Export] Node2D shootDownSpawn_;
    [Export] Node2D shootUpLeftSpawn_;
    [Export] Node2D shootUpRightSpawn_;
    [Export] Node2D shootDownLeftSpawn_;
    [Export] Node2D shootDownRightSpawn_;
    [Export] Timer timer_;
    Node2D bulletSpawn_ { get => shootLeftSpawn_; }
    Vector2 facingDirection_ = Vector2.Left;
    Vector2 moveDirection_ = Vector2.Zero;
    bool lastAInput_ = false;
    bool canShoot_ = true;
    bool lockFacingDirection_ = false;
    float shootCooldown_ = 0.4f;
    enum PlayerState
    {
        IDLE = 0,
        SHOOTING
    }

    public override void _Ready()
    {
        animationPlayer_.Play("default");
        timer_.Timeout += OnTimerCooldown;
    }

    private void OnTimerCooldown()
    {
        canShoot_ = true;
    }

    public override void _Process(double delta)
    {
        // TODO Animation make player face direction they move and change sprite animation
        if (Input.IsActionPressed("up"))
            moveDirection_.Y = -1;
        else if (Input.IsActionPressed("down"))
            moveDirection_.Y = 1;
        else
            moveDirection_.Y = 0;
        if (Input.IsActionPressed("left"))
            moveDirection_.X = -1;
        else if (Input.IsActionPressed("right"))
            moveDirection_.X = 1;
        else
            moveDirection_.X = 0;
        moveDirection_ = moveDirection_.Normalized();

        if (Input.IsActionPressed("a") && canShoot_)
        {
            // shoot, set cooldown, lock facing direction
            GD.Print("shoot");
            Bullet bullet = bullet_.Instantiate<Bullet>();
            bulletList_.AddChild(bullet);
            bullet.GlobalPosition = bulletSpawn_.GlobalPosition;
            bullet.moveDirection_ = facingDirection_;
            timer_.Start();
            canShoot_ = false;
            lockFacingDirection_ = true;
        }
        else
        {
            lockFacingDirection_ = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 motion = moveDirection_ * speed_ * (float)delta;
        Velocity = motion;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("b"))
        {
            // bomb
            GD.Print("bomb");
        }
    }

    public void HandleBulletCollision(Area2D area)
    {
        // enemy will handle when it gets hit by a bullet, just the name of the object
        area.GetParent().QueueFree();
    }
}
