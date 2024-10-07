using Godot;

[GlobalClass]
public partial class Gooblin : EnemyBehavior
{
    bool newAggroChance = false;
    float aggroChance = 0.0f;
    
    public override void Execute(double delta, Enemy enemy, Player player)
    {
        // wander random, random chance to walk towards player every 2 seconds
        // closer to player, higher chance to walk towards player
        if (newAggroChance)
        {
            Vector2I windowSize = DisplayServer.WindowGetSize();
            Vector2 playerPos = player.GlobalPosition / windowSize;
            Vector2 enemyPos = enemy.GlobalPosition / windowSize;
            float distanceToPlayer = enemyPos.DistanceTo(playerPos);
            aggroChance = new RandomNumberGenerator().Randf() / 10.0f * (1 + (1 - distanceToPlayer));
            GD.Print($"Aggro chance: {aggroChance} also gooblin");
            newAggroChance = false;
        }
        
    }

    public override void OnDeath(Enemy enemy)
    {
        GD.Print("Gooblin died");
    }

    public override void OnGlobalTick()
    {
        newAggroChance = true;
        GD.Print("new aggro chance");
    }
}
