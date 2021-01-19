using Godot;
using System;

public class EnemyIdle : EnemyState
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

        // enemy.monsterController.ResetAnimation();

        // PlayerController playerController = (PlayerController)(enemy.player);
        // if ((playerController.CurrentState == playerController.moveState) && (enemy.distanceToPlayer > enemy.minDistanceToPlayer))
        // {
        //     // direction -= enemy.localBackward;
        //     enemy.TransitionToState(enemy.moveState);
        // }
        // else if ((playerController.CurrentState == playerController.moveState) && (enemy.distanceToPlayer <= enemy.minDistanceToPlayer))
        // {
        //     // direction -= enemy.localRight;
        //     // enemy.TransitionToState(enemy.moveState);
        //     enemy.TransitionToState(enemy.attackState);
        // }
        // else if ((playerController.CurrentState == playerController.attackState) && (enemy.distanceToPlayer > enemy.minDistanceToPlayer))
        // {
        //     // direction = enemy.localRight - enemy.localBackward;
        //     enemy.TransitionToState(enemy.moveState);
        // }
        // else if ((playerController.CurrentState == playerController.attackState) && (enemy.distanceToPlayer <= enemy.minDistanceToPlayer))
        // {
        //     // direction = enemy.localRight;
        //     enemy.TransitionToState(enemy.moveState);
        // }
        // else if (playerController.CurrentState == playerController.idleState)
        // {
        //     enemy.TransitionToState(enemy.attackState);
        // }

        // if (Input.IsActionPressed("move_right"))
        // {
        //     direction += enemy.localRight;
        // }
        // if (Input.IsActionPressed("move_left"))
        // {
        //     direction -= enemy.localRight;
        // }
        // if (Input.IsActionPressed("move_back"))
        // {
        //     direction += enemy.localBackward;
        // }
        // if (Input.IsActionPressed("move_forward"))
        // {
        //     direction -= enemy.localBackward;
        // }

        // if (direction != Vector3.Zero)
        // {
        //     enemy.TransitionToState(enemy.moveState);
        //     direction = direction.Normalized();
        // }

        // enemy.velocity.x = direction.x * enemy.speed * delta;
        // enemy.velocity.z = direction.z * enemy.speed * delta;
        // enemy.velocity.y = -enemy.fallAccelaration * delta;

        // enemy.velocity = enemy.MoveAndSlide(enemy.velocity, Vector3.Up);
    }
}
