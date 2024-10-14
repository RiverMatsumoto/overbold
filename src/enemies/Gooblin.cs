using Godot;

[GlobalClass]
public partial class Gooblin : EnemyBehavior
{
    Enemy enemy_;
    Player player_;
    Timer timer_;
    bool newWanderLocation_ = false;
    Vector2 wanderLocation_ = new Vector2(0, 0);
    bool newAggroChance_ = false;
    float aggroChance_ = 0.0f;
    Vector2 moveDirection_ = new Vector2(GD.RandRange(1, 1), GD.RandRange(1, 1)).Normalized();
    Tween tween_;
    double timePassed_ = 0.0f;
    readonly Vector2I WINDOW_SIZE = DisplayServer.WindowGetSize();
    

    public override void Initialize(Enemy enemy, Player player)
    {
        enemy_ = enemy;
        player_ = player;
        // Create timer, add it to enemy
        timer_ = new Timer
        {
            WaitTime = 1.0f,
            OneShot = false,
            Autostart = true
        };
        timer_.Timeout += newWanderLocation;
        enemy.AddChild(timer_);
        timer_.CallDeferred("start");
    }

    public override void Execute(double delta)
    {
        // wander random, random chance to walk towards player every 2 seconds
        // closer to player, higher chance to walk towards player
        timePassed_ += delta;
        if (newAggroChance_)
        {
            Vector2 playerPos = player_.GlobalPosition / WINDOW_SIZE;
            Vector2 enemyPos = enemy_.GlobalPosition / WINDOW_SIZE;
            // distance to player is 0 to 1, 1 being right on top of player
            float distanceToPlayer = 1 - enemyPos.DistanceTo(playerPos);
            // GD.Print(distanceToPlayer);
            aggroChance_ = new RandomNumberGenerator().Randf() + distanceToPlayer * 0.5f;
            newAggroChance_ = false;
            // GD.Print(aggroChance_);
        }
        if (timePassed_ > 1.0f)
        {
            timePassed_ = 0.0f;
            if (newWanderLocation_ && aggroChance_ < 0.5f)
            {
                // wander 
                // tween move direction by rotating it by a random amount
                var rng = new RandomNumberGenerator();
                // Vector2 randomDirection = new Vector2(rng.RandfRange(-3.14f, 3.14f), rng.RandfRange(-3.14f, 3.14f)).Normalized();
                Vector2 wanderSpherePos = enemy_.GlobalPosition;
                float radius = 200.0f;
                float randTheta = rng.RandfRange(0, 2 * 3.14f);
                float wanderX = wanderSpherePos.X + radius * Mathf.Cos(randTheta);
                float wanderY = wanderSpherePos.X + radius * Mathf.Sin(randTheta);
                wanderLocation_ = new Vector2(wanderX, wanderY);
                // lean the circle towards the center of the screen depending on how close to edge of screen the enemy is
                wanderSpherePos = wanderSpherePos / WINDOW_SIZE;
                // 0.5 is the center of the screen
                Vector2 screenCenter = new Vector2(0.5f, 0.5f);
                // percent of screen away from center, direction, multiply by window_size to move the sphere center
                Vector2 direction = (screenCenter - wanderSpherePos).Normalized();
                Vector2 magnitude = (screenCenter - wanderSpherePos).Abs();
                GD.Print($"Direction: {direction}");
                GD.Print($"Magnitude: {magnitude}");
                wanderLocation_ = wanderLocation_ + (magnitude / screenCenter * direction * radius);
                enemy_.wanderSpherePos_ = wanderSpherePos + magnitude / screenCenter * direction * radius;
                enemy_.wanderSphereRadius_ = radius;
                enemy_.wanderSpherePosition_ = enemy_.ToLocal(wanderLocation_);
                moveDirection_ = enemy_.GlobalPosition.DirectionTo(wanderLocation_).Normalized();
                GD.Print($"Wander location: {wanderLocation_}");
            }
            newWanderLocation_ = false;
        }
        if (aggroChance_ > 0.5f)
            enemy_.Velocity = enemy_.GlobalPosition.DirectionTo(player_.GlobalPosition) * 100;
        else
            enemy_.Velocity = moveDirection_ * 100;
        enemy_.MoveAndSlide();
    }

    public override void OnDeath(Enemy enemy)
    {
        GD.Print("Gooblin died");
        enemy.QueueFree();
    }

    public override void OnGlobalTick()
    {
        newAggroChance_ = true;
    }

    void newWanderLocation()
    {
        timer_.WaitTime = new RandomNumberGenerator().RandfRange(0.5f, 2.0f);
        newWanderLocation_ = true;
        GD.Print("new wander location");
    }
}
