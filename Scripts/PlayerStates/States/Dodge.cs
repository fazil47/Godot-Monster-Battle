using Godot;
using System;

public class Dodge : State
{
    public override void Enter(PlayerController player)
    {
        PlayAnimation(player, "idle");
    }

    public override void Update(PlayerController player, float delta)
    {

    }

    public override void PhysicsUpdate(PlayerController player, float delta)
    {
        Vector3 direction = Vector3.Zero;
        
        float dodgeSpeed;
        if (player.GetNode<Timer>("DodgeDurationTimer").TimeLeft == 0)
        {
            dodgeSpeed = player.dodgeSpeedMultiplier;
        }
        else
        {
            dodgeSpeed = 1;
        }

        if (Input.IsActionPressed("move_right"))
        {
            direction -= player.Transform.basis.x;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction += player.Transform.basis.x;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction -= player.Transform.basis.z;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction += player.Transform.basis.z;
        }

        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
        }
        else
        {
            player.TransitionToState(player.idleState);
        }

        player.velocity.x = direction.x * player.speed * delta * dodgeSpeed;
        player.velocity.z = direction.z * player.speed * delta * dodgeSpeed;
        // player.velocity.y = 0;

        // player.velocity.LinearInterpolate(direction * player.speed * delta, 1f);
        player.velocity = player.MoveAndSlide(player.velocity, Vector3.Up);
    }
}
