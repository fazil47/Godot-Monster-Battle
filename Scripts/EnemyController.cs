using Godot;
using System;

public class EnemyController : KinematicBody
{
    #region Enemy Variables
    [Export]
    public float speed = 80f;
    [Export]
    public float fallAccelaration = 8f;
    [Export]
    public float dodgeSpeedMultiplier = 5f;
    [Export]
    public float minDistanceToPlayer = 25;
    [Export]
    public bool enableEnemyAI = true;
    [Export]
    public float healthPoints = 100;
    [Export]
    public float rangedAttackDamage = 20f;
    [Export]
    public float meleeAttackDamege = 10f;
    [Export]
    public float staggerVelocityMultiplier = 2.5f;
    [Export]
    public float evadeDodgeProb = 0.95f;
    #endregion

    public float currentHealth;

    public Vector3 velocity = Vector3.Zero;

    private EnemyState currentState;
    public EnemyState CurrentState
    {
        get { return currentState; }
    }

    #region Enemy States
    // public readonly EnemyIdle idleState = new EnemyIdle();
    public readonly EnemyMove moveState = new EnemyMove();
    public readonly EnemyAttack attackState = new EnemyAttack();
    public readonly EnemyStagger staggerState = new EnemyStagger();
    #endregion

    #region Enemy AI Timers
    public Timer attackMode;
    public Timer evadeMode;
    public Timer chaseMode;
    #endregion

    public bool isEvade = false;

    public MonsterScript monsterScript;
    public Vector3 localBackward;
    public Vector3 localRight;
    public float distanceToPlayer;

    public Node player;

    public bool isPlayerInMeleeArea = false;
    public Timer rangedAttackTimer;
    public Timer meleeAttackTimer;

    RandomNumberGenerator randomNumberGenerator = new RandomNumberGenerator();

    [Signal]
    public delegate void EnemyDeath();

    public override void _Ready()
    {
        currentHealth = healthPoints;
        player = GetNode("/root/Battle/Player");

        monsterScript = (MonsterScript)(GetNode("MonsterNode").GetChild(0));

        rangedAttackTimer = GetNode<Timer>("RangedAttackTimer");
        meleeAttackTimer = GetNode<Timer>("MeleeAttackTimer");

        attackMode = GetNode<Timer>("EnemyAITimers/AttackMode");
        evadeMode = GetNode<Timer>("EnemyAITimers/EvadeMode");
        chaseMode = GetNode<Timer>("EnemyAITimers/ChaseMode");

        Node camera = GetNode("MonsterNode").GetChild(0).GetNode("Camera");
        camera.QueueFree();

        if (enableEnemyAI)
        {
            TransitionToState(moveState);
            evadeMode.Start();
        }
    }

    public override void _Process(float delta)
    {
        if (enableEnemyAI)
        {
            currentState.Update(this, delta);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (this.currentHealth <= 0)
        {
            Die();
        }

        if (enableEnemyAI)
        {
            distanceToPlayer = this.GlobalTransform.origin.DistanceTo(((KinematicBody)(this.player)).GlobalTransform.origin);
            currentState.PhysicsUpdate(this, delta);
        }
    }

    public void TransitionToState(EnemyState state)
    {
        currentState = state;
        currentState.Enter(this);
    }

    public void _on_MeleeAttackArea_body_entered(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            isPlayerInMeleeArea = true;
            this.TransitionToState(this.attackState);
        }
    }

    public void _on_MeleeAttackArea_body_exited(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            isPlayerInMeleeArea = false;
        }
    }

    public void RangedHit()
    {
        if (currentState != staggerState)
        {
            this.currentHealth -= this.rangedAttackDamage;
            this.TransitionToState(this.staggerState);
            this.attackMode.Stop();
            this.evadeMode.Start();
            this.isEvade = true;
        }
    }

    public void MeleeHit()
    {
        this.isEvade = false;
        if (currentState != staggerState)
        {
            this.currentHealth -= this.meleeAttackDamege;
            this.TransitionToState(this.staggerState);
        }
    }

    public void Die()
    {
        EmitSignal(nameof(EnemyDeath));
    }

    public void _on_AttackMode_timeout()
    {
        randomNumberGenerator.Randomize();
        float evadeProb = randomNumberGenerator.Randf();
        if (evadeProb >= 0.5f)
        {
            isEvade = true;
            evadeMode.Start();
        }
        else
        {
            isEvade = false;
            chaseMode.Start();
        }
        this.TransitionToState(moveState);
    }

    public void _on_EvadeMode_timeout()
    {
        isEvade = false;
        attackMode.Start();
        this.TransitionToState(attackState);
    }

    public void _on_ChaseMode_timeout()
    {
        isEvade = true;
        attackMode.Start();
        this.TransitionToState(attackState);
    }

    private void _on_DodgeDurationTimer_timeout()
    {
        GetNode<Timer>("DodgeCooldownTimer").Start();
    }
}
