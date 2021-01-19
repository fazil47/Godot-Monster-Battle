using Godot;
using System;

public class EnemyStagger : EnemyState
{
    Vector3 forceDirection;
    Timer staggerTimer;
    RandomNumberGenerator randGen;

    public override void Enter(EnemyController enemy)
    {
        StopAnimation(enemy);
        forceDirection = -((PlayerController)(enemy.player)).localBackward;
        staggerTimer = enemy.GetNode<Timer>("StaggerTimer");
        staggerTimer.Start();
        randGen = new RandomNumberGenerator();
    }

    public override void Update(EnemyController enemy, float delta)
    {

    }

    public override void PhysicsUpdate(EnemyController enemy, float delta)
    {
        if (staggerTimer.TimeLeft != 0)
        {
            enemy.velocity.x = forceDirection.x * enemy.speed * delta * enemy.staggerVelocityMultiplier;
            enemy.velocity.z = forceDirection.z * enemy.speed * delta * enemy.staggerVelocityMultiplier;
            enemy.velocity.y = -enemy.fallAccelaration * delta;
            enemy.velocity = enemy.MoveAndSlide(enemy.velocity, Vector3.Up);
        }
        else
        {

            enemy.TransitionToState(enemy.moveState);
        }
    }
}
