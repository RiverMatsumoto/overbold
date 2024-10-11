using Godot;

public partial class Enemy : Player
{
    [Export] public EnemyBehavior enemyBehavior_;
    [Export] Player player_;
    Timer tick_;
    [Export] Area2D hurtbox_;

    public void Initialize(Player player, Timer tick)
    {
        player_ = player;
        tick_ = tick;
        enemyBehavior_ = (EnemyBehavior)enemyBehavior_.Duplicate();
        tick_.Timeout += enemyBehavior_.OnGlobalTick;
        hurtbox_.AreaEntered += OnHurtboxAreaEntered;
    }

    public override void _ExitTree()
    {
        tick_.Timeout -= enemyBehavior_.OnGlobalTick;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        enemyBehavior_.Execute(delta, this, player_);
    }

    public void TakeDamage(int damage)
    {
        GD.Print(enemyBehavior_.Health);
        enemyBehavior_.Health -= damage;
        if (enemyBehavior_.Health <= 0)
        {
            enemyBehavior_.OnDeath(this);
        }
    }
    
    public void OnHurtboxAreaEntered(Area2D area)
    {
        if (area.Name == "Bullet")
        {
            animationPlayer_.Play("hurt");
            animationPlayer_.Queue("walk");
            TakeDamage(1);
        }
    }
}
