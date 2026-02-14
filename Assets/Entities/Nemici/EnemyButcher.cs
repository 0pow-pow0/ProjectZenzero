using UnityEngine;  
using System;

public class EnemyButcher : Enemy
{
    ///Debug shit 

    public BaseStatsButcher baseStats { get; private set; } = new BaseStatsButcher();

    // Tempo prima di ricalcolare la destinazione 
    DeltaTimerAction recalculationTimer;
    const float RECALCULATION_TIME = 0.3f;

    private EnemyButcher()
    {
        stats = baseStats;
    }
    
    /// <summary>
    /// Crea GameObject con script e tutto cio' che necessita'
    /// </summary>
    /// <returns></returns>
    public static EnemyButcher Create()
    {
        GameObject newButcher = Instantiate( 
            GameManager.GI().GetPrefab("Macellaio"));
        return newButcher.GetComponent<EnemyButcher>(); 
    }


    private void Awake()
    {
        base.Init();
        stats.RefillHP();
        
        recalculationTimer = DeltaTimerAction.CreateDeltaTimerAction(SetAgentDestination,
            RECALCULATION_TIME,
            false);

        //spawnPoint = GameObject.Find("SpawnPoint1");

        //baseStats.HP = baseStats.maxHP;
        //fsm.SwitchState(fsm.spawningState);
        //Debug.Log("switch to spawn");
    }


        
    private void Update()
    {        
        base.Update();
        

        debugText.UpdateDebugText(fsm._debug_actual_state_text, transform.position, 
            GameManager.GI().mainSceneCamera);


    }

    /// <summary>
    /// Refilla anche gli HP.
    /// </summary>
    /// <param name="newStats"></param>
    public override void SetStats(BaseStats newStats)
    {
        base.SetStats(newStats);
        if(stats is BaseStatsButcher)
        {
            /// Aggiungi stats specifiche di BaseStatsButcher
            BaseStatsButcher statsButcher = (BaseStatsButcher)newStats;
            
            //baseStats.maxHP = statsButcher.maxHP;
            //baseStats.damageBodyCollision = statsButcher.damageBodyCollision;
            //baseStats.speed = statsButcher.speed;
            //agent.speed = baseStats.speed;
        }
        baseStats.RefillHP();
    }

    void OnTriggerEnter()
    {

    }
}

