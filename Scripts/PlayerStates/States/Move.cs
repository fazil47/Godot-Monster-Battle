using Godot;
using System;

public class Move : State
{
    public override void Enter(PlayerController player)
    {
        PlayAnimation(player, "move");
    }

    public override void Update(PlayerController player, float delta)
    {

    }

    public override void PhysicsUpdate(PlayerController player, float delta)
    {
        Vector3 direction = Vector3.Zero;

        float dodgeSpeed;
        if (player.GetNode<Timer>("DodgeDurationTimer").TimeLeft > 0)
        {
            dodgeSpeed = player.dodgeSpeedMultiplier;
            player.isDodging = true;
        }
        else
        {
            player.isDodging = false;
            dodgeSpeed = 1;
        }

        if (Input.IsActionPressed("move_right"))
        {
            direction += player.localRight;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction -= player.localRight;
        }
        if (Input.IsActionPressed("move_back"))
        {

            direction += player.localBackward;
        }
        if (Input.IsActionPressed("move_forward"))
        {

            direction -= player.localBackward;
        }

        if (direction != Vector3.Zero && !player.hasPlayerWon)
        {
            player.monsterScript.HandleMotion(direction, delta, player.localRight, player.localBackward);
            direction = direction.Normalized();
        }
        else
        {
            direction = Vector3.Zero;
            player.monsterScript.ResetAnimation();
            player.TransitionToState(player.idleState);
        }

        if (Input.IsActionJustPressed("dodge") && player.GetNode<Timer>("DodgeCooldownTimer").TimeLeft == 0)
        {
            player.currentStamina = Mathf.Clamp(player.currentStamina - player.dodgeStamina, 0, player.stamina);
            player.GetNode<Timer>("DodgeDurationTimer").Start();
        }
        else if (Input.IsActionJustReleased("dodge"))
        {
            player.GetNode<Timer>("DodgeCooldownTimer").Start();
        }

        player.velocity.x = direction.x * player.speed * delta * dodgeSpeed;
        player.velocity.z = direction.z * player.speed * delta * dodgeSpeed;
        player.velocity.y = -player.fallAccelaration * delta;

        // player.velocity.LinearInterpolate(direction * player.speed * delta, 1f);
        player.velocity = player.MoveAndSlide(player.velocity, Vector3.Up);
        // player.MoveAndCollide(player.velocity);
    }
}
