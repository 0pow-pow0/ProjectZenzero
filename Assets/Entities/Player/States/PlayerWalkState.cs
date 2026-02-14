using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState() :
        base("Walk")
    {

    }

    public override bool VerifySwitchConditions(PlayerFSM p)
    {
        return true;
    }

    public override void InitState(PlayerFSM p)
    {
        p.plrScr.anim.SetBool("isWalk", true);
    }

    public override void UpdateState(PlayerFSM p)
    {
        float x, z;

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");


        // Se non si sta muovendo, cambia stato
        if(x == 0 && z == 0)
        {
            p.SwitchState(p.idleState);
        }

        if(p.plrScr.InputAttack())
        {
            p.SwitchState(p.attackState);
        }


        Vector3 inputTranslation = new Vector3(x, 0, z);

        // Gli assi dati in input vengono ruotati in base all'asse di rotazione Y della camera
        Quaternion camYRot = Quaternion.Euler(0, p.plrScr.cam.transform.localEulerAngles.y, 0);

        inputTranslation = camYRot * inputTranslation;
        p.plrScr.rb.velocity =
            new Vector3(
                inputTranslation.x * p.plrScr.speed,
                p.plrScr.rb.velocity.y,
                inputTranslation.z * p.plrScr.speed);

        p.plrScr.RotatePlayer();

    }

    public override void FixedUpdateState(PlayerFSM p)
    {

    }

    public override void ExitState(PlayerFSM p)
    {
        // Resetta solo gli assi di movimento, non anche la gravita'
        p.plrScr.rb.velocity = new Vector3(0, p.plrScr.rb.velocity.y, 0);

        p.plrScr.anim.SetBool("isWalk", false);
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
