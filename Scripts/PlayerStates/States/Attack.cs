using Godot;
using System;

public class Attack : State
{
    public override void Enter(PlayerController player)
    {
        PlayAnimation(player, "attack");
        player.isDodging = false;
    }

    public override void Update(PlayerController player, float delta)
    {

    }

    public override void PhysicsUpdate(PlayerController player, float delta)
    {
        if (Input.IsActionJustPressed("attack") && player.rangedAttackTimer.TimeLeft == 0 && !player.hasPlayerWon && (player.currentStamina >= player.rangedAttackStamina))
        {
            player.currentStamina = Mathf.Clamp(player.currentStamina - player.rangedAttackStamina, 0, player.stamina);

            player.rangedAttackTimer.Start();

            Vector3 shootFrom = player.monsterScript.rangedAttackOrigin.GlobalTransform.origin;

            Vector3 shootTarget = ((MonsterScript)(player.enemy.GetNode("MonsterNode").GetChild(0))).rangedAttackTarget.GlobalTransform.origin;

            Vector3 shootDirection = shootTarget - shootFrom;
            Node rangedAttackProjectile = player.monsterScript.rangedAttackProjectile.Instance();
            player.GetParent().AddChild(rangedAttackProjectile);
            ((KinematicBody)rangedAttackProjectile).GlobalTransform = player.monsterScript.rangedAttackOrigin.GlobalTransform;
            ((KinematicBody)rangedAttackProjectile).Rotation = player.Rotation;
            ((KinematicBody)rangedAttackProjectile).Scale = new Vector3(5, 5, 5);
            ((RangedAttack)rangedAttackProjectile).direction = shootDirection;
            ((KinematicBody)rangedAttackProjectile).AddCollisionExceptionWith(player);
        }
        else if (Input.IsActionJustPressed("attack_alternate") && player.meleeAttackTimer.TimeLeft == 0 && !player.hasPlayerWon)
        {
            player.meleeAttackTimer.Start();

            Vector3 attackDirection = -player.localBackward;
            Node meleeAttack = player.monsterScript.meleeAttackNode.Instance();
            player.GetParent().AddChild(meleeAttack);
            ((Spatial)meleeAttack).GlobalTransform = player.monsterScript.meleeAttackOrigin.GlobalTransform;
            ((Spatial)meleeAttack).Rotation = player.Rotation;
            ((Spatial)meleeAttack).RotateY(Mathf.Pi);
            ((Spatial)meleeAttack).Scale = new Vector3(10, 10, 10);

            if (player.isEnemyInMeleeArea || (((EnemyController)(player.enemy)).distanceToPlayer < ((EnemyController)(player.enemy)).minDistanceToPlayer))
            {
                ((EnemyController)(player.enemy)).MeleeHit();
            }
        }
        else if (!(Input.IsActionPressed("attack") || Input.IsActionPressed("attack_alternate")))
        {
            player.TransitionToState(player.idleState);
        }
    }
}
