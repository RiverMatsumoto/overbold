using Godot;

public abstract partial class EnemyBehavior : Resource
{
    // movement, attacks, special stuff. Needs to be virtual to create instance of enemybehaviors
    public abstract void Initialize(Enemy enemy, Player player);
    public abstract void Execute(double delta);
    public abstract void OnDeath(Enemy enemy);
    public abstract void OnGlobalTick();
    [Export] public int Health { get; set; }
}
