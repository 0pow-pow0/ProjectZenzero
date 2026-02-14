using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootState : EnemyBaseState
{
    public EnemyShootState() :
        base("Shoot")
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

    }
}
