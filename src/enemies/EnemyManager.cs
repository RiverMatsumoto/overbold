using System;
using Godot;
public partial class EnemyManager : Node
{
    [Export] Timer timer_;
    [Export] PackedScene[] globalEnemyPool_;
    [Export] Player player_;
    [Export] Node2D sprites_;
    [Export] Node2D[] spawnPoints;

    public override void _Ready()
    {
        // tempory just spawn in middle of screen, eventually set up game and rounds
        Engine.MaxFps = 60;
        timer_.Start();
        SpawnEnemy();
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (globalEnemyPool_.Length <= 0)
            GD.PrintErr($"{Name}: Global enemy pool is empty");

        RandomNumberGenerator rng = new();
        Enemy enemy = globalEnemyPool_[rng.RandiRange(0, globalEnemyPool_.Length - 1)].Instantiate<Enemy>();
        enemy.Initialize(player_, timer_);
        Vector2 spawnPos = spawnPoints[rng.RandiRange(0, spawnPoints.Length - 1)].Position;
        GD.Print($"Spawn pos: {spawnPos}");
        enemy.Position = spawnPos;
        sprites_.CallDeferred("add_child", enemy);
    }
}
