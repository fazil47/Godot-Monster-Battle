using Godot;
using System;

public class EnemyAttack : EnemyState
{
    PlayerController playerController;

    public override void Enter(EnemyController enemy)
    {
        PlayAnimation(enemy, "attack");
        playerController = (PlayerController)(enemy.player);
    }

    public override void Update(EnemyController enemy, float delta)
    {

    }

    public override void PhysicsUpdate(EnemyController enemy, float delta)
    {
        enemy.monsterScript.ResetAnimation();

        if ((enemy.distanceToPlayer <= enemy.minDistanceToPlayer) && (enemy.meleeAttackTimer.TimeLeft == 0) && (enemy.evadeMode.TimeLeft == 0))
        {
            enemy.meleeAttackTimer.Start();
            Vector3 attackDirection = -enemy.localBackward;
            Node meleeAttack = enemy.monsterScript.meleeAttackNode.Instance();
            enemy.GetParent().AddChild(meleeAttack);
            ((Spatial)meleeAttack).GlobalTransform = enemy.monsterScript.meleeAttackOrigin.GlobalTransform;
            ((Spatial)meleeAttack).Rotation = enemy.Rotation;
            ((Spatial)meleeAttack).Scale = new Vector3(10, 10, 10);

            playerController.MeleeHit();
            enemy.attackMode.Stop();
            enemy._on_AttackMode_timeout();
        }
        else if ((enemy.rangedAttackTimer.TimeLeft == 0) && !(enemy.isPlayerInMeleeArea) && !(playerController.isDodging))
        {
            enemy.rangedAttackTimer.Start();

            Vector3 shootFrom = enemy.monsterScript.rangedAttackOrigin.GlobalTransform.origin;
            Vector3 shootTarget = ((MonsterScript)(enemy.player.GetNode("MonsterNode").GetChild(0))).rangedAttackTarget.GlobalTransform.origin;

            Vector3 shootDirection = shootTarget - shootFrom;
            Node rangedAttackProjectile = enemy.monsterScript.rangedAttackProjectile.Instance();
            enemy.GetParent().AddChild(rangedAttackProjectile);
            ((KinematicBody)rangedAttackProjectile).GlobalTransform = enemy.monsterScript.rangedAttackOrigin.GlobalTransform;
            ((KinematicBody)rangedAttackProjectile).Rotation = enemy.Rotation;
            ((RangedAttack)rangedAttackProjectile).direction = shootDirection;
            ((KinematicBody)rangedAttackProjectile).AddCollisionExceptionWith(enemy);
        }
        else
        {
            enemy.attackMode.Stop();
            enemy._on_AttackMode_timeout();
            enemy.TransitionToState(enemy.moveState);
        }
        // else
        // {
        //     enemy.TransitionToState(enemy.staggerState);
        // }
    }
}
