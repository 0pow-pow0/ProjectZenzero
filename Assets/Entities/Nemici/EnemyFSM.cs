using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyFSM : MonoBehaviour
{
    [NonSerialized] public string _debug_actual_state_text;


    // Reference utili per evitare di cercarle ogni volta
    [NonSerialized] public GameObject enem;
    [NonSerialized] public Enemy enemScr;
    RelayCollider relayColliderBody;

    public EnemyBaseState currentState { get; private set; }        
    #region FSM_STATES
    public EnemySpawningState spawningState { get; private set; }
    public EnemyIdleState idleState {  get; private set; }
    public EnemyApproachState approachState { get; private set; }
    public EnemyHurtState hurtState { get; private set; }
    public EnemyDeadState deadState { get; private set; }
    #endregion

    private EnemyFSM()
    {

    }

    void Awake()
    {
        enemScr = GetComponent<Enemy>();

        idleState = new EnemyIdleState();
        approachState = new EnemyApproachState();   
        hurtState = new EnemyHurtState();
        spawningState = new EnemySpawningState();
        deadState = new EnemyDeadState();

        relayColliderBody =
            UtilityShit.FindChildWithName(gameObject, "BodyCollider").GetComponent<RelayCollider>();
        relayColliderBody.relayOnTriggerEnter += RelayBodyOnTriggerEnter;
        relayColliderBody.relayOnTriggerStay += RelayBodyOnTriggerStay;
        relayColliderBody.relayOnTriggerExit += RelayBodyOnTriggerExit; 
    }

    void Start()
    {
        currentState = spawningState;
        currentState.InitState(this);
        
    }


    void Update()
    {
        currentState.UpdateState(this);
        _debug_actual_state_text = currentState.GetStateName();
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public bool SwitchState(EnemyBaseState newState)
    {
        if (!newState.VerifySwitchConditions(this))
        {
            return false;
        }
        //Debug.Log("Current State: " + currentState.GetStateName());
        //Debug.Log("New State: " + newState.GetStateName());
        currentState.ExitState(this);
        currentState = newState;
        currentState.InitState(this);

        return true;
    }

    #region RELAYS
    /// <summary>
    /// Chiamato all'OnTriggerEnter dello script principale Enemy.
    /// Questo perche' Enemy gestisce CHI LANCIA un OnTriggerEnter.
    /// Chiamato dal collider che riceve un'OnTriggerEnter DECISO DA ENEMY.
    /// </summary>
    public void RelayBodyOnTriggerEnter(Collider other)
    {
        currentState.BodyOnTriggerEnter(this, other);
    }

    public void RelayBodyOnTriggerStay(Collider other) { }
    public void RelayBodyOnTriggerExit(Collider other) { }
    #endregion


};