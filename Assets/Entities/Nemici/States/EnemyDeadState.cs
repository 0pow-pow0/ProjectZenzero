using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState() :
        base("Dead")
    {

    }

    public override bool VerifySwitchConditions(EnemyFSM p)
    {

        return true;
    }

    public override void InitState(EnemyFSM p)
    {
        Debug.Log("DEAD!");
        p.enemScr.anim.SetBool("isDead", true);
        p.enemScr.agent.isStopped = true;
        p.enemScr.hpBar.gameObject.SetActive(false);    
        p.enemScr.bodyCollider.gameObject.SetActive(false);
        p.enemScr.physicsColldier.gameObject.SetActive(false);  
        p.enemScr.GenerateEXPDropExp();
    }

    public override void UpdateState(EnemyFSM p)
    {
       
    }

    public override void FixedUpdateState(EnemyFSM p)
    {

    }

    public override void ExitState(EnemyFSM p)
    {
        // In pratica se rinasce
        //p.enemScr.anim.SetBool("isDead", false);
        //p.enemScr.anim.SetBool("isDead", true);
        //p.enemScr.agent.isStopped = true;
        //p.enemScr.agent.enabled = false;
        //p.enemScr.hpBar.gameObject.SetActive(false);
        //p.enemScr.bodyCollider.gameObject.SetActive(false);
    }

    public override void BodyOnTriggerEnter(EnemyFSM p, Collider other)
    {

    }
}
