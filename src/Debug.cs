using Godot;

public partial class Debug : Control
{
	[Export] bool debug_ = false;
    [Export] EnemyManager enemyManager_;
    [Export] RoundManager roundManager_;
    [Export] Button killAllEnemiesButton_;

	public override void _Ready()
	{
        if (debug_)
        {
            Visible = true;
            killAllEnemiesButton_.Pressed += OnKillAllEnemiesButtonPressed;
        }
        else
        {
            Visible = false;
        }
	}

    public void OnKillAllEnemiesButtonPressed()
    {
        enemyManager_.KillAllEnemies();
        foreach (string enemy in roundManager_.enemyCounts_.Keys)
        {
            roundManager_.SetEnemyCount(enemy, 0);
        }
    }
}
