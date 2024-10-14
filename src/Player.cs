using Godot;

public partial class Player : CharacterBody2D
{
    [Export] public float speed_ { get; set; } = 100;
    [Export] protected AnimationPlayer animationPlayer_;
    [Export] protected PackedScene bullet_;
    [Export] protected Node2D bulletList_;
    [Export] protected Node2D shootLeftSpawn_;
    [Export] protected Node2D shootRightSpawn_;
    [Export] protected Node2D shootUpSpawn_;
    [Export] protected Node2D shootDownSpawn_;
    [Export] protected Node2D shootUpLeftSpawn_;
    [Export] protected Node2D shootUpRightSpawn_;
    [Export] protected Node2D shootDownLeftSpawn_;
    [Export] protected Node2D shootDownRightSpawn_;
    [Export] protected Sprite2D faceLeftSprite_;
    [Export] protected Sprite2D faceRightSprite_;
    [Export] protected Sprite2D faceUpSprite_;
    [Export] protected Sprite2D faceDownSprite_;
    [Export] Timer timer_;
    bool invincible_ = false;

    Node2D bulletSpawn_
    {
        get {
            if (facingDirection_ == Vector2.Left)
                return shootLeftSpawn_;
            else if (facingDirection_ == Vector2.Right)
                return shootRightSpawn_;
            else if (facingDirection_ == Vector2.Up)
                return shootUpSpawn_;
            else if (facingDirection_ == Vector2.Down)
                return shootDownSpawn_;
            else if (facingDirection_ == (Vector2.Up + Vector2.Left).Normalized())
                return shootUpLeftSpawn_;
            else if (facingDirection_ == (Vector2.Up + Vector2.Right).Normalized())
                return shootUpRightSpawn_;
            else if (facingDirection_ == (Vector2.Down + Vector2.Left).Normalized())
                return shootDownLeftSpawn_;
            else if (facingDirection_ == (Vector2.Down + Vector2.Right).Normalized())
                return shootDownRightSpawn_;
            else
                return shootLeftSpawn_;
        }
    }
    Vector2 facingDirection_ = Vector2.Left;
    Vector2 moveDirection_ = Vector2.Zero;
    bool lastAInput_ = false;
    bool canShoot_ = true;
    float shootCooldown_ = 0.4f;
    enum PlayerState
    {
        IDLE = 0,
        SHOOTING
    }

    public override void _Ready()
    {
        if (animationPlayer_ != null)
            animationPlayer_.Play("walk_left");
        if (timer_ != null)
            timer_.Timeout += OnTimerCooldown;
    }

    private void OnTimerCooldown()
    {
        canShoot_ = true;
    }

    public override void _Process(double delta)
    {
        // TODO Animation make player face direction they move and change sprite animation
        bool aPressed = Input.IsActionPressed("a");
        if (Input.IsActionPressed("up"))
        {
            moveDirection_.Y = -1;
            if (!aPressed)
                animationPlayer_.Play("walk_up");
        }
        else if (Input.IsActionPressed("down"))
        {
            moveDirection_.Y = 1;
            if (!aPressed)
                animationPlayer_.Play("walk_down");
        }
        else
        {
            moveDirection_.Y = 0;
        }
        if (Input.IsActionPressed("left"))
        {
            moveDirection_.X = -1;
            if (!aPressed)
                animationPlayer_.Play("walk_left");
        }
        else if (Input.IsActionPressed("right"))
        {
            moveDirection_.X = 1;
            if (!aPressed)
                animationPlayer_.Play("walk_right");
        }
        else
        {
            moveDirection_.X = 0;
        }
        moveDirection_ = moveDirection_.Normalized();
        if (!aPressed && moveDirection_ != Vector2.Zero)
            facingDirection_ = moveDirection_;
        if (moveDirection_ == Vector2.Zero)
        {
            animationPlayer_.Pause();
        }
        else
        {
            animationPlayer_.Play();
        }

        if (aPressed && canShoot_)
        {
            // shoot, set cooldown, lock facing direction
            Bullet bullet = bullet_.Instantiate<Bullet>();
            bulletList_.AddChild(bullet);
            bullet.GlobalPosition = bulletSpawn_.GlobalPosition;
            bullet.moveDirection_ = facingDirection_;
            timer_.Start();
            canShoot_ = false;
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

    #region Animation
    void DisableSprites()
    {
        faceLeftSprite_.Visible = false;
        faceRightSprite_.Visible = false;
        faceUpSprite_.Visible = false;
        faceDownSprite_.Visible = false;
    }

    void FaceRight()
    {
        DisableSprites();
        faceRightSprite_.Visible = true;
    }

    void FaceLeft()
    {
        DisableSprites();
        faceLeftSprite_.Visible = true;
    }

    void FaceUp()
    {
        DisableSprites();
        faceUpSprite_.Visible = true;
    }

    void FaceDown()
    {
        DisableSprites();
        faceDownSprite_.Visible = true;
    }

    void FaceUpRight()
    {
        // DisableSprites();
    }

    void FaceUpLeft()
    {
    }

    void FaceDownLeft()
    {

    }

    void FaceDownRight()
    {

    }
    #endregion
}
