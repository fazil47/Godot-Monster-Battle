using Godot;
using System;

public class RangedAttack : KinematicBody
{
    const float PROJECTILE_VELOCITY = 2f;

    float timeAlive = 5;
    public Vector3 direction = new Vector3();
    bool hit = false;

    AnimationPlayer animationPlayer;
    CollisionShape collisionShape;

    AudioStreamPlayer3D movementAudio;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        collisionShape = GetNode<CollisionShape>("CollisionShape");

        movementAudio = GetNode<AudioStreamPlayer3D>("MovementAudio");
        movementAudio.Playing = true;
    }

    public override void _Process(float delta)
    {
        if (!GetNode<BattleController>("/root/Battle").hasPlayerWon && !GetNode<BattleController>("/root/Battle").hasPlayerLost)
        {
            if (hit)
            {
                return;
            }

            timeAlive -= delta;
            if (timeAlive < 0)
            {
                hit = true;
                animationPlayer.Play("explode");
            }
            KinematicCollision col = MoveAndCollide(delta * direction * PROJECTILE_VELOCITY);
            if (col != null)
            {
                if (col.Collider.HasMethod("RangedHit"))
                {
                    try
                    {
                        ((PlayerController)(col.Collider)).RangedHit();
                    }
                    catch
                    {
                        ((EnemyController)(col.Collider)).RangedHit();
                    }
                }
                collisionShape.Disabled = true;
                movementAudio.QueueFree();
                animationPlayer.Play("explode");
                hit = true;
            }
        }
        else
        {
            QueueFree();
        }
    }
}
