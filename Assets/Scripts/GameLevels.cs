using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Informazioni del livello come:
/// - Spawnpoints
/// - Statistiche di ogni nemico spawnato
/// - mesh livello
/// Ogni livello viene istanziato nel GameManager :D
/// </summary>
public class GameLevels
{
    public string meshLevelName;
    private GameObject gameObject;
    
    public int enemiesToKill { get; private set; } // 
    public int enemiesBeforeBoss { get; private set; }  //
   
    ///-------- Spawns
    public float spawnRate { get; private set; } // Ogni quanto spawnera' nemici al secondo
    public GameObject[] spawns { get; private set; }
    public GameObject spawnpointPlayer;



    public GameLevels(string _meshLevelName)
    {
        meshLevelName = _meshLevelName;
    }



    ///---------------------------------
    ///------------- REFS
    ///---------------------------------
    private void InitReferences()
    {
        activeSpawners = new List<SpawnerEnemy>();
    }

    ///---------------------------------
    ///------------- SPAWN 
    ///---------------------------------
    #region SPAWN LOGIC

    //List<Enemy> inGameEnemies;
    List<SpawnerEnemy> activeSpawners;

    /// <summary>
    /// Questa funzione sceglie uno spawner.
    /// Ogni spawn viene suddiviso in ogni spawner, se ci sono 4 nemici da spawnare, 
    /// verra' effettuata una rotazione fra i 4 spawner, prima uno, poi un'altro,
    /// questo significa che ogni spawner spawnera' 1 nemico.
    /// Questa funzione permette questo.
    /// </summary>
    /// <returns></returns>
    private int spawnerIndex = 0;    
    
    /// <summary>
    /// Aggiungi uno spawner a quelli gia' esistenti.
    /// Setta gli spawnpoints dello spawner a quelli del livello.
    /// </summary>
    public void AddSpawner(SpawnerEnemy newSpawner)
    {
        newSpawner.spawnPoints = spawns;
        activeSpawners.Add(newSpawner);
    }

    GameObject GetSpawner()
    {
        spawnerIndex++;
        if(spawnerIndex >= spawns.Length)
            spawnerIndex = spawns.Length - 1;
        return spawns[spawnerIndex];
    }

    #endregion




    /// <summary>
    /// Rimuove ogni istanza creata, come la mesh.
    /// Mantiene le sue informazioni di base, serve solo per deallocare la roba
    /// che non serve piu' quando magari viene switchata
    /// </summary>
    void DestroyLevel()
    {

    }


    /// <summary>
    /// Verifica che tutto sia stato impostato correttamente
    /// </summary>
    void AssertInit()
    {
        Debug.AssertFormat(loaderLevelCalled != false,
            "Loader livello non impostato!", this);
    }
    
    
        bool loaderLevelCalled = false;
    public void LoadLevel()
    {
        gameObject = GameObject.Instantiate(
            Resources.Load<GameObject>("Prefabs/Mappe/" + meshLevelName));
        gameObject.transform.SetParent(GameManager.GI().mapWrapper.transform);
        loaderLevelCalled = true;
    
        //------- SPAWNPOINTS
        GameObject spawnpointsWrapper = UtilityShit.FindChildWithName(gameObject, "Spawnpoints Wrapper");
        spawns = new GameObject[spawnpointsWrapper.transform.childCount];
        for (int i = 0; i < spawns.Length; i++)
        {
            spawns[i] = spawnpointsWrapper.transform.GetChild(i).gameObject;
        }

        //------- HANDS
        AnimationHand.Init();
        spawnpointPlayer = UtilityShit.FindChildWithName(gameObject, "PlayerSpawnpoint");

        /// Resett la velocita' cosi' previeni cadute velocissime che passano oltre i muri
        GameManager.GI().plrScr.transform.position = spawnpointPlayer.transform.position;   
        Debug.Log(GameManager.GI().plrScr.transform.position + " + "   + spawnpointPlayer.transform.position);
        GameManager.GI().plrScr.rb.velocity = Vector3.zero;
        loaderLevelCalled = true;
        //GameManager
    }

    public void Awake()
    {
        InitReferences();
        //AssertInit();
    }

    //MAX_ENEMIES_IN_GAME
    const float MEIG = 10;

    public void Start()
    {
        AssertInit();
    }
    public void Update()
    {
        /// GLI SPAWNER SI AGGIORNANO DA SOLI
        int aliveEnemies = 0;
        int deadEnemies = 0;
        foreach (SpawnerEnemy spawner in activeSpawners)
        {
            // TODO: Tranquillamente ottimizzabile
            foreach(Enemy en in spawner.spawnedEnemies)
            {
                // se e' vivo
                if(en.fsm.currentState is not EnemyDeadState)
                {
                    aliveEnemies++;
                }
                else
                {
                    deadEnemies++;
                }
                
            }

            // TODO: sostituire l'operando a destra con una variabile costante 


            //Debug.Log("Spawner: " + spawner.spawnStats);
        }

        List<Enemy> enemiesToDestroy = new List<Enemy>();
        for(int y = 0; y < activeSpawners.Count && deadEnemies > MEIG; y++)
        {
            SpawnerEnemy spawner = activeSpawners[y];
            for (int x = 0; x < spawner.spawnedEnemies.Count && deadEnemies > MEIG; x++)
            {
                Enemy enem = spawner.spawnedEnemies[x];
                // Se e' morto
                if(enem.fsm.currentState is EnemyDeadState) 
                {
                    enemiesToDestroy.Add(enem); 
                }
            }
        }

        foreach(Enemy enem in enemiesToDestroy)
        {
            foreach(SpawnerEnemy spawner in activeSpawners)
            {
                // cerca nemico negli spawner
                if(spawner.spawnedEnemies.Remove(enem))
                {
                    GameObject.Destroy(enem.gameObject);
                    UtilityShit.Log("Nemico Eliminato!", Color.yellow);
                    deadEnemies--;
                    break;
                }
            }
            if (deadEnemies <= MEIG)
                break;
        }
        

    }

    public void OnDestroy()
    {

    }
}

// In pratica quando i nemici vengono spawnati una mano li porta
// verso il punto di spawn. Per evitare di istanziare 14241 mani per ogni player,
// semplicemente ne creo un tot allo startup e semplicemente li utilizzo quando sono
// "disponibili"

// Da riscrivere, completamente
// Fa veramente cagare

public class AnimationHand
{
    static public AnimationHand[] hands;
    static GameObject handsWrapper;
    public static int BUFFER_SIZE_HANDS { get; set; } = 10;
    private AnimationHand() { }
    public GameObject gameObject { get; private set; }
    private bool isAvailable;
    public Animator anim { get; private set; }

    // Per evitare che si disattivi nel caso in cui venga richiamato
    // istantaneamente dopo aver distrutto 
    private IEnumerable detachCoroutine;

    public static AnimationHand AttachHand(GameObject parentToAttach)
    {
        for (int i = 0; i < hands.Length; i++)
        {
            if (hands[i].isAvailable)
            {
                hands[i].gameObject.SetActive(true);
                hands[i].gameObject.transform.SetParent(parentToAttach.transform, false);
                hands[i].isAvailable = false;
                
                return hands[i];
            }
        }

        // Se nessuna mano e' disponibile
        // allora rip, significa che il buffer e' troppo piccolo per la velocita' di spawn
        // sisisisi ottimizzabile ma fanculo sucamela
        BUFFER_SIZE_HANDS++;
        Array.Resize(ref hands, hands.Length + 1);
        Debug.Log("Array resized!");
        hands[hands.Length - 1] = new AnimationHand();
        hands[hands.Length - 1].gameObject = GameObject.Instantiate(GameManager.GI().GetPrefab("Mano"));
        hands[hands.Length - 1].gameObject.SetActive(true);
        hands[hands.Length - 1].anim = hands[hands.Length - 1].gameObject.GetComponent<Animator>();
        hands[hands.Length - 1].isAvailable = false;
        hands[hands.Length - 1].gameObject.transform.SetParent(handsWrapper.transform);
            
        return hands[hands.Length - 1];
    }

    public static void DetachHand(MonoBehaviour mono, AnimationHand handToDetach)
    {
        handToDetach.gameObject.transform.SetParent(handsWrapper.transform);
        //handToDetach.isAvailable = true;
        mono.StartCoroutine(ResetAfter(handToDetach, 2f));
    }

    private static IEnumerator ResetAfter(AnimationHand hand, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        hand.isAvailable = true;
        hand.gameObject.SetActive(false);   
    }

    public static void Init()
    {
        handsWrapper = GameObject.Find("Hands Wrapper");

        hands = new AnimationHand[BUFFER_SIZE_HANDS];
        for (int i = 0; i < hands.Length; i++)
        {
            hands[i] = new AnimationHand();
            hands[i].gameObject = GameObject.Instantiate(GameManager.GI().GetPrefab("Mano"));
            hands[i].anim = hands[i].gameObject.GetComponent<Animator>();
            hands[i].isAvailable = true;
            hands[i].gameObject.transform.SetParent(handsWrapper.transform);
        }
    }
}
