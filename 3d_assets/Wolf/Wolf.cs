using Godot;
using System;

public class Wolf : MonsterScript
{
    [Export]
    float angularAccelaration = 0.1f;
    [Export]
    float rotateAngle = Mathf.Pi / 2;
    private Vector3 originalRotation;

    public override void _Ready()
    {
        originalRotation = GetChild<Spatial>(0).Rotation;
        rangedAttackOrigin = GetNode<Position3D>("RangedAttackOrigin");
        rangedAttackTarget = GetNode<Position3D>("RangedAttackTarget");
        rangedAttackProjectile = GD.Load<PackedScene>("res://3d_assets/Wolf/rangedAttack/WolfRangedAttack.tscn");
        meleeAttackNode = GD.Load<PackedScene>("res://3d_assets/Wolf/meleeAttack/WolfMeleeAttack.tscn");
        meleeAttackOrigin = GetNode<Position3D>("MeleeAttackOrigin");
    }

    public override void HandleMotion(Vector3 direction, float delta, Vector3 localRight, Vector3 localBackward)
    {
        float yRotation = 0;

        if (IsRight(direction, localRight))
        {
            yRotation = -rotateAngle;
        }
        else if (IsLeft(direction, localRight))
        {
            yRotation = rotateAngle;
        }
        else if (IsBackward(direction, localBackward))
        {
            yRotation = 2 * rotateAngle;
        }
        else if (IsForward(direction, localBackward))
        {
            yRotation = 0;
        }
        else if (IsForwardLeft(direction, localRight, localBackward))
        {
            yRotation = rotateAngle / 2;
        }
        else if (IsForwardRight(direction, localRight, localBackward))
        {
            yRotation = -rotateAngle / 2;
        }
        else if (IsBackwardLeft(direction, localRight, localBackward))
        {
            yRotation = (3 / 2) * rotateAngle;
        }
        else if (IsBackwardRight(direction, localRight, localBackward))
        {
            yRotation = -(3 / 2) * rotateAngle;
        }

        Vector3 newRotation = new Vector3(originalRotation.x, originalRotation.y + yRotation, originalRotation.z);
        GetChild<Spatial>(0).Rotation = GetChild<Spatial>(0).Rotation.LinearInterpolate(newRotation, angularAccelaration);
    }

    public override void ResetAnimation()
    {
        GetChild<Spatial>(0).Rotation = GetChild<Spatial>(0).Rotation.LinearInterpolate(originalRotation, angularAccelaration);
    }

    //     public override void _PhysicsProcess(float delta)
    //     {
    //         bool flag = false;
    //         float yRotation = 0;
    //         if (Input.IsActionPressed("move_right"))
    //         {
    //             flag = true;
    //             yRotation -= rotateAngle;
    //         }
    //         else if (Input.IsActionPressed("move_left"))
    //         {
    //             flag = true;
    //             yRotation += rotateAngle;
    //         }
    //         if (Input.IsActionPressed("move_back"))
    //         {
    //             if (flag)
    //             {
    //                 if (yRotation < 0)
    //                 {
    //                     yRotation = (yRotation - (2 * rotateAngle)) / 2;
    //                 }
    //                 else
    //                 {
    //                     yRotation = (yRotation + (2 * rotateAngle)) / 2;
    //                 }
    //             }
    //             else
    //             {
    //                 flag = true;
    //                 yRotation = 2 * rotateAngle;
    //             }
    //         }
    //         else if (Input.IsActionPressed("move_forward"))
    //         {
    //             if (flag)
    //             {
    //                 yRotation = (0 + yRotation) / 2;
    //             }
    //             else
    //             {
    //                 flag = true;
    //                 yRotation = 0;
    //             }
    //         }

    //         if (flag && !Input.IsActionPressed("attack"))
    //         {
    //             if (yRotation == (3 * (Mathf.Pi / 2)))
    //             {
    //                 yRotation = -1 * (yRotation - Mathf.Pi);
    //             }
    //             Vector3 newRotation = new Vector3(originalRotation.x, originalRotation.y + yRotation, originalRotation.z);
    //             GetChild<Spatial>(0).Rotation = GetChild<Spatial>(0).Rotation.LinearInterpolate(newRotation, angularAccelaration);
    //         }
    //         else
    //         {
    //             GetChild<Spatial>(0).Rotation = GetChild<Spatial>(0).Rotation.LinearInterpolate(originalRotation, angularAccelaration);
    //         }
    //     }
}
