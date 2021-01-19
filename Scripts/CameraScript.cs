using Godot;
using System;

public class CameraScript : Spatial
{
    [Export]
    float maxZoom = 3f;
    [Export]
    float minZoom = 0.5f;
    [Export]
    public float zoomSpeed = 0.09f;
    [Export]
    float mouseSensitivity = 0.005f;
    [Export]
    float moveRightAngle = -Mathf.Pi / 5;
    [Export]
    float moveRightSpeed = 0.1f;

    float zoom = 1.5f;

    Spatial innerGimbal;

    Vector3 originalRotation;

    public override void _Ready()
    {
        innerGimbal = GetNode<Spatial>("InnerGimbal");
        Input.SetMouseMode(Input.MouseMode.Captured);
        originalRotation = this.Rotation;
    }

    public override void _Process(float delta)
    {
        if (GetNode<BattleController>("/root/Battle").hasPlayerWon)
        {
            float innerGimbalXRotation = innerGimbal.Rotation.x;
            innerGimbal.Rotation = new Vector3(Mathf.Clamp(innerGimbalXRotation, 0f, 1f), innerGimbal.Rotation.y, innerGimbal.Rotation.z);
            Scale = Scale.LinearInterpolate(Vector3.One * zoom, zoomSpeed);
        }
        else if (Input.IsActionPressed("move_right"))
        {
            Rotation = Rotation.LinearInterpolate(new Vector3(originalRotation.x, originalRotation.y + moveRightAngle, originalRotation.z), moveRightSpeed);
            // RotateObjectLocal(Vector3.Up, moveRightAngle);
        }
        else
        {
            Rotation = Rotation.LinearInterpolate(originalRotation, moveRightSpeed);
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (GetNode<BattleController>("/root/Battle").hasPlayerWon)
        {
            if (@event is InputEventMouseMotion)
            {
                if (((InputEventMouseMotion)@event).Relative.x != 0)
                {
                    RotateObjectLocal(Vector3.Up, ((InputEventMouseMotion)@event).Relative.x * -1f * mouseSensitivity);
                }
                if (((InputEventMouseMotion)@event).Relative.y != 0)
                {
                    float yRotation = Mathf.Clamp(((InputEventMouseMotion)@event).Relative.y, -30, 30);
                    innerGimbal.RotateObjectLocal(Vector3.Right, yRotation * mouseSensitivity);
                }
            }

            if (@event.IsActionPressed("cam_zoom_in"))
            {
                zoom -= zoomSpeed;
            }
            else if (@event.IsActionPressed("cam_zoom_out"))
            {
                zoom += zoomSpeed;
            }
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }
    }
}
