using System;
using UnityEngine;


public abstract class PlayerBaseState
{
    protected string _state_name;
    
    protected PlayerBaseState(string state_name)
    {
        _state_name = state_name;
    }

    //<summary>
    // Verifica se e' possibile passare al seguente stato
    //</summary>
    public abstract bool VerifySwitchConditions(PlayerFSM p);
    public abstract void InitState(PlayerFSM p);
    public abstract void UpdateState(PlayerFSM p);
    public abstract void FixedUpdateState(PlayerFSM p);
    public abstract void ExitState(PlayerFSM p);

    public abstract void BodyOnTriggerEnter(PlayerFSM p, Collider other);

    public abstract void BodyOnTriggerStay(PlayerFSM p, Collider other);

    public abstract void BodyOnTriggerExit(PlayerFSM p, Collider other);

    public string GetStateName() { return _state_name; } 

    // Capita tante volte di utilizzare questo frammento di codice all'interno degli stati.
    // Per questo
    public void VerifyEnemyBodyCollision(PlayerFSM p, Collider other)
    {
        //Debug.Log("COLLIDED WITH: " + other.gameObject.layer);
        if (other.gameObject.layer == LayerMask.NameToLayer("enemyBodyCollider"))
        {
            p.hurtState.SetDamage(- 
                Enemy.GetEnemyScriptFromBodyCollider(other).stats.damageBodyCollision);
            p.SwitchState(p.hurtState);
        }
    }

    public void VerifyEnemyProjectileCollision(PlayerFSM p, Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("enemyProjectileCollider"))
        {
            p.hurtState.SetDamage(-
                ProjectileUtility.GetProjectileInterfaceFromProjectileCollider(other).damage);
            p.SwitchState(p.hurtState);
        }
    }
};