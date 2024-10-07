using Godot;

public abstract partial class EnemyBehavior : Resource
{
    // movement, attacks, special stuff. Needs to be virtual to create instance of enemybehaviors
    public abstract void Execute(double delta, Enemy enemy, Player player);
    public abstract void OnDeath(Enemy enemy);
    public abstract void OnGlobalTick();
}
