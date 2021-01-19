/*
Base interface for all states: it doesn't do anything in itself
but forces us to pass the right arguments to the methods below
and makes sure every State object had all of these methods.
*/

using Godot;
using System;

public abstract class EnemyState
{
    // [Signal]
    // public delegate void Finished(string nextStateName);

    public void PlayAnimation(EnemyController enemy, string animationName, bool loop = true)
    {
        Node monsterNode = enemy.GetNode("MonsterNode");
        Node characNode = monsterNode.GetChild(0);
        Animation anim = characNode.GetNode<AnimationPlayer>("AnimationPlayer").GetAnimation(animationName);
        anim.Loop = loop;
        characNode.GetNode<AnimationPlayer>("AnimationPlayer").Play(animationName);
    }

    public void StopAnimation(EnemyController enemy)
    {
        Node monsterNode = enemy.GetNode("MonsterNode");
        Node characNode = monsterNode.GetChild(0);
        characNode.GetNode<AnimationPlayer>("AnimationPlayer").Stop();
    }

    public abstract void Enter(EnemyController enemy);

    public abstract void Update(EnemyController enemy, float delta);

    public abstract void PhysicsUpdate(EnemyController enemy, float delta);

    // public void OnAnimationFinished(string animName)
    // {
    //     return;
    // }
}
