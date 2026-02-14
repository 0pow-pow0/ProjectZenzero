using System;
using UnityEngine;


public abstract class EnemyBaseState
{
    protected string _state_name;

    protected EnemyBaseState(string state_name)
    {
        _state_name = state_name;
    }

    //<summary>
    // Verifica se e' possibile passare al seguente stato
    //</summary>
    public abstract bool VerifySwitchConditions(EnemyFSM p);
    public abstract void InitState(EnemyFSM p);
    public abstract void UpdateState(EnemyFSM p);
    public abstract void FixedUpdateState(EnemyFSM p);
    public abstract void ExitState(EnemyFSM p);

    public abstract void BodyOnTriggerEnter(EnemyFSM p, Collider other);

    public void VerifyProjectilePlayerCollision(EnemyFSM p, Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("playerProjectileHitboxCollider"))
        {
            p.hurtState.SetDamage(
                -ProjectileUtility.GetProjectileInterfaceFromProjectileCollider(other).damage);
            p.SwitchState(p.hurtState);
        }
    }

    public string GetStateName() { return _state_name; }
};