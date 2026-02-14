using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Weapons;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState() :
        base("Attack")
    {

    }

    //Weapons.Animation currentAnimation; 
    WeaponAnimations currentAnimationIndex; 

    public override bool VerifySwitchConditions(PlayerFSM p)
    {
        return p.plrScr.weaponScr.CanAttack();
    }

    public override void InitState(PlayerFSM p)
    {
        p.plrScr.weaponScr.InitAttack();
    }

    public override void UpdateState(PlayerFSM p)
    {
        float x, z;

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

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

        if (!p.plrScr.weaponScr.UpdateAttack())
        {
            p.SwitchState(p.idleState);
        }

        if(!p.plrScr.InputAttack())
        {
            p.SwitchState(p.idleState);
        }

        if(p.plrScr.InputWalking())
        {

        }
        
    }

    public override void FixedUpdateState(PlayerFSM p)
    {

    }

    public override void ExitState(PlayerFSM p)
    {
        p.plrScr.weaponScr.ExitAttack();
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
