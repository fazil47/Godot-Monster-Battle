using Godot;
using System;

public abstract class MonsterScript : Spatial
{
    public Position3D rangedAttackOrigin;
    public PackedScene rangedAttackProjectile;
    public Position3D rangedAttackTarget;
    public Position3D meleeAttackOrigin;
    public PackedScene meleeAttackNode;

    public abstract void HandleMotion(Vector3 direction, float delta, Vector3 localRight, Vector3 localBackward);

    public abstract void ResetAnimation();

    // Checks if the direction of movement is towards left
    public bool IsLeft(Vector3 direction, Vector3 localRight)
    {
        return (direction.x == -localRight.x);
    }

    // Checks if the direction of movement is towards left
    public bool IsRight(Vector3 direction, Vector3 localRight)
    {
        return (direction.x == localRight.x);
    }

    // Checks if the direction of movement is towards left
    public bool IsForward(Vector3 direction, Vector3 localBackward)
    {
        return (direction.z == -localBackward.z);
    }

    // Checks if the direction of movement is towards left
    public bool IsBackward(Vector3 direction, Vector3 localBackward)
    {
        return (direction.z == localBackward.z);
    }

    public bool IsForwardLeft(Vector3 direction, Vector3 localRight, Vector3 localBackward)
    {
        return -(localBackward + localRight) == direction;
    }

    public bool IsForwardRight(Vector3 direction, Vector3 localRight, Vector3 localBackward)
    {
        return (-localBackward + localRight) == direction;
    }

    public bool IsBackwardLeft(Vector3 direction, Vector3 localRight, Vector3 localBackward)
    {
        return (localBackward - localRight) == direction;
    }

    public bool IsBackwardRight(Vector3 direction, Vector3 localRight, Vector3 localBackward)
    {
        return (localBackward + localRight) == direction;
    }
}
