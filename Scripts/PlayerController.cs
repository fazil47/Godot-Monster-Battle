using Godot;
using System;

public class PlayerController : KinematicBody
{
    #region Player Variables
    [Export]
    public float speed = 80f;
    [Export]
    public float fallAccelaration = 8f;
    [Export]
    public float dodgeSpeedMultiplier = 10f;
    [Export]
    public float healthPoints = 100f;
    [Export]
    public float rangedAttackDamage = 20f;
    [Export]
    public float meleeAttackDamege = 10f;
    [Export]
    public float staggerVelocityMultiplier = 2.5f;
    [Export]
    public float stamina = 100f;
    [Export]
    private float staminaRegenarationNormal = 12.5f;
    [Export]
    public float rangedAttackStamina = 12.5f;
    [Export]
    public float dodgeStamina = 5f;
    #endregion

    public float currentHealth;
    public float currentStamina;
    private float currentStaminaRegenaration;

    public Vector3 velocity = Vector3.Zero;

    private State currentState;
    public State CurrentState
    {
        get { return currentState; }
    }

    public Node enemy;

    #region Player States
    public readonly Idle idleState = new Idle();
    public readonly Move moveState = new Move();
    public readonly Attack attackState = new Attack();
    public readonly Stagger staggerState = new Stagger();
    #endregion

    public MonsterScript monsterScript;
    public Vector3 localBackward;
    public Vector3 localRight;

    public bool isDodging = false;

    public Timer rangedAttackTimer;
    public Timer meleeAttackTimer;

    public bool isEnemyInMeleeArea = false;

    public bool hasPlayerLost;
    public bool hasPlayerWon;

    public CameraScript cameraGimbal;

    RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

    [Signal]
    public delegate void PlayerDeath();

    public override void _Ready()
    {
        currentHealth = healthPoints;
        currentStamina = stamina;
        currentStaminaRegenaration = staminaRegenarationNormal;

        TransitionToState(idleState);
        enemy = GetNode("/root/Battle/Enemy");
        monsterScript = (MonsterScript)(GetNode("MonsterNode").GetChild(0));
        rangedAttackTimer = GetNode<Timer>("RangedAttackTimer");
        meleeAttackTimer = GetNode<Timer>("MeleeAttackTimer");

        cameraGimbal = GetNode<CameraScript>("CameraGimbal");
        Node camera = GetNode("MonsterNode").GetChild(0).GetNode("Camera");
        Transform cameraTransform = ((Spatial)camera).GlobalTransform;
        camera.GetParent().RemoveChild(camera);
        cameraGimbal.GetNode<Spatial>("InnerGimbal").AddChild(camera);
        ((Spatial)camera).GlobalTransform = cameraTransform;
    }

    public override void _Process(float delta)
    {
        if (currentStamina < stamina)
        {
            currentStamina = Mathf.Clamp(currentStamina + (currentStaminaRegenaration * delta), 0, stamina);
        }

        hasPlayerLost = GetNode<BattleController>("/root/Battle").hasPlayerLost;
        hasPlayerWon = GetNode<BattleController>("/root/Battle").hasPlayerWon;

        currentState.Update(this, delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        if (this.currentHealth <= 0)
        {
            Die();
        }
        if (((Input.IsActionJustPressed("attack") && currentStamina > rangedAttackStamina) || Input.IsActionJustPressed("attack_alternate")) && !hasPlayerWon)
        {
            if (rangedAttackTimer.TimeLeft == 0 && meleeAttackTimer.TimeLeft == 0)
            {
                this.TransitionToState(this.attackState);
            }
        }
        currentState.PhysicsUpdate(this, delta);
    }

    void Die()
    {
        ((EnemyController)enemy).enableEnemyAI = false;
        EmitSignal(nameof(PlayerDeath));
    }

    public void TransitionToState(State state)
    {
        currentState = state;
        currentState.Enter(this);
    }

    public void _on_MeleeAttackArea_body_entered(Node body)
    {
        if (body.IsInGroup("Enemy"))
        {
            this.isEnemyInMeleeArea = true;
        }
    }

    public void _on_MeleeAttackArea_body_exited(Node body)
    {
        if (body.IsInGroup("Enemy"))
        {
            this.isEnemyInMeleeArea = false;
        }
    }

    public void RangedHit()
    {
        if (currentState != staggerState)
        {
            this.currentHealth -= this.rangedAttackDamage;
            this.TransitionToState(this.staggerState);
        }
    }

    public void MeleeHit()
    {
        if (currentState != staggerState)
        {
            this.currentHealth -= this.meleeAttackDamege;
            this.TransitionToState(this.staggerState);
        }
    }
}
