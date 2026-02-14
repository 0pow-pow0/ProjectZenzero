using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState() :
        base("Idle")
    {

    }

    public override bool VerifySwitchConditions(PlayerFSM p)
    {
        return true;
    }

    public override void InitState(PlayerFSM p)
    {
        p.plrScr.anim.SetBool("isIdle", true);
    }

    public override void UpdateState(PlayerFSM p)
    {
        if(p.plrScr.InputWalking())
        {
            p.SwitchState(p.walkState);
        }

        if(p.plrScr.InputAttack())
        {
            p.SwitchState(p.attackState);
        }

        p.plrScr.RotatePlayer();
    }

    public override void FixedUpdateState(PlayerFSM p)
    {

    }

    public override void ExitState(PlayerFSM p)
    {
        p.plrScr.anim.SetBool("isIdle", false);
    }

    public override void BodyOnTriggerEnter(PlayerFSM p, Collider other)
    {
        VerifyEnemyBodyCollision(p, other);
        VerifyEnemyProjectileCollision(p, other);
    }

    public override void BodyOnTriggerStay(PlayerFSM p, Collider other)
    {
    }

    public override void BodyOnTriggerExit(PlayerFSM p, Collider other)
    {
    }
}
