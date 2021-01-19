using Godot;
using System;

public class EnemyDodge : EnemyState
{
    public override void Enter(EnemyController enemy)
    {
        PlayAnimation(enemy, "idle");
    }

    public override void Update(EnemyController enemy, float delta)
    {

    }

    public override void PhysicsUpdate(EnemyController enemy, float delta)
    {
        // Vector3 direction = Vector3.Zero;
        // float dodgeSpeed = enemy.DodgeSpeedMultiplier;

        // if (Input.IsActionPressed("move_right"))
        // {
        //     direction -= enemy.Transform.basis.x;
        // }
        // if (Input.IsActionPressed("move_left"))
        // {
        //     direction += enemy.Transform.basis.x;
        // }
        // if (Input.IsActionPressed("move_back"))
        // {
        //     direction -= enemy.Transform.basis.z;
        // }
        // if (Input.IsActionPressed("move_forward"))
        // {
        //     direction += enemy.Transform.basis.z;
        // }

        // if (direction != Vector3.Zero)
        // {
        //     direction = direction.Normalized();
        // }
        // else
        // {
        //     enemy.TransitionToState(enemy.idleState);
        // }

        // enemy.velocity.x = direction.x * enemy.speed * delta * dodgeSpeed;
        // enemy.velocity.z = direction.z * enemy.speed * delta * dodgeSpeed;
        // // enemy.velocity.y = 0;

        // // enemy.velocity.LinearInterpolate(direction * enemy.speed * delta, 1f);
        // enemy.velocity = enemy.MoveAndSlide(enemy.velocity, Vector3.Up);
    }
}
