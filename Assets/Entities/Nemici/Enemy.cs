using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    ///Debug shit 
    public DebugText debugText;


    #region COSTANTI

    const float TIME_AFTER_DESTINATION_RECALCULATION = 10;

    #endregion


    [NonSerialized] public Slider hpBar;
    public BaseStats stats;
    [NonSerialized] public EnemyFSM fsm;
    [NonSerialized] public NavMeshAgent agent;
    [NonSerialized] public BoxCollider bodyCollider;
    [NonSerialized] public BoxCollider physicsColldier;
    [NonSerialized] public Animator anim;
    
    
    
    [NonSerialized] public GameObject spawnPoint; // Punto in cui viene generato

    public IEnumerable SetSpawnImmortality(float immortalityTime)
    {
        stats.invincible = true;
        yield return new WaitForSeconds(immortalityTime);
        stats.invincible = false;
    }

    public Enemy()
    {
    }

    /// <summary>
    /// Genera particella di EXP in base al valore contenuto nelle STATS
    /// </summary>
    public void GenerateEXPDropExp()
    {
        EXPOrb expOrb= Instantiate(GameManager.GI().GetPrefab("EXPOrb")).GetComponent<EXPOrb>();
        expOrb.transform.position = transform.position;
        Debug.Log("EXP: " + stats.droppedExp);
        expOrb.SetEXPValue(stats.droppedExp);
        expOrb.rb.AddForce(
            new Vector3(
                UnityEngine.Random.value * 5,
                0,
                UnityEngine.Random.value * 5
                )
            ,
            ForceMode.Impulse);
    }

    public void SetAgentDestination()
    {
        // Muoviti verso il playerz
        if (agent == null || !agent.enabled || agent.isStopped)
            return;

        agent.SetDestination(GameManager.GI().plrScr.transform.position);
    }

    public void Init()
    {
        debugText = new DebugText();
        fsm = GetComponent<EnemyFSM>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        bodyCollider = UtilityShit.FindChildWithName(gameObject, "BodyCollider").
            GetComponent<BoxCollider>();
        physicsColldier = UtilityShit.FindChildWithName(gameObject, "PhysicsCollider").
            GetComponent<BoxCollider>();
        hpBar = GameObject.Instantiate(
            GameManager.GI().GetPrefab("UIHPBar"),
            GameManager.GI().debugEnemyHPBarsWrapper.transform).GetComponent<Slider>();
        hpBar.transform.position =
            GameManager.GI().mainSceneCamera.WorldToScreenPoint(transform.position) -
            new Vector3(0, 20f);

    }

    public void OnDestroy()
    {
        GameObject.Destroy(debugText.gameObject);
        GameObject.Destroy(hpBar.gameObject);
        //GameObject.Destroy();
    }

    public void Update()
    {
        hpBar.transform.position =
            GameManager.GI().mainSceneCamera.WorldToScreenPoint(transform.position) -
            new Vector3(0, 20f);
        if(stats.maxHP != 0)
            hpBar.value = 
                (float)stats.HP / (float)stats.maxHP;
        if(fsm.currentState is not EnemyDeadState)
        {
            if(stats.HP <= 0)
            {
                fsm.SwitchState(fsm.deadState); 
            }
        }


    }

    public static Enemy GetEnemyScriptFromBodyCollider(Collider other)
    {
        Enemy enem = other.gameObject.GetComponentInParent<Enemy>();
        if(enem == null)
        {
            Debug.LogError("Impossibile trovare script nemico da collider!");
            return null;
        }

        return enem;
    }

    /// <summary>
    /// Setta solo maxhp, damage e speed.
    /// </summary>
    /// <param name="newStats"></param>
    public virtual void SetStats(BaseStats newStats)
    {
        stats.maxHP = newStats.maxHP;
        stats.damageBodyCollision = newStats.damageBodyCollision;
        stats.speed = newStats.speed;
        stats.SetDroppedEXP(newStats.droppedExp);
        agent.speed = stats.speed;
        stats.RefillHP();
    }

}

