using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawningState : EnemyBaseState
{
    const float timeToSpawn = 2f;
    public GameObject spawnPointToReach;
    private GameObject spawnPointOrigin;
    private AnimationHand animationHand;

    public EnemySpawningState() :
        base("Spawning")
    {
    }

    public override bool VerifySwitchConditions(EnemyFSM p)
    {
        // non puo' effettuare transizione a se' stesso
        return true;
    }

    public override void InitState(EnemyFSM p)
    {
        /// TODO, RENDI IMMATERIALI

        p.enemScr.stats.invincible = true;
        spawnPointToReach = p.enemScr.spawnPoint;
        Debug.AssertFormat(spawnPointToReach != null, "Spawnpoint non settato", this);

        p.enemScr.stats.invincible = true;
        // Prendi il figlio dello spawnpoint
        spawnPointOrigin = spawnPointToReach.transform.GetChild(0).gameObject;

        Debug.AssertFormat(spawnPointOrigin != null, 
            "SpawnPointOrigin non settata!", this);

        p.enemScr.transform.position =
            new Vector3(
                spawnPointOrigin.transform.position.x,
                spawnPointOrigin.transform.position.y,
                spawnPointOrigin.transform.position.z
                );
        // Attacca mano all'animazione
        p.enemScr.agent.enabled = false;
        animationHand = AnimationHand.AttachHand(p.enemScr.gameObject);
        animationHand.anim.SetBool("isClose", true);
        animationHand.anim.SetBool("isOpen", false);

        // Utilizziamo il bodyCollider per posizionare la mano sopra
        // la testa del player
        animationHand.gameObject.transform.position =
            new Vector3(p.enemScr.transform.position.x,

                p.enemScr.transform.position.y + 
                (p.enemScr.bodyCollider.bounds.extents.y) +
                    2f // Dovrebbe essere la meta' della grandezza della mano ma vbb
                ,
                p.enemScr.transform.position.z);

    }

    public override void UpdateState(EnemyFSM p)
    {
        p.enemScr.transform.position = Vector3.MoveTowards(p.enemScr.transform.position,
            spawnPointToReach.transform.position, 0.05f);
       
        if(Vector3.Distance(p.enemScr.transform.position, 
            spawnPointToReach.transform.position) <= 0.1f)
        {
            //Debug.Log("Changing to idle from spawn!");
            //p.SwitchState(p.idleState);
            if(!activated)
            {
                p.enemScr.StartCoroutine(waitToDrop(p, 1f));
            }
        }



    }

    bool activated = false;
    IEnumerator waitToDrop(EnemyFSM p, float timeToWait)
    {
        activated = true;
        yield return new WaitForSeconds(timeToWait);
        activated = false;
        p.enemScr.agent.enabled = true;
        
        if(p.currentState is not EnemyDeadState)
            p.SwitchState(p.approachState);
    }

    public override void FixedUpdateState(EnemyFSM p)
    {

    }

    public override void ExitState(EnemyFSM p)
    {
        // Apre la mano e fa cadere il player
        animationHand.anim.SetBool("isClose", false);
        animationHand.anim.SetBool("isOpen", true);
        // Stacca mano
        AnimationHand.DetachHand(p.enemScr, animationHand);

        p.enemScr.agent.enabled = true;
        //p.enemScr.StartCoroutine(p.enemScr.SetSpawnImmortality(2.5f));
        //animationHand = null;
        p.enemScr.stats.RefillHP();
    }

    public override void BodyOnTriggerEnter(EnemyFSM p, Collider other)
    {

    }
}
