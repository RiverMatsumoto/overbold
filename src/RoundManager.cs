using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class RoundManager : Node
{
    [Export] EnemyManager enemyManager_;
    [Export] RichTextLabel pointsEarnedLabel_;
    [Export] RichTextLabel enemiesLeftLabel_;
    [Export] Control upgradeScreen_;
    [Export] Timer enemySpawnTimer_;
    public Dictionary<string, int> enemyCounts_ = new();
    public int currentRound_ = 1;
    public int maxEnemies_ = 10;
    public int totalEnemiesNotKilledYet_ = 0;
    public int totalDeadEnemies_ = 0;
    int pointsEarned_ = 0;
    bool waitingForRoundEndInput_ = false;

    public void SetEnemyCount(string enemyName, int count)
    {
        if (enemyCounts_.ContainsKey(enemyName))
        {
            enemyCounts_[enemyName] = count;
        }
    }

    public int GetEnemyCount(string enemyName)
    {
        if (enemyCounts_.ContainsKey(enemyName))
        {
            return enemyCounts_[enemyName];
        }
        return 0;
    }

    public override void _Ready()
    {
        for (int i = 0; i < enemyManager_.GlobalEnemyPool.Length; i++)
        {
            enemyCounts_.Add(enemyManager_.GlobalEnemyPool[i].ResourcePath.GetFile().GetBaseName(), 0);
            GD.Print(enemyManager_.GlobalEnemyPool[i].ResourcePath.GetFile().GetBaseName());
        }
        enemyManager_.EnemyDied += OnEnemyDied;
        SetEnemyCount("Gooblin", 2);
        enemiesLeftLabel_.Text = $"Enemies Left: {enemyCounts_.Values.Sum()}";
        StartRound();
    }
    
    public void OnEnemyDied()
    {
        // update ui
        totalDeadEnemies_++;
        if (enemiesLeftLabel_ != null)
        {
            enemiesLeftLabel_.Text = $"Enemies Left: {totalEnemiesNotKilledYet_ - totalDeadEnemies_}";
        }

        // check round ended
        if (enemyCounts_.Values.Sum() <= 0)
        {
            EndRound();
        }
    }

    public void SpawnNewRandomEnemy()
    {
        // spawn a random enemy that still needs to be spawned
        RandomNumberGenerator rng = new();
        KeyValuePair<string, int>[] enemies = enemyCounts_.Where(x => x.Value > 0).ToArray();
        if (enemies.Length >= 0)
        {
            GD.Print(enemies[0].Key);
            GD.Print(enemies[0].Value);
            int randomEnemy = rng.RandiRange(0, enemies.Length - 1);
            string enemyName = enemies[randomEnemy].Key;
            enemyManager_.SpawnEnemy(enemyName);
            enemyCounts_[enemyName]--;
        }
        else
        {
            GD.Print("No enemies to spawn");
        }
    }

    public void StartRound()
    {
        // put on a timer to spawn enemies with a max enemy count
        totalDeadEnemies_ = 0;
        totalEnemiesNotKilledYet_ = enemyCounts_.Values.Sum();
        enemySpawnTimer_.Timeout += SpawnNewRandomEnemy;
        enemySpawnTimer_.Start();
    }

    public void EndRound()
    {
        enemySpawnTimer_.Timeout -= SpawnNewRandomEnemy;
        GD.Print("Round ended");
        totalDeadEnemies_ = totalEnemiesNotKilledYet_;
        enemiesLeftLabel_.Text = $"Enemies Left: {totalEnemiesNotKilledYet_ - totalDeadEnemies_}";
        
        // show round end text, wait for player to press a button to start next round
        pointsEarnedLabel_.Show();
        Tween tween = CreateTween();
        tween.TweenMethod(new Callable(this, nameof(SetPointsEarnedText)), 0, 0, 0.0f);
        tween.TweenInterval(1.0f);
        tween.TweenMethod(new Callable(this, nameof(SetPointsEarnedText)), 0, 100, 2.5f);
        tween.TweenInterval(2.5f);
        tween.TweenProperty(this, "waitingForRoundEndInput_", true, 0.0f);
        tween.Play();
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("a") && waitingForRoundEndInput_)
        {
            waitingForRoundEndInput_ = false;
            GoToUpgradeScreen();
        }
    }

    public void SetPointsEarnedText(int points)
    {
        pointsEarnedLabel_.Text = $"Points Earned: {points}";
    }

    public void GoToUpgradeScreen()
    {
        GD.Print("Show upgrade screen, ");
        pointsEarnedLabel_.Visible = false;
        upgradeScreen_.Visible = true;
    }
}
