using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Non e' proprio uno stato, diciamo che non dura neanche un frame.
/// Questo perche', da scelta di design, vogliamo che il player: perda vita, diventi immortale, 
/// continui normalmente sfruttando la finestra di immortalita'.
/// </summary>
public class PlayerHurtState : PlayerBaseState
{
    const float DURATION_HURT_INVINCIBILITY = 2f;
    
    // Timer che setta a false il bool dell'immortalita' dopo tot secondi
    DeltaTimerAction immortalityTimerAction;
    public int damage { get; private set; }
    public void SetDamage(int dm)
    {
        if (dm >= 0)
            UtilityShit.Log("Il valore del danno e' 0 o positivo", Color.yellow);

        damage = dm;
    }

    public PlayerHurtState() :
        base("Hurt")
    {

    }

    public override bool VerifySwitchConditions(PlayerFSM p)
    {
        // Se e' invincibile NON possiamo entrare in HURT STATE.
        // Altrimenti si'.
        return !p.plrScr.isInvincible;
    }

    public override void InitState(PlayerFSM p)
    {
        p.plrScr.anim.SetTrigger("isHurt");

        p.plrScr.AddHP(damage);
        //Color baseColor = p.plrScr.materials[0].color;
        //p.plrScr.materials[0].color = Color.gray;

        p.plrScr.isInvincible = true;
        immortalityTimerAction = DeltaTimerAction.CreateDeltaTimerAction(
            () =>
            {
                //p.plrScr.materials[0].color = baseColor;
                p.plrScr.isInvincible = false;
                //p.plrScr.anim.SetBool("isHurt", false );
            },
            DURATION_HURT_INVINCIBILITY,
            true
            );
        // Cambiamo subito stato, proprio perche' non ci serve l'hurt state se non per
        // le inizializzazioni fatte sopra. [ VEDI SUMMARY CLASSE PlayerHurtState ]
        p.SwitchState(p.idleState);
    }

    public override void UpdateState(PlayerFSM p)
    {
 

    }

    public override void FixedUpdateState(PlayerFSM p)
    {

    }

    public override void ExitState(PlayerFSM p)
    {
        damage = 0;
    }

    public override void BodyOnTriggerEnter(PlayerFSM p, Collider other)
    {
    }

    public override void BodyOnTriggerStay(PlayerFSM p, Collider other)
    {
    }

    public override void BodyOnTriggerExit(PlayerFSM p, Collider other)
    {
    }
}
