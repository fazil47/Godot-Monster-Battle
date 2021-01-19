using Godot;
using System;

public class Stagger : State
{
    public Vector3 forceDirection;
    Timer staggerTimer;

    public override void Enter(PlayerController player)
    {
        StopAnimation(player);
        forceDirection = -((EnemyController)(player.enemy)).localBackward;
        staggerTimer = player.GetNode<Timer>("StaggerTimer");
        staggerTimer.Start();
    }

    public override void Update(PlayerController player, float delta)
    {

    }

    public override void PhysicsUpdate(PlayerController player, float delta)
    {
        if (staggerTimer.TimeLeft != 0)
        {
            player.velocity.x = forceDirection.x * player.speed * delta * player.staggerVelocityMultiplier;
            player.velocity.z = forceDirection.z * player.speed * delta * player.staggerVelocityMultiplier;
            player.velocity.y = -player.fallAccelaration * delta;
            player.velocity = player.MoveAndSlide(player.velocity, Vector3.Up);
        }
        else
        {
            player.TransitionToState(player.idleState);
        }
    }
}
