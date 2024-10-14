using System.Linq;
using Godot;
public partial class EnemyManager : Node
{
    [Signal] public delegate void EnemyDiedEventHandler();
    [Export] public Node2D entities_;
    [Export] Timer timer_;
    [Export] PackedScene[] globalEnemyPool_;
    [Export] Player player_;
    [Export] Node2D[] spawnPoints;

    public PackedScene[] GlobalEnemyPool { get => globalEnemyPool_; }

    public override void _Ready()
    {
        // tempory just spawn in middle of screen, eventually set up game and rounds
        Engine.MaxFps = 60;
        timer_.Start();
        // CallDeferred("SpawnEnemy");
        // CallDeferred("SpawnEnemy");
    }

    public void SpawnEnemy()
    {
        if (globalEnemyPool_.Length <= 0)
        {
            GD.PrintErr($"{Name}: Global enemy pool is empty");
            return;
        }

        RandomNumberGenerator rng = new();
        Enemy enemy = globalEnemyPool_[rng.RandiRange(0, globalEnemyPool_.Length - 1)].Instantiate<Enemy>();
        enemy.Initialize(player_, timer_);
        enemy.TreeExited += () => { EmitSignal(nameof(EnemyDied)); };
        Vector2 spawnPos = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)].Position;
        GD.Print($"Spawn pos: {spawnPos}");
        enemy.Position = spawnPos;
        entities_.AddChild(enemy);
    }
    
    public void SpawnEnemy(string enemyName)
    {
        if (globalEnemyPool_.Length <= 0)
        {
            GD.PrintErr($"{Name}: Global enemy pool is empty");
            return;
        }

        RandomNumberGenerator rng = new();
        // get enemy with specific name
        Enemy enemy = globalEnemyPool_.First(x => x.ResourcePath.GetFile().GetBaseName() == enemyName).Instantiate<Enemy>();
        enemy.Initialize(player_, timer_);
        enemy.TreeExited += () => { EmitSignal(nameof(EnemyDied)); };
        Vector2 spawnPos = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)].Position;
        GD.Print($"Spawn pos: {spawnPos}");
        enemy.Position = spawnPos;
        entities_.AddChild(enemy);
    }

    public void KillAllEnemies()
    {
        foreach (Node2D child in entities_.GetChildren())
        {
            if (child is Enemy)
                child.QueueFree();
        }
    }
}
