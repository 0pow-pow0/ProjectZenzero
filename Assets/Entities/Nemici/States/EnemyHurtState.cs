using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurtState : EnemyBaseState
{
    const float DURATION_HURT = 1.5f; // Durata stato di "Hurt"
    Stopwatch hurtStopwatch;
    public int damage { get; private set; } = 0;
    public void SetDamage(int dm)
    {
        if (dm >= 0)
            UtilityShit.Log("Il valore del danno e' 0 o positivo", Color.yellow);

        damage = dm;
    }

    public EnemyHurtState() :
        base("Hurt")
    {
        hurtStopwatch = Stopwatch.CreateStopwatch();
    }

    public override bool VerifySwitchConditions(EnemyFSM p)
    {
        return true;
    }

    public override void InitState(EnemyFSM p)
    {
        p.enemScr.anim.SetBool("isHurt", true);
        p.enemScr.agent.isStopped = true;   

        // Se non lo faccio si ferma in modo LERP
        p.enemScr.agent.velocity = Vector3.zero;
        p.enemScr.stats.AddHP(damage);
        hurtStopwatch.Reset();
    }

    public override void UpdateState(EnemyFSM p)
    {
        if(hurtStopwatch.elapsedTime >= DURATION_HURT)
        {
            p.SwitchState(p.approachState);
        }
    }

    public override void FixedUpdateState(EnemyFSM p)
    {

    }

    public override void ExitState(EnemyFSM p)
    {
        p.enemScr.anim.SetBool("isHurt", false);
        p.enemScr.agent.isStopped = false;
        //Debug.Log("Agent state after stopping: " + p.enemScr.agent.enabled);
        damage = 0;
    }

    
    public override void BodyOnTriggerEnter(EnemyFSM p, Collider other)
    {
        // Se collide di nuovo il proiettile
        // Danneggia nuovamente il nemico
        if (other.gameObject.layer == LayerMask.NameToLayer("playerProjectileHitboxCollider"))
        {
            p.enemScr.stats.AddHP(-
                ProjectileUtility.GetProjectileInterfaceFromProjectileCollider(other).damage);

            // Resetta timer hurt
            hurtStopwatch.Reset();
        }

    }

}
