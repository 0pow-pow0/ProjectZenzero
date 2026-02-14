using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager inst;

    public static GameManager GI()
    {
        if(inst == null)
        {
            Debug.LogError("GameManager non istanziato");
            //inst = new GameManager();
            return null;
        }

        return inst;
    }
    private GameManager() {
    }

    public bool isGamePaused;

    public void PauseGame()
    {
        Time.timeScale = 0; 
        isGamePaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    #region RIFERIMENTI

    /// <summary>
    /// Molto importante come variabile, in quanto se le carte non vengono messe qui,
    /// in pratica non vengono renderizzare, poiche' si devono trovare all'interno di una canvas
    /// </summary>
    [NonSerialized] public Player plrScr;


    public Camera mainSceneCamera { get; private set; }

    ///---------- WRAPPERS
    public GameObject enemiesWrapper { get; private set; }
    public GameObject projectilesWrapper { get; private set; }
    public GameObject debugTextWrapper { get; private set; }
    public GameObject debugEnemyHPBarsWrapper { get; private set; }    
    public GameObject mapWrapper { get; private set; }
    public GameObject debugPoint { get; private set; }


    #region UI SHIT
    public GameObject UICardMimicWrapper { get; private set; }
    public GameObject CardManagerWrapper { get; private set; }
    [NonSerialized] public CardPowerUpsSelectionScreen powerUpSelScreen;
    [NonSerialized] public CardPowerUpsViewerScreen powerUpsViewerScreen;

    /// <summary>
    /// Mostra del testo in una data posizione dello schermo :D
    /// </summary>

    // \KFAE ShitLo instanzia il gamemanager per evitare flussi di codice errati
    /*public DebugText CreateDebugText()
    {
        //GameObject newTextObject = GameObject.Instantiate(default(GameObject));
        //DebugText newDeb = new DebugText(newTextObject);

        
    }*/

    #endregion

    private void InitReferences()
    {
        //Debug.Log(System.Environment.Version);

        debugTextWrapper = GameObject.Find("Debug Text Wrapper");
        debugEnemyHPBarsWrapper = GameObject.Find("DebugEnemyHPBarsWrapper");
        debugPoint = GameObject.Find("DebugPoint");
            
        mapWrapper = GameObject.Find("Map Wrapper");    
        mainSceneCamera = GameObject.Find("Camera").GetComponent<Camera>();

        enemiesWrapper = GameObject.Find("Enemies Wrapper");
        UICardMimicWrapper = GameObject.Find("UICardsMimicsWrapper");
        CardManagerWrapper = GameObject.Find("CardManagerWrapper");
        projectilesWrapper = GameObject.Find("ProjectilesWrapper");

        powerUpSelScreen = 
            GameObject.Find("CardPowerUpsViewer").GetComponent<CardPowerUpsSelectionScreen>();
        
        powerUpsViewerScreen = 
            GameObject.Find("CardPowerUpSelection").GetComponent<CardPowerUpsViewerScreen>();


        plrScr = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        Debug.Assert(plrScr != null, "IMPOSSIBILE TROVARE REFERENCE PLAYER!", this);
    }
    #endregion

    #region TIMER MANAGER 

    List<DeltaTimer> timers;
    List<Stopwatch> stopwatches;
    List<DeltaTimerAction> actionTimers;
    public void AddDeltaTimer(DeltaTimer timer)
    {
        if(timers.Contains(timer))
        {
            Debug.LogWarning("Stai provando ad aggiungere un timer gia' presente!");
            return;
        }

        timers.Add(timer);
    }

    public bool RemoveDeltaTimer(DeltaTimer timer)
    {
        if(timers.Remove(timer))
        {
            return true;
        }

        Debug.LogError("Impossibile trovare timer da rimuovere");
        return false;
    }

    public void AddStopwatch(Stopwatch stp)
    {
        if(stopwatches.Contains(stp))
        {
            Debug.LogWarning("Stai provando ad aggiungere uno stopwatch gia' presente!");
            return;
        }

        stopwatches.Add(stp);
    }

    public bool RemoveStopwatch(Stopwatch stp)
    {
        if (stopwatches.Remove(stp))
        {
            return true;
        }

        Debug.LogError("Impossibile trovare stopwatch da rimuovere");
        return false;
    }

    public void AddDeltaTimerAction(DeltaTimerAction timer)
    {
        if (actionTimers.Contains(timer))
        {
            Debug.LogWarning("Stai provando ad aggiungere un timer gia' presente!");
            return;
        }

        actionTimers.Add(timer);
    }

    public bool RemoveDeltaTimerAction(DeltaTimerAction timer)
    {
        if (actionTimers.Remove(timer))
        {
            return true;
        }

        Debug.LogError("Impossibile trovare timer da rimuovere");
        return false;
    }

    #endregion

    #region FILE MANAGER

    private Dictionary<string, Sprite> sprites;
    private Dictionary<string, GameObject> prefabs;
    private Dictionary<string, GameLevels> levels;
    private Dictionary<string, Material> materials;

    private void InitFileManager()
    {
        sprites = new Dictionary<string, Sprite>();
        prefabs = new Dictionary<string, GameObject>();
        levels = new Dictionary<string, GameLevels>();
        materials = new Dictionary<string, Material>();

        ///------------ CARTE
        sprites.Add("CardFrontPowerUpWeapon", Resources.Load<Sprite>("Assets/Cards/CardFrontBlack"));
        sprites.Add("CardFrontPowerUpStat", Resources.Load<Sprite>("Assets/Cards/CardFrontGreen"));
        sprites.Add("CardArtworkHeartRed", Resources.Load<Sprite>("Assets/Cards/ArtworkHeartRed"));
        sprites.Add("ButtonConfirmBackground", Resources.Load<Sprite>("Assets/Cards/ConfirmButtonBackground"));
        sprites.Add("CardArtworkSpeed", Resources.Load<Sprite>("Assets/Cards/ArtworkSpeed"));
        sprites.Add("CardArtworkDamage", Resources.Load<Sprite>("Assets/Cards/ArtworkDamage"));
        sprites.Add("CardArtworkHealthRegen", Resources.Load<Sprite>("Assets/Cards/ArtworkHealthRegen"));
        sprites.Add("CardArtworkAttackSpeed", Resources.Load<Sprite>("Assets/Cards/ArtworkAttackSpeed"));
        sprites.Add("CardArtworkUser", Resources.Load<Sprite>("Assets/Cards/ArtworkUserIcon"));

        ///------------ MATERIALI
        materials["PlayerBaseMaterial"] = Resources.Load<Material>("");


        prefabs["MimicTemplate"] = Resources.Load<GameObject>("Prefabs/Altro/MimicTemplate");

        ///------------ NEMICI
        prefabs["Macellaio"] = Resources.Load<GameObject>("Prefabs/Nemici/Macellaio/MacellaioInst");
        prefabs["Contadino"] = Resources.Load<GameObject>("Prefabs/Nemici/Contadino/Contadino");


        ///------------ ARMI
        prefabs["Arco"] = Resources.Load<GameObject>("Prefabs/Armi/Arco");
        prefabs["Cannone"] = Resources.Load<GameObject>("Prefabs/Armi/Cannone");
        prefabs["Balestra"] = Resources.Load<GameObject>("Prefabs/Armi/Balestra");


        ///------------ PROIETTILI
        //prefabs["ProjectileBastoneMagico"] = Resources.Load<GameObject>("Prefabs/ProjectileBastoneMagico");
        //prefabs["ProjectileCannone"] = Resources.Load<GameObject>("Prefabs/ProjectileCannone");
        prefabs["Dardo"] = Resources.Load<GameObject>("Prefabs/Armi/Dardo");
        prefabs["PallaDiCannone"] = Resources.Load<GameObject>("Prefabs/Armi/PallaDiCannone");
        prefabs["Freccia"] = Resources.Load<GameObject>("Prefabs/Armi/Freccia");

        //---- NEMICI


        ///------------ ALTRO
        prefabs["Mano"] = Resources.Load<GameObject>("Prefabs/Hand");
        prefabs["UIHPBar"] = Resources.Load<GameObject>("Prefabs/Nemici/UIHPBar");
        prefabs["EXPOrb"] = Resources.Load<GameObject>("Prefabs/Altro/EXPOrb");
        prefabs["DebugPoint"] = Resources.Load<GameObject>("Prefabs/Altro/DebugPoint");
        prefabs["CardButton"] = Resources.Load<GameObject>("Prefabs/Altro/CardButton");

        AssertFileManager();
    }

    private void AssertFileManager()
    {
        foreach(string key in sprites.Keys)
        {
            Debug.Assert(sprites[key] != null, key + " non e' stata caricata", this);
        }

        foreach (string key in prefabs.Keys)
        {
            Debug.Assert(prefabs[key] != null, key + " non e' stata caricata", this);
        }
    }

    public Sprite GetSprite(string spriteName)
    {
        // Se non contiene la chiave
        if (!sprites.ContainsKey(spriteName))
        {
            Debug.LogError("Sprite: " + spriteName + " non trovata!");
            return null;
        }

        return sprites[spriteName];
    }

    public GameObject GetPrefab(string prefabName)
    {
        // Se non contiene la chiave
        if (!prefabs.ContainsKey(prefabName))
        {
            Debug.LogError("Prefab: " + prefabName + " non trovata!");
            return null;
        }

        return prefabs[prefabName];
    }

    #endregion

    #region LEVEL MANAGER
    public GameLevels activeLevel { get; private set; }

    void InitLevels()
    {

        //// LEVELS
        /// Istanza delegata alla funzione LoadLevel, cosi' non si intasa la memoria
        levels["Livello1"] = new GameLevels("Livello1");
        activeLevel = levels["Livello1"];
        activeLevel.Awake();
        activeLevel.LoadLevel();

        levels["Livello1"].AddSpawner
            (
                new SpawnerEnemyButcher
                (
                    96,
                    0.8f,
                    new BaseStatsButcher
                    (
                        20,
                        5,
                        20,
                        0.5f
                    )
                )
            );
        levels["Livello1"].AddSpawner
            (
                new SpawnerEnemyFarmer
                (
                    36,
                    0.3f,
                    new BaseStatsFarmer
                    (
                        60,
                        3,
                        50,
                        1f  
                    )
                )
            );



        activeLevel = levels["Livello1"];


    }

    #endregion

    #region INPUTSHIT

    /// <summary>
    /// Sono i punti i cui il cursore sta puntando ALL'INTERNO della mappa di gioco
    /// </summary>
    public Vector2 lastHitPositionMouseXZ;
    public Vector3 lastHitPositionMouse;


    #endregion

    //public IProjectile CreateProjectile(GameObject _parent,
    //Vector3 _spawnPoint, Vector3 _travelDir,
    //float _speed, float _maxTravelDistance, Effect[] effects)
    //{
    //    IProjectile proj = new IProjectile();
    //    proj.parent = _parent;
    //    proj.spawnPoint = _spawnPoint;
    //    proj.travelDirection = _travelDir;
    //    proj.speed = _speed;
    //    proj.maxTravelDistance = _maxTravelDistance;

    //    // Attiva effetti!

    //    ProjectileManager.GI().projectiles.Add(proj);
    //    return proj;
    //}
    TextMeshProUGUI textElapsedTimeSinceLevelStart;
    Stopwatch elapsedTimeSinceLevelStart;
    void Awake()
    {
        // Per prevenire che questo script 
        inst = this;

        timers = new List<DeltaTimer>();
        stopwatches = new List<Stopwatch>();
        actionTimers = new List<DeltaTimerAction>();
        InitReferences();
        InitFileManager();
        //InitLevels();

        elapsedTimeSinceLevelStart = Stopwatch.CreateStopwatch();
        textElapsedTimeSinceLevelStart = GameObject.Find("DebugTimerSinceLevelStart").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        // Metto qui per fare in modo che tutto si inizializzi 
        InitLevels();
    }

    void Update()
    {
        textElapsedTimeSinceLevelStart.text = $"{elapsedTimeSinceLevelStart.elapsedTime:F0}";
        foreach(DeltaTimer timer in timers)
        {
            timer.Update();
        }

        foreach(Stopwatch stp in stopwatches)
        {
            stp.Update();
        }

        for(int i = actionTimers.Count - 1; i >= 0; i--)
        {
            // Chiamano la funzione removedeltatimeraction autonomamente
            actionTimers[i].Update();
        }
        //Debug.Log("Active Action Timers: " + actionTimers.Count);

        activeLevel.Update();

        //activeLevel.Update();
    }

    // void OnSceneChange()
    // dealloc timers and stopwatches
}