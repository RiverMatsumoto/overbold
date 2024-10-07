using Godot;
using System;

[GlobalClass]
public partial class Gnome : EnemyBehavior
{

    public override void Execute(double delta, Enemy enemy, Player player)
    {
        
    }

    public override void OnDeath(Enemy enemy)
    {
        throw new NotImplementedException();
    }

    public override void OnGlobalTick()
    {
        throw new NotImplementedException();
    }

}
