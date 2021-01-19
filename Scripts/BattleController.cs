using Godot;
using System;

public class BattleController : Spatial
{
    [Export]
    public PackedScene defeatSmoke;
    [Export]
    AudioStream playerWinBackgroundMusic;
    [Export]
    float lookAtSpeed = 0.1f;

    Node player;
    Node enemy;
    public bool hasPlayerWon = false;
    public bool hasPlayerLost = false;

    ProgressBar playerHealthBar;
    ProgressBar playerStaminaBar;
    ProgressBar enemyHealthBar;

    ColorRect playerWinScreen;
    ColorRect playerLossScreen;

    AudioStreamPlayer backgroundAudio;

    public override void _Ready()
    {
        player = GetNode("Player");
        enemy = GetNode("Enemy");

        backgroundAudio = GetNode<AudioStreamPlayer>("BackgroundAudio");
        backgroundAudio.Playing = true;

        hasPlayerWon = false;
        hasPlayerLost = false;

        playerHealthBar = GetNode<ProgressBar>("UserInterface/PlayerStatus/PlayerHealth");
        playerStaminaBar = GetNode<ProgressBar>("UserInterface/PlayerStatus/PlayerStamina");
        enemyHealthBar = GetNode<ProgressBar>("UserInterface/EnemyHealth");

        playerWinScreen = GetNode<ColorRect>("UserInterface/PlayerWinScreen");
        playerWinScreen.Hide();
        playerLossScreen = GetNode<ColorRect>("UserInterface/PlayerLossScreen");
        playerLossScreen.Hide();

        OS.WindowFullscreen = true;
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().Quit();
        }
        if (Input.IsActionJustPressed("ui_accept") && (hasPlayerLost || hasPlayerWon))
        {
            GetTree().ReloadCurrentScene();
        }

        if (!hasPlayerLost && !hasPlayerWon)
        {
            playerHealthBar.Value = (((PlayerController)player).currentHealth / ((PlayerController)player).healthPoints) * 100;
            playerStaminaBar.Value = (((PlayerController)player).currentStamina / ((PlayerController)player).stamina) * 100;
            enemyHealthBar.Value = (((EnemyController)enemy).currentHealth / ((EnemyController)enemy).healthPoints) * 100;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!hasPlayerLost && !hasPlayerWon)
        {
            Transform playerTransform = ((KinematicBody)player).GlobalTransform;
            Vector3 enemyLocation = ((KinematicBody)enemy).GlobalTransform.origin;
            Transform playerLookAtTransform = playerTransform.LookingAt(enemyLocation, Vector3.Up);

            ((KinematicBody)player).GlobalTransform = playerLookAtTransform;
            ((KinematicBody)player).RotateY(Mathf.Pi);

            // The player behaviour only works when the negative is taken
            ((PlayerController)player).localBackward = -((KinematicBody)player).GlobalTransform.basis.z;
            ((PlayerController)player).localRight = -((KinematicBody)player).GlobalTransform.basis.x;

            if (!((PlayerController)player).isDodging)
            {
                Transform enemyTransform = ((KinematicBody)enemy).GlobalTransform;
                Vector3 playerLocation = ((KinematicBody)player).GlobalTransform.origin;
                Transform enemyLookAtTransform = enemyTransform.LookingAt(playerLocation, Vector3.Up);
                ((KinematicBody)enemy).GlobalTransform = ((KinematicBody)enemy).GlobalTransform.InterpolateWith(enemyLookAtTransform, lookAtSpeed);
                ((EnemyController)enemy).localBackward = ((KinematicBody)enemy).GlobalTransform.basis.z;
                ((EnemyController)enemy).localRight = ((KinematicBody)enemy).GlobalTransform.basis.x;
            }
            else if (((EnemyController)enemy).CurrentState != ((EnemyController)enemy).staggerState)
            {
                ((EnemyController)enemy).TransitionToState(((EnemyController)enemy).moveState);
            }
        }
    }

    private void _on_Player_PlayerDeath()
    {
        this.hasPlayerLost = true;
        this.hasPlayerWon = false;
        Transform playerGlobalTransform = ((Spatial)player).GlobalTransform;
        Spatial defeatSmokeInstance = (Spatial)(defeatSmoke.Instance());
        this.AddChild(defeatSmokeInstance);
        defeatSmokeInstance.GlobalTransform = playerGlobalTransform;
        defeatSmokeInstance.GetNode<Particles>("Smoke").Emitting = true;
        player.QueueFree();

        backgroundAudio.QueueFree();

        playerHealthBar.QueueFree();
        enemyHealthBar.QueueFree();
        playerStaminaBar.QueueFree();
        playerLossScreen.Show();
    }

    private void _on_Enemy_EnemyDeath()
    {
        this.hasPlayerWon = true;
        this.hasPlayerLost = false;
        Transform enemyGlobalTransform = ((Spatial)enemy).GlobalTransform;
        Spatial defeatSmokeInstance = (Spatial)(defeatSmoke.Instance());
        this.AddChild(defeatSmokeInstance);
        defeatSmokeInstance.GlobalTransform = enemyGlobalTransform;
        defeatSmokeInstance.GetNode<Particles>("Smoke").Emitting = true;
        enemy.QueueFree();

        backgroundAudio.Stream = playerWinBackgroundMusic;
        backgroundAudio.VolumeDb = 1f;
        backgroundAudio.Playing = true;

        playerHealthBar.QueueFree();
        enemyHealthBar.QueueFree();
        playerStaminaBar.QueueFree();
        playerWinScreen.Show();
    }
}
