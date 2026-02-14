using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Transactions;

public class PlayerFSM : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _debug_actual_state_text;


    // Reference utili per evitare di cercarle ogni volta
    public GameObject plr;
    public Player plrScr;

    PlayerBaseState currentState;
    RelayCollider bodyCollider;
    #region FSM_STATES
    public PlayerIdleState idleState;
    public PlayerWalkState walkState;
    public PlayerAttackState attackState;
    public PlayerHurtState hurtState;
    #endregion

    private PlayerFSM()
    {

    }

    void Awake()
    {
        idleState = new PlayerIdleState();  
        walkState = new PlayerWalkState();
        attackState = new PlayerAttackState();
        hurtState = new PlayerHurtState();
        bodyCollider =
            UtilityShit.FindChildWithName(gameObject, "BodyCollider")
            .GetComponent<RelayCollider>(); 

        bodyCollider.relayOnTriggerEnter = BodyOnTriggerEnter;
        bodyCollider.relayOnTriggerStay = BodyOnTriggerStay;
        bodyCollider.relayOnTriggerExit = BodyOnTriggerExit;
        //bodyCollider.relayOnCollisionEnter = BodyOnCollisionEnter;
    }

    void Start()
    {
        currentState = idleState;
        currentState.InitState(this);
    }


    void Update()
    {
        currentState.UpdateState(this);
        _debug_actual_state_text.text = currentState.GetStateName();    
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public bool SwitchState(PlayerBaseState newState)
    {
        if(!newState.VerifySwitchConditions(this))
        {
            return false;
        }

        currentState.ExitState(this);
        currentState = newState;
        currentState.InitState(this);

        return true;
    }
    #region RELAYS

    public void BodyOnTriggerEnter(Collider other)
    {
        currentState.BodyOnTriggerEnter(this, other);
    }


    public void BodyOnTriggerStay(Collider other)
    {
        currentState.BodyOnTriggerStay(this, other);
    }

    public void BodyOnTriggerExit(Collider other)
    {
        currentState.BodyOnTriggerExit(this, other);
    }

    #endregion
};