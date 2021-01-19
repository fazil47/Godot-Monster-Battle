using Godot;
using System;

public class MeleeAttack : Spatial
{
    float timeAlive = 0.5f;

    AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        // collisionShape = GetNode<CollisionShape>("CollisionShape");
        animationPlayer.Play("bite");
    }

    public override void _Process(float delta)
    {
        timeAlive -= delta;
        // MoveAndSlide(delta * direction * PROJECTILE_VELOCITY);
        if (timeAlive <= 0)
        {
            QueueFree();
        }
    }
}
