using Godot;

public partial class Enemy : Player
{
    [Export] public EnemyBehavior enemyBehavior_;
    [Export] Player player_;
    Timer tick_;
    [Export] Area2D hurtbox_;
    [Export] bool debug_ = false;
    public Vector2 wanderSpherePos_ = new Vector2();
    public Vector2 wanderSpherePosition_ = new Vector2();
    public float wanderSphereRadius_ = 0.0f;

    public void Initialize(Player player, Timer tick)
    {
        player_ = player;
        tick_ = tick;
        enemyBehavior_ = (EnemyBehavior)enemyBehavior_.Duplicate();
        enemyBehavior_.Initialize(this, player_);
        tick_.Timeout += enemyBehavior_.OnGlobalTick;
        hurtbox_.AreaEntered += OnHurtboxAreaEntered;
    }

    // TODO extract similar functionality for player and enemy to clean up
    public override void _Ready()
    {
        // animationPlayer_.Play("walk");
    }

    public override void _ExitTree()
    {
        tick_.Timeout -= enemyBehavior_.OnGlobalTick;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        enemyBehavior_.Execute(delta);
        if (debug_)
        {
            QueueRedraw();
        }
    }

    public void TakeDamage(int damage, Vector2 knockback = new Vector2())
    {
        enemyBehavior_.Health -= damage;
        animationPlayer_.Play("hurt");
        animationPlayer_.Queue("walk");
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "global_position", GlobalPosition + knockback, 0.15f).SetTrans(Tween.TransitionType.Quad).SetEase(Tween.EaseType.Out);
        tween.Play();
        if (enemyBehavior_.Health <= 0)
        {
            enemyBehavior_.OnDeath(this);
        }
    }
    
    public void OnHurtboxAreaEntered(Area2D area)
    {
        // if (area.Name == "Bullet")
        // {
        //     animationPlayer_.Play("hurt");
        //     animationPlayer_.Queue("walk");
            // TakeDamage(1);
        // }
    }

    public override void _Draw()
    {
        base._Draw();
        DrawCircle(wanderSpherePos_ / GlobalScale, wanderSphereRadius_ / GlobalScale.X, new Color(1, 0, 0, 0.2f));
        DrawCircle(wanderSpherePosition_ / GlobalScale, 5f, new Color(0, 0, 1, 0.2f));
    }
}
