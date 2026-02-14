using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stato in cui corre verso il player senza fare altro.
/// In realta' corre finche' non si stacca il NavMeshAgent, quindi anche se ci si trova
/// in un'altro stato, e' possibile correre.
/// Basta staccare il navMeshAgent. Molto utile visto che possiamo correre e attaccare, quindi avere
/// il behaviour della "corsa" indipendente da questo stato e' utile.
/// </summary>
public class EnemyApproachState : EnemyBaseState
{
    public EnemyApproachState() :
        base("Approach")
    {

    }

    public override bool VerifySwitchConditions(EnemyFSM p)
    {
        return true;
    }

    public override void InitState(EnemyFSM p)
    {
    }

    public override void UpdateState(EnemyFSM p)
    {
    }

    public override void FixedUpdateState(EnemyFSM p)
    {

    }

    public override void ExitState(EnemyFSM p)
    {
    }

    public override void BodyOnTriggerEnter(EnemyFSM p, Collider other)
    {
        VerifyProjectilePlayerCollision(p, other);
    }
}
