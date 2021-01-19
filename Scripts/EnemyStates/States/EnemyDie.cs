using Godot;
using System;

public class EnemyDie : EnemyState
{
    public override void Enter(EnemyController enemy)
    {
        StopAnimation(enemy);
    }

    public override void Update(EnemyController enemy, float delta)
    {

    }

    public override void PhysicsUpdate(EnemyController enemy, float delta)
    {

    }
}
