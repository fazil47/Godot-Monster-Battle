using Godot;
using System;

public class EnemyMove : EnemyState
{
    PlayerController playerController;
    RandomNumberGenerator randGen;

    public override void Enter(EnemyController enemy)
    {
        PlayAnimation(enemy, "move");
        playerController = (PlayerController)(enemy.player);
        randGen = new RandomNumberGenerator();
    }

    public override void Update(EnemyController enemy, float delta)
    {

    }

    public override void PhysicsUpdate(EnemyController enemy, float delta)
    {
        if (enemy.attackMode.TimeLeft != 0)
        {
            enemy.TransitionToState(enemy.attackState);
            enemy._PhysicsProcess(delta);
            return;
        }

        Vector3 direction = Vector3.Zero;

        randGen.Randomize();
        float dodgeProb = randGen.Randf();
        bool dodge = dodgeProb <= enemy.evadeDodgeProb; // Returns true if random number is less than 0.95, which has a 95% chance of happening
        if (enemy.isEvade && Input.IsActionJustPressed("attack") && dodge && enemy.GetNode<Timer>("DodgeCooldownTimer").TimeLeft == 0)
        {
            enemy.GetNode<Timer>("DodgeDurationTimer").Start();
        }

        float dodgeSpeed = 1;
        if (enemy.GetNode<Timer>("DodgeDurationTimer").TimeLeft > 0)
        {
            dodgeSpeed = enemy.dodgeSpeedMultiplier;
        }
        else
        {
            dodgeSpeed = 1;
        }

        if ((enemy.distanceToPlayer <= enemy.minDistanceToPlayer) && (!enemy.isEvade) && (enemy.rangedAttackTimer.TimeLeft == 0 && enemy.meleeAttackTimer.TimeLeft == 0))
        {
            enemy.TransitionToState(enemy.attackState);
        }
        else if (!(enemy.isEvade) && (enemy.distanceToPlayer > enemy.minDistanceToPlayer))
        {
            direction -= enemy.localBackward;
        }
        else if ((enemy.isEvade))
        {
            direction = enemy.localRight;
        }

        if (direction != Vector3.Zero)
        {
            enemy.monsterScript.HandleMotion(direction, delta, enemy.localRight, enemy.localBackward);
            direction = direction.Normalized();
        }

        enemy.velocity.x = direction.x * enemy.speed * delta * dodgeSpeed;
        enemy.velocity.z = direction.z * enemy.speed * delta * dodgeSpeed;
        enemy.velocity.y = -enemy.fallAccelaration * delta;

        enemy.velocity = enemy.MoveAndSlide(enemy.velocity, Vector3.Up);
    }
}
