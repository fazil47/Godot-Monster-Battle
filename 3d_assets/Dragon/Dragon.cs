using Godot;
using System;

public class Dragon : MonsterScript
{
    #region Exposed Variables
    [Export]
    float rollRotateAngle = Mathf.Pi / 16;
    [Export]
    float pitchRotateAngle = Mathf.Pi / 16;
    [Export]
    float angularAccelaration = 0.1f;
    #endregion

    private Vector3 originalRotation;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        originalRotation = GetNode<Spatial>("RootNode").Rotation;
        rangedAttackOrigin = GetNode<Position3D>("RootNode/skeletal/Skeleton/dragon_wings/RangedAttackOrigin");
        rangedAttackTarget = GetNode<Position3D>("RootNode/skeletal/Skeleton/dragon_wings/RangedAttackTarget");
        rangedAttackProjectile = GD.Load<PackedScene>("res://3d_assets/Dragon/rangedAttack/DragonRangedAttack.tscn");
        meleeAttackNode = GD.Load<PackedScene>("res://3d_assets/Dragon/meleeAttack/DragonMeleeAttack.tscn");
        meleeAttackOrigin = GetNode<Position3D>("MeleeAttackOrigin");
    }

    public override void HandleMotion(Vector3 direction, float delta, Vector3 localRight, Vector3 localBackward)
    {
        Vector3 newRotation = originalRotation;

        if (IsRight(direction, localRight))
        {
            // Console.WriteLine("IsRight - Dragon");
            newRotation = new Vector3(newRotation.x, newRotation.y, newRotation.z + rollRotateAngle);
        }
        else if (IsLeft(direction, localRight))
        {
            // Console.WriteLine("IsLeft - Dragon");
            newRotation = new Vector3(newRotation.x, newRotation.y, newRotation.z - rollRotateAngle);
        }
        else if (IsBackward(direction, localBackward))
        {
            // Console.WriteLine("IsBackward - Dragon");
            newRotation = new Vector3(newRotation.x - pitchRotateAngle, newRotation.y, newRotation.z);
        }
        else if (IsForward(direction, localBackward))
        {
            // Console.WriteLine("IsForward - Dragon");
            newRotation = new Vector3(newRotation.x + pitchRotateAngle, newRotation.y, newRotation.z);
        }
        else if (IsForwardLeft(direction, localRight, localBackward))
        {
            // Console.WriteLine("IsForwardLeft - Dragon");
            newRotation = new Vector3(newRotation.x + pitchRotateAngle, newRotation.y, newRotation.z - rollRotateAngle);
        }
        else if (IsForwardRight(direction, localRight, localBackward))
        {
            // Console.WriteLine("IsForwardRight - Dragon");
            newRotation = new Vector3(newRotation.x + pitchRotateAngle, newRotation.y, newRotation.z + rollRotateAngle);
        }
        else if (IsBackwardLeft(direction, localRight, localBackward))
        {
            // Console.WriteLine("IsBackwardLeft - Dragon");
            newRotation = new Vector3(newRotation.x - pitchRotateAngle, newRotation.y, newRotation.z - rollRotateAngle);
        }
        else if (IsBackwardRight(direction, localRight, localBackward))
        {
            // Console.WriteLine("IsBackwardRight - Dragon");
            newRotation = new Vector3(newRotation.x - pitchRotateAngle, newRotation.y, newRotation.z + rollRotateAngle);
        }

        GetNode<Spatial>("RootNode").Rotation = GetNode<Spatial>("RootNode").Rotation.LinearInterpolate(newRotation, angularAccelaration);
    }

    public override void ResetAnimation()
    {
        GetNode<Spatial>("RootNode").Rotation = GetNode<Spatial>("RootNode").Rotation.LinearInterpolate(originalRotation, angularAccelaration);
    }
}
