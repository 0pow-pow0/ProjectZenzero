using UnityEngine;
using System;

public class EnemyFarmer : Enemy
{
    ///Debug shit 

    /// GONNA CUMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM
    /// MI ROMPO IL CAZZO A FARE LA CLASSE PER IL FARMER
    /// EHEHEHEHEH DEBUG PURPOSE GIUSTO??????????
    /// TODO
    public BaseStatsFarmer baseStats { get; private set; } = new BaseStatsFarmer();

    // Tempo prima di ricalcolare la destinazione 
    DeltaTimerAction recalculationTimer;
    const float RECALCULATION_TIME = 0.3f;

    private EnemyFarmer()
    {
        stats = baseStats;
    }

    /// <summary>
    /// Crea GameObject con script e tutto cio' che necessita'
    /// </summary>
    /// <returns></returns>
    public static EnemyFarmer Create()
    {
        GameObject newFarmer = Instantiate(
            GameManager.GI().GetPrefab("Contadino"));
        return newFarmer.GetComponent<EnemyFarmer>();
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

        //Debug.Log(baseStats);   
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
        if (stats is BaseStatsFarmer)
        {
            // Aggiungi stats specifiche di BaseStatsFarmer
            BaseStatsFarmer statsButcher = (BaseStatsFarmer)newStats;
        }

        baseStats.RefillHP();
    }

    void OnTriggerEnter()
    {

    }
}

