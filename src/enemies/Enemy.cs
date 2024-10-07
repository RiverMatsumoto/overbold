using Godot;

public partial class Enemy : Player
{
    [Export] public EnemyBehavior enemyBehavior_;
    [Export] Player player_;
    Timer tick_;

    public void Initialize(Player player, Timer tick)
    {
        player_ = player;
        tick_ = tick;
        enemyBehavior_ = (EnemyBehavior)enemyBehavior_.Duplicate();
        tick_.Timeout += enemyBehavior_.OnGlobalTick;
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
}
